using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TravelBuddy.code;
using TravelBuddyDataLayer;

namespace TravelBuddy
{
    public partial class account : System.Web.UI.Page
    {
        // Start log file
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Get list of countries
        public static TravellerController tc = new TravellerController();
        public static List<string> Countries = tc.GetCountries();

        private void Page_PreInit(object sender, EventArgs e)
        {
            if (Session[AppData.LOGGEDIN] != null)
            {
                bool loggedIn = (bool)Session[AppData.LOGGEDIN];

                if (loggedIn == true)
                {
                }
                else
                {
                    Response.Write("Access denied. Redirecting...");
                    Response.AddHeader("REFRESH", "2;URL=./login.aspx");
                    Response.End();
                }
            }
            else
            {
                Response.Write("Access denied. Redirecting...");
                Response.AddHeader("REFRESH", "2;URL=./login.aspx");
                Response.End();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            /////////////////////// Register a javascript function to check if page is in Ajax postback ////////////////////
            var script = String.Format("var isAjaxPostback = {0};", ScriptManager.GetCurrent(Page).IsInAsyncPostBack.ToString().ToLower());
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "isAjaxPostback", script, true);

            if (!IsPostBack)
            {
                // Assign values of Personal Information section from user session data
                txtFirstName.Text = (string)Session[AppData.FIRSTNAME];
                txtLastName.Text = (string)Session[AppData.LASTNAME];
                lblCountry.Text = "Country: <strong>" + (string)Session[AppData.COUNTRY] + "</strong>";
                CountryListBox.DataSource = Countries;
                CountryListBox.DataBind();
            }
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  EVENT HANDLERS  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        /// <summary>
        /// Handles the click event of btnUpdatePI to update personal information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdatePI_Click(object sender, EventArgs e)
        {
            int id = (int)Session[AppData.USERID];
            string fn = txtFirstName.Text;
            string ln = txtLastName.Text;
            string country = CountryListBox.Text;

            TravellerController travellerController = new TravellerController();
            try
            {
                bool piUpdated = travellerController.UpdateTravellerInfo(id, fn, ln, country);

                if (piUpdated == true)
                {
                    Traveller user = travellerController.GetTravellerByUserName((string)Session[AppData.USERNAME]);
                    Session[AppData.FIRSTNAME] = user.FirstName;
                    Session[AppData.LASTNAME] = user.LastName;
                    Session[AppData.COUNTRY] = user.Country;

                    // Refresh data on page
                    txtFirstName.Text = (string)Session[AppData.FIRSTNAME];
                    txtLastName.Text = (string)Session[AppData.LASTNAME];
                    lblCountry.Text = "Country: <strong>" + (string)Session[AppData.COUNTRY] + "</strong>";

                    var msg = AppData.CreateAlert("Personal information updated successfully", "success");
                    updatePI_ErrPlaceholder.Controls.Add(msg);
                }
                else
                {
                    var msg = AppData.CreateAlert("Personal information could not be updated.", "danger");
                    updatePI_ErrPlaceholder.Controls.Add(msg);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                var msg = AppData.CreateAlert("There was an error while updating your information. Please try again.", "danger");
                updatePI_ErrPlaceholder.Controls.Add(msg);
            }
            finally
            {
                travellerController.Dispose();
            }
        }

        /// <summary>
        /// Handles the click event of btnUpdatePW to update user password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdatePW_Click(object sender, EventArgs e)
        {
            Validate("ValidateNewPW");
            if (!Page.IsValid) return;

            TravellerController travellerController = new TravellerController();
            try
            {
                string userName = (string)Session[AppData.USERNAME];
                string oldPW = txtOldPW.Text;
                int id = (int)Session[AppData.USERID];
                string newPW = txtNewPW.Text;

                bool oldPW_Verified = travellerController.VerifyCredentials(userName, oldPW);

                if (oldPW_Verified == false)
                {
                    var msg = AppData.CreateAlert("Incorrect current password, please try again", "danger");
                    updatePW_ErrPlaceholder.Controls.Add(msg);
                    return;
                }
                else if (newPW == oldPW)
                {
                    var msg = AppData.CreateAlert("New password cannot be same as current password, please choose something else", "danger");
                    updatePW_ErrPlaceholder.Controls.Add(msg);
                    return;
                }

                bool pwUpdated = travellerController.UpdateTravellerPassword(id, newPW);

                if (pwUpdated == true)
                {
                    var msg = AppData.CreateAlert("Password updated successfully", "success");
                    updatePW_ErrPlaceholder.Controls.Add(msg);
                }
                else
                {
                    var msg = AppData.CreateAlert("Password was not updated.", "danger");
                    updatePW_ErrPlaceholder.Controls.Add(msg);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

                var errMsg = AppData.CreateAlert("There was an error while updating your password. Please try again.", "danger");
                updatePW_ErrPlaceholder.Controls.Add(errMsg);
            }
            finally
            {
                travellerController.Dispose();
            }
        }

        /// <summary>
        /// Handles server-side validation of new password to ensure proper password length
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void NewPW_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Length >= 6)
                args.IsValid = true;
            else
            {
                args.IsValid = false;
            }
        }
    }
}