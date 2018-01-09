using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.code;

namespace TravelBuddyDataLayer
{
    public class LocationController : ControllerBase
    {
        private LocationRepository _locationRepository = null;

        public LocationController()
        {
            _locationRepository = new LocationRepository();
            _repository = _locationRepository;
        }

        // Get one location
        public Location GetLocations(int id)
        {
            Location location = null;
            DataTable dt = null;

            dt = _locationRepository.GetLocations(id);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                        location = new Location();
                        location.AddRowElementsToLocation(dt.Rows[0]);
                }
                else
                {
                    //record that 0 or more than 1 row was returned.
                }
            }

            return location;
        }

        // Get all Locations
        public List<Location> GetLocations()
        {
            List<Location> Locations = new List<Location>();
            Location loc = null;
            DataTable dt = null;

            dt = _locationRepository.GetLocations();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        loc = new Location();
                        loc.AddRowElementsToLocation(row);
                        Locations.Add(loc);
                    }
                }
                else
                {
                    //record that 0 rows were returned.
                }
            }

            return Locations;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public LocationGeo GetCoordinatesByLocationID(int id)
        {
            LocationGeo locationGeo = null;
            DataTable dt = null;

            dt = _locationRepository.GetCoordinatesByLocationID(id);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    locationGeo = new LocationGeo();
                    locationGeo.AddRowElementsToLocationGeo(dt.Rows[0]);
                }
                else
                {
                    //record that 0 or more than 1 row was returned.
                }
            }

            return locationGeo;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public List<Comment> GetCommentsByLocationID(int id)
        {
            List<Comment> Comments = new List<Comment>();
            Comment c = null;
            DataTable dt = null;

            dt = _locationRepository.GetCommentsByLocationID(id);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        c = new Comment();
                        c.AddRowElementsToComment(row);
                        Comments.Add(c);
                    }
                }
                else
                {
                    //record that 0 rows were returned.
                }
            }

            return Comments;
        }
    }
}
