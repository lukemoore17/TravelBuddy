using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddyDataLayer
{
    public class Traveller
    {
        private int _TravellerID;
        public int TravellerID
        {
            get { return _TravellerID; }
            set { _TravellerID = value; }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public Traveller()
        {
        }

        public void AddRowElementsToTraveller(DataRow row)
        {
            TravellerID = 0;
            FirstName = "";
            LastName = "";
            Country = "";

            TravellerID = Convert.ToInt32(row["TravellerID"]);
            FirstName = row["FirstName"].ToString().Trim();
            LastName = row["LastName"].ToString().Trim();
            Country = row["Country"].ToString().Trim();
            Email = row["Email"].ToString().Trim();
        }
    }
}
