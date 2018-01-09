using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TravelBuddy.code;

namespace TravelBuddy
{
    public partial class TravelBuddy : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[AppData.LOGGEDIN] != null)
            {
                bool loggedIn = (bool)Session[AppData.LOGGEDIN];
                if (loggedIn == true)
                {
                    HtmlAnchor destinations = new HtmlAnchor()
                    {
                        HRef = "./destinations.aspx",
                        InnerText = "My Destinations"
                    };
                    destinations.Attributes["class"] = "nav-link";

                    HtmlAnchor account = new HtmlAnchor()
                    {
                        HRef = "./account.aspx",
                        InnerText = "My Account"
                    };
                    account.Attributes["class"] = "nav-link";

                    HtmlAnchor logout = new HtmlAnchor()
                    {
                        HRef = "#",
                        InnerText = "Log Out"
                    };
                    logout.Attributes["class"] = "nav-link";
                    logout.ServerClick += Logout_Click;

                    menuPlaceholder.Controls.Add(destinations);
                    menuPlaceholder.Controls.Add(account);
                    menuPlaceholder.Controls.Add(logout);
                }
                else
                {
                    HtmlAnchor login = new HtmlAnchor()
                    {
                        HRef = "./login.aspx",
                        InnerText = "Log In"
                    };
                    login.Attributes["class"] = "nav-link";

                    menuPlaceholder.Controls.Add(login);
                }
            }
            else
            {
                HtmlAnchor login = new HtmlAnchor()
                {
                    HRef = "./login.aspx",
                    InnerText = "Log In"
                };
                login.Attributes["class"] = "nav-link";

                menuPlaceholder.Controls.Add(login);
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  EVENT HANDLERS  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        private void Logout_Click(object sender, EventArgs e)
        {
            if (Session[AppData.LOGGEDIN] != null)
            {
                bool loggedIn = (bool)Session[AppData.LOGGEDIN];

                if (loggedIn == true)
                {
                    Session.RemoveAll();

                    string gif = "<img src='./img/loader.gif' />";
                    var msg = AppData.CreateAlert("Logging you out..." + gif, "danger");
                    cpMainContent.Controls.Clear();
                    msgPlaceholder.Controls.Add(msg);

                    Response.AddHeader("REFRESH", "2;URL=./default.aspx");
                }
            }
        }
    }
}