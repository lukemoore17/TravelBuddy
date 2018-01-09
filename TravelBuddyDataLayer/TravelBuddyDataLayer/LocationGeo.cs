using System;
using System.Data;

namespace TravelBuddy.code
{
    public class LocationGeo
    {
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private decimal _Latitude;
        public decimal Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        private decimal _Longitude;
        public decimal Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public string _AirportCode;
        public string AirportCode
        {
            get { return _AirportCode; }
            set { _AirportCode = value; }
        }

        public LocationGeo()
        {
        }

        public void AddRowElementsToLocationGeo(DataRow row)
        {
            ID = Convert.ToInt32(row["LocationID"]);
            Latitude = Convert.ToDecimal(row["Latitude"]);
            Longitude = Convert.ToDecimal(row["Longitude"]);
            AirportCode = row["AirportCode"].ToString().Trim();
        }
    }
}
