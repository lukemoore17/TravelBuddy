using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddyDataLayer
{
    class TravellerRepository : RepositoryBase
    {
        TravelBuddyEntities _db = new TravelBuddyEntities();
        public TravellerRepository()
        {
        }

        public bool VerifyCredentials(string userName, string passwordHash)
        {
            bool verified = false;

            int nCount = 0;
            var foo = _db.my_spS_VerifyTraveller(userName, passwordHash);
            var d = foo.ToList();

            nCount = d.Any() ? Convert.ToInt32(d[0]) : 0;

            verified = (nCount == 1) ? true : false;

            return verified;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool VerifyTravellerEmail(int travellerID, string email)
        {
            bool verified = false;

            var s = _db.my_spS_VerifyTravellerEmail(travellerID, email);
            var v = s.ToList();

            int nCount = v.Any() ? Convert.ToInt32(v[0]) : 0;

            verified = (nCount == 1) ? true : false;

            return verified;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CheckIfUsernameExists(string userName)
        {
            bool exists = false;

            var s = _db.my_spS_CheckIfTravellerUsernameExists(userName);
            var v = s.ToList();

            int nCount = v.Any() ? Convert.ToInt32(v[0]) : 0;

            exists = (nCount == 1) ? true : false;
            if (nCount > 1)
            {
                // record that more than one user exists with that username
            }

            return exists;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CreateNewTraveller(string firstName, string lastName, string country, string userName, string email, string passwordHash)
        {
            bool created = false;

            _db.my_spI_CreateNewTraveller(firstName, lastName, country, userName, email, passwordHash);

            var s = _db.my_spS_VerifyTraveller(userName, passwordHash);
            var v = s.ToList();

            int nCount = v.Any() ? Convert.ToInt32(v[0]) : 0;

            created = (nCount == 1) ? true : false;

            return created;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool DeleteTraveller(int travellerID, string userName)
        {
            bool deleted = false;

            _db.my_spD_DeleteTravellerByTravellerID(travellerID);

            deleted = (CheckIfUsernameExists(userName) == false) ? true : false;

            return deleted;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public DataTable GetTravellerByUserName(string userName)
        {
            DataTable dtTraveller = null;

            var s = _db.my_spS_TravellerByUserName(userName);

            dtTraveller = LINQToDataTable(s);

            return dtTraveller;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerUsername(int travellerID, string userName)
        {
            bool updated = false;

            _db.my_spU_UpdateTravellerUsername(travellerID, userName);

            DataTable dtTraveller = GetTravellerByUserName(userName);
            updated = (dtTraveller.Rows.Count == 1) ? true : false;

            return updated;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerPassword(int travellerID, string userName, string passwordHash)
        {
            bool updated = false;

            _db.my_spU_UpdateTravellerPassword(travellerID, passwordHash);

            var s = _db.my_spS_VerifyTraveller(userName, passwordHash);
            var v = s.ToList();

            int nCount = v.Any() ? Convert.ToInt32(v[0]) : 0;

            updated = (nCount == 1) ? true : false;

            return updated;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerEmail(int travellerID, string email)
        {
            bool updated = false;

            _db.my_spU_UpdateTravellerEmail(travellerID, email);

            updated = (VerifyTravellerEmail(travellerID, email) == true) ? true : false;

            return updated;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerInfo(int travellerID, string firstName, string lastName, string country)
        {
            bool updated = false;

            int i = _db.my_spU_UpdateTravellerInfo(travellerID, firstName, lastName, country);

            if (i == -1)
                updated = true;

            return updated;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool UpdateTravellerPassword(int travellerID, string passwordHash)
        {
            bool updated = false;

            int i = _db.my_spU_UpdateTravellerPassword(travellerID, passwordHash);

            if (i == -1)
                updated = true;

            return updated;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CreateNewFavorite(int travellerID, int locationID)
        {
            bool created = false;

            int i = _db.my_spI_NewFavorite(travellerID, locationID);

            if (i == -1)
                created = true;

            return created;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool DeleteFavorite(int travellerID, int locationID)
        {
            bool deleted = false;

            int i = _db.my_spD_Favorite(travellerID, locationID);

            if (i == -1)
                deleted = true;

            return deleted;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //
        public DataTable GetTravellerFavoritesByTravellerID(int travellerID)
        {
            DataTable dtFavorites = null;

            var s = _db.my_spS_TravellerFavoritesByTravellerID(travellerID);

            dtFavorites = LINQToDataTable(s);

            return dtFavorites;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public bool CreateNewComment(int travellerID, int locationID, string dateCreated, string comment)
        {
            bool created = false;

            int i = _db.my_spI_NewComment(travellerID, locationID, dateCreated, comment);

            if (i == -1)
                created = true;

            return created;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public List<string> GetAirports()
        {
            var airports = from Airport in _db.Airport
                           select Airport.AiportCodeAndName;

            List<string> airportsList = airports.ToList();

            return airportsList;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public List<string> GetCountries()
        {
            var countries = from Country in _db.Country
                            select Country.Country1;

            List<string> countriesList = countries.ToList();

            return countriesList;
        }
    }
}
