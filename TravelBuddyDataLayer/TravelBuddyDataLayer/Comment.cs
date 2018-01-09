using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TravelBuddyDataLayer;

namespace TravelBuddy.code
{
    public class Comment
    {
        private int _LocationID;
        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }

        private string _CommentDate;
        public string CommentDate
        {
            get { return _CommentDate; }
            set { _CommentDate = value; }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _UserComment;
        public string UserComment
        {
            get { return _UserComment; }
            set { _UserComment = value; }
        }

        public Comment(int locationID, string datetime, string name, string comment)
        {
            LocationID = locationID;
            CommentDate = datetime;
            UserName = name;
            UserComment = comment;
        }

        public Comment()
        {
        }

        public void AddRowElementsToComment(DataRow row)
        {
            CommentDate = "";
            UserName = "";
            UserComment = "";

            LocationID = Convert.ToInt32(row["LocationID"]);
            CommentDate = row["DateCreated"].ToString().Trim();
            UserName = row["UserName"].ToString().Trim();
            UserComment = row["Comment"].ToString().Trim();
        }
    }
}