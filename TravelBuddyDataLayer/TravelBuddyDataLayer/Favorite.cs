using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TravelBuddyDataLayer;

namespace TravelBuddy.code
{
    /// <summary>
    /// The Favorite class creates a process for favorite travel locations to be fetched to the web page
    /// </summary>
    public class Favorite
    {
        private int _TravellerID;
        public int TravellerID
        {
            get { return _TravellerID; }
            set { _TravellerID = value; }
        }

        private int _LocationID;
        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }

        public Favorite(int travellerID, int locationID)
        {
            TravellerID = travellerID;
            LocationID = locationID;
        }

        public Favorite()
        {
        }

        public void AddRowElementsToFavorite(DataRow row)
        {
            TravellerID = Convert.ToInt32(row["TravellerID"]);
            LocationID = Convert.ToInt32(row["LocationID"]);
        }
    }
}