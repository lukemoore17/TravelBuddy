using System.Web.UI.HtmlControls;

namespace TravelBuddy.code
{
    public class AppData
    {
        public const string USERID = "USERID";
        public const string USERNAME = "USERNAME";
        public const string EMAIL = "EMAIL";
        public const string FIRSTNAME = "FIRSTNAME";
        public const string LASTNAME = "LASTNAME";
        public const string COUNTRY = "COUNTRY";

        public const string LOGGEDIN = "LOGGEDIN";
        public const string LOGGEDINTIME = "LOGGEDINTIME";

        /// <summary>
        /// Return a Bootstrap alert Div
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="type">Type of bootstrap alert</param>
        /// <returns>Returns an HtmlGenericControl object as a Div</returns>
        public static HtmlGenericControl CreateAlert(string message, string type)
        {
            HtmlGenericControl msg = new HtmlGenericControl("div");
            msg.Attributes["role"] = "alert";

            switch (type)
            {
                case "danger":
                    msg.Attributes["class"] = "alert alert-danger";
                    break;
                case "success":
                    msg.Attributes["class"] = "alert alert-success";
                    break;
                case "warning":
                    msg.Attributes["class"] = "alert alert-warning";
                    break;
                default:
                    msg.Attributes["class"] = "alert alert-info";
                    break;
            }

            msg.InnerHtml = message;

            return msg;
        }
    }
}