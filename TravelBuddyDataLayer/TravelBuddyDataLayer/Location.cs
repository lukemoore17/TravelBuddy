using System;
using System.Data;

namespace TravelBuddy.code
{
    /// <summary>
    /// The Location class creates a process for travel locations to be fetched to the web page
    /// </summary>
    public class Location
    {
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _ImageLink;
        public string ImageLink
        {
            get { return _ImageLink; }
            set { _ImageLink = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// The Location constructor creates a new instance of the class 'Location'
        /// </summary>
        /// <param name="location">Name of the travel location</param>
        /// <param name="imagelink">URL where the location image is found</param>
        /// <param name="description">Description of the location</param>
        public Location(int id, string name, string imageLink, string description)
        {
            ID = id;
            Name = name;
            ImageLink = imageLink;
            Description = description;
        }

        public Location()
        {
        }

        public void AddRowElementsToLocation(DataRow row)
        {
            Name = "";
            ImageLink = "";
            Description = "";

            ID = Convert.ToInt32(row["LocationID"]);
            Name = row["Name"].ToString().Trim();
            ImageLink = row["ImageLink"].ToString().Trim();
            Description = row["Description"].ToString().Trim();
        }
    }
}