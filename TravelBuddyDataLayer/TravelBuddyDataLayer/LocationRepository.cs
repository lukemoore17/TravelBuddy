using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddyDataLayer
{
    class LocationRepository : RepositoryBase
    {
        TravelBuddyEntities _db = new TravelBuddyEntities();
        public LocationRepository()
        {
        }

        // Returns one location
        public DataTable GetLocations(int id)
        {
            DataTable dtLocations = null;

            var s = _db.my_spS_LocationByID_OrAllLocationsByNull(id);

            dtLocations = LINQToDataTable(s);

            return dtLocations;
        }

        // NULL overflow method to return all locations
        public DataTable GetLocations()
        {
            DataTable dtLocations = null;

            var s = _db.my_spS_LocationByID_OrAllLocationsByNull(null);

            dtLocations = LINQToDataTable(s);

            return dtLocations;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public DataTable GetCoordinatesByLocationID(int id)
        {
            DataTable dtCoordinates = null;

            var s = _db.my_spS_LocationCoordinatesByLocationID(id);

            dtCoordinates = LINQToDataTable(s);

            return dtCoordinates;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //
        public DataTable GetCommentsByLocationID(int id)
        {
            DataTable dtComments = null;

            var s = _db.my_spS_CommentsByLocationID(id);

            dtComments = LINQToDataTable(s);

            return dtComments;
        }
    }
}
