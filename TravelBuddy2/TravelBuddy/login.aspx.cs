using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TravelBuddyDataLayer;
using TravelBuddy.code;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TravelBuddy
{
    public partial class _login : System.Web.UI.Page
    {
        // Start log file
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string justRegistered = Request.QueryString["justregistered"];
                if (justRegistered == "true")
                {
                    var msg = AppData.CreateAlert("Your account had been created successfully. Please Log In.", "success");
                    messagePlaceholder.Controls.Add(msg);
                }
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            TravellerController travellerController = new TravellerController();
            try
            {
                string userName = txtUsername.Text;
                string password = txtPassword.Text;

                bool verified = travellerController.VerifyCredentials(userName, password);

                if (verified == true)
                {
                    Traveller user = travellerController.GetTravellerByUserName(userName);
                    user.UserName = userName;

                    if (user != null)
                    {
                        Session[AppData.USERID] = user.TravellerID;
                        Session[AppData.USERNAME] = user.UserName;
                        Session[AppData.FIRSTNAME] = user.FirstName;
                        Session[AppData.LASTNAME] = user.LastName;
                        Session[AppData.COUNTRY] = user.Country;
                        Session[AppData.EMAIL] = user.Email;
                        Session[AppData.LOGGEDIN] = true;
                        Session[AppData.LOGGEDINTIME] = DateTime.UtcNow;

                        Response.Redirect("default.aspx?", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        var msg = AppData.CreateAlert("There was an error while logging you in.", "danger");
                        loginErrPlaceholder.Controls.Add(msg);

                        string logMessage = "Error: User tried to login with verified credentials, but no user was created.";
                        logger.Error(logMessage);
                    }

                }
                else
                {
                    var msg = AppData.CreateAlert("Invalid username and/or password", "danger");
                    loginErrPlaceholder.Controls.Add(msg);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                travellerController.Dispose();
            }
        }
    }
}