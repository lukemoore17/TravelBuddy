using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using TravelBuddy.code;

namespace TravelBuddyDataLayer
{
    public class TravellerController : ControllerBase
    {
        // Start log file
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private TravellerRepository _travellerRepository = null;

        public TravellerController()
        {
            _travellerRepository = new TravellerRepository();
            _repository = _travellerRepository;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //

        public bool VerifyCredentials(string userName, string password)
        {
            var passwordHash = Crypto.Hash(password);

            return _travellerRepository.VerifyCredentials(userName, passwordHash);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CheckIfUsernameExists(string userName)
        {
            return _travellerRepository.CheckIfUsernameExists(userName);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CreateNewTraveller(string firstName, string lastName, string country, string userName, string email, string password)
        {
            var passwordHash = Crypto.Hash(password);

            return _travellerRepository.CreateNewTraveller(firstName, lastName, country, userName, email, passwordHash);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool DeleteTraveller(int travellerID, string userName)
        {
            return _travellerRepository.DeleteTraveller(travellerID, userName);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //

        public bool UpdateTravellerPassword(int travellerID, string password)
        {
            var passwordHash = Crypto.Hash(password);

            return _travellerRepository.UpdateTravellerPassword(travellerID, passwordHash);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public Traveller GetTravellerByUserName(string userName)
        {
            Traveller t = null;
            DataTable dt = null;

            dt = _travellerRepository.GetTravellerByUserName(userName);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    t = new Traveller();

                    t.AddRowElementsToTraveller(dt.Rows[0]);
                }
                else
                {
                    logger.Error("TravellerController.cs :: line 72 :: GetTravellerByUserName return 0 or more than 1 rows", new SystemException());
                }
            }

            return t;

        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerUsername(int travellerID, string userName)
        {
            return _travellerRepository.UpdateTravellerUsername(travellerID, userName);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerPassword(int travellerID, string userName, string password)
        {
            string passwordHash = Crypto.Hash(password);
            return _travellerRepository.UpdateTravellerPassword(travellerID, userName, passwordHash);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerEmail(int travellerID, string email)
        {
            return _travellerRepository.UpdateTravellerEmail(travellerID, email);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerInfo(int travellerID, string firstName, string lastName, string country)
        {
            return _travellerRepository.UpdateTravellerInfo(travellerID, firstName, lastName, country);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CreateNewFavorite(int travellerID, int locationID)
        {
            return _travellerRepository.CreateNewFavorite(travellerID, locationID);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool DeleteFavorite(int travellerID, int locationID)
        {
            return _travellerRepository.DeleteFavorite(travellerID, locationID);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public List<Favorite> GetTravellerFavoritesByTravellerID(int studentID)
        {
            List<Favorite> Favorites = new List<Favorite>();
            Favorite f = null;
            DataTable dt = null;

            dt = _travellerRepository.GetTravellerFavoritesByTravellerID(studentID);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        f = new Favorite();
                        f.AddRowElementsToFavorite(row);
                        Favorites.Add(f);
                    }
                }
                else
                {
                    //record that 0 rows were returned.
                }
            }

            return Favorites;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CreateNewComment(int travellerID, int locationID, string dateCreated, string comment)
        {
            return _travellerRepository.CreateNewComment(travellerID, locationID, dateCreated, comment);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public List<string> GetAirports()
        {
            return _travellerRepository.GetAirports();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public List<string> GetCountries()
        {
            return _travellerRepository.GetCountries();
        }
    }
}
