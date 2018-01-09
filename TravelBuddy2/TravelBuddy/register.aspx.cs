using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using TravelBuddyDataLayer;
using System.Reflection;
using System.Web.UI.HtmlControls;

namespace TravelBuddy
{
    public partial class _register : System.Web.UI.Page
    {
        // Start log file
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static TravellerController tc = new TravellerController();
        public static List<string> Countries = tc.GetCountries();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CountryListBox.DataSource = Countries;
                CountryListBox.DataBind();
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  EVENT HANDLERS  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        protected void Register_Click(object sender, EventArgs e)
        {
            Validate();
            if (!Page.IsValid) return;

            System.Threading.Thread.Sleep(2000);

            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string country = CountryListBox.Text;
            string email = txtEmail.Text;
            string userName = txtUsername.Text;
            string password = txtPassword.Text;

            TravellerController travellerController = new TravellerController();
            try
            {
                bool userExists = travellerController.CheckIfUsernameExists(userName);
                if (userExists == true)
                {
                    HtmlGenericControl messageDiv = new HtmlGenericControl("div")
                    {
                        ID = "message"
                    };
                    messageDiv.Attributes["class"] = "alert alert-danger";
                    messageDiv.InnerText = "That username already exists. Please choose another.";

                    registerErrPlaceholder.Controls.Add(messageDiv);
                }
                else
                {

                    bool registered = travellerController.CreateNewTraveller(firstName, lastName, country, userName, email, password);

                    if (registered == true)
                    {
                        Response.Redirect("login.aspx?justregistered=true", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        HtmlGenericControl messageDiv = new HtmlGenericControl("div")
                        {
                            ID = "message"
                        };
                        messageDiv.Attributes["class"] = "alert alert-danger";
                        messageDiv.Attributes["role"] = "alert";
                        messageDiv.InnerText = "There was an error while created your account.";

                        messagePlaceholder.Controls.Add(messageDiv);
                    }
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

        protected void UsernameCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (args.Value.Length >= 4 && args.Value.Length <= 20) ? true : false;

            if (args.Value.Length < 4) CustomValidator1.ErrorMessage = "Username too short";
            if (args.Value.Length > 20) CustomValidator1.ErrorMessage = "Username too long";
        }

        protected void PasswordCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
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