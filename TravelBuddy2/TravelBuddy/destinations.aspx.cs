using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TravelBuddy.code;
using TravelBuddyDataLayer;
using log4net;
using System.Reflection;
using System.Linq;
using System.Dynamic;

namespace TravelBuddy
{
    public partial class _destinations : _default
    {
        // Start log file
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //public static TravellerController tc = new TravellerController();
        //public static List<string> airports = tc.GetAirports();


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
                    Response.AddHeader("REFRESH", "0;URL=./login.aspx");
                    Response.End();
                }
            }
            else
            {
                Response.Write("Access denied. Redirecting...");
                Response.AddHeader("REFRESH", "0;URL=./login.aspx");
                Response.End();
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            LoadFavorites();
        }

        protected void LoadFavorites()
        {
            List<Favorite> FavoriteList = GetFavoriteList();
            List<Location> LocationList = new List<Location>();

            if (FavoriteList.Count != 0)
            {
                LocationController locationController = new LocationController();
                foreach (Favorite favorite in FavoriteList)
                {
                    try
                    {
                        Location x = locationController.GetLocations(favorite.LocationID);
                        LocationList.Add(x);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        continue;
                    }
                }
                locationController.Dispose();
            }
            else
            {
                Label noFavorites = new Label
                {
                    Text = "You have no favorites.",
                    CssClass = "panel panel-danger",
                };
                loc_Placeholder.Controls.Add(noFavorites);
            }

            if (LocationList.Count != 0)
            {
                foreach (Location location in LocationList)
                {
                    LoadLocations(location);
                    LoadLocationModals(location);
                }

                LocationController locationController = new LocationController();
                List<LocationGeo> LocationCoordinates = new List<LocationGeo>();

                foreach (Location location in LocationList)
                {
                    try
                    {
                        LocationGeo x = locationController.GetCoordinatesByLocationID(location.ID);
                        LocationCoordinates.Add(x);
                    }
                    catch (Exception ex)
                    {
                        // Error retrieving coordinates
                        logger.Error(ex.Message);
                    }
                }
                locationController.Dispose();

                if (LocationCoordinates.Count != 0)
                {
                    foreach (LocationGeo locationGeo in LocationCoordinates)
                    {
                        LoadLocationGeo(locationGeo);
                        LoadLocationFlightModals(locationGeo);
                    }
                }
            }
        }

        protected override void LoadLocations(Location location)
        {
            // Create a new HtmlTable
            HtmlGenericControl LocationContainer = new HtmlGenericControl("div");
            LocationContainer.Attributes["class"] = "col-md-4";
            HtmlGenericControl LocationCard = new HtmlGenericControl("div");
            LocationCard.Attributes["class"] = "card";
            //LocationCard.Attributes["style"] = "width:20rem;";

            HtmlGenericControl LocationCardBody = new HtmlGenericControl("div");
            LocationCardBody.Attributes["class"] = "card-body";
            HtmlGenericControl LocationButtons = new HtmlGenericControl("div");

            // Create an UpdatePanel to update Favorites without refreshing page
            UpdatePanel ButtonPanel = new UpdatePanel()
            {
                ID = "btnPanel_" + location.Name,
            };
            // Add unload event to prevent exception with dynamically created UpdatePanels
            ButtonPanel.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };

            // Add More Info button to update panel
            HtmlButton moreInfo = new HtmlButton()
            {
                ID = "btnMoreInfo_" + location.Name,
                InnerText = "More Info",
            };
            moreInfo.Attributes.Add("class", "btn btn-outline-primary btn-sm");
            moreInfo.Attributes.Add("data-toggle", "modal");
            moreInfo.Attributes.Add("Data-target", "#cpMainContent_modal_" + location.Name);
            ButtonPanel.ContentTemplateContainer.Controls.Add(moreInfo);

            // Add find-flights button
            HtmlButton findFlights = new HtmlButton()
            {
                ID = "btnFindFlights_" + location.Name,
                InnerText = "Find Flights"
            };
            findFlights.Attributes.Add("class", "btn btn-outline-info btn-sm");
            findFlights.Attributes.Add("data-toggle", "modal");
            findFlights.Attributes.Add("data-target", "#cpMainContent_flightsModal_" + location.ID);
            findFlights.Attributes.Add("data-id", location.ID.ToString());
            //findFlights.ServerClick += (sender, EventArgs) => { FindFlightsBtn_ServerClick(sender, EventArgs, location); };
            ButtonPanel.ContentTemplateContainer.Controls.Add(findFlights);

            // Add UpdatePanel to buttons div
            LocationButtons.Controls.Add(ButtonPanel);

            // Make sure location object is passed to PopulateLocations method
            if (location != null)
            {
                HtmlImage image = new HtmlImage()
                {
                    Src = location.ImageLink,
                    ID = "img_" + location.Name
                };
                image.Attributes["class"] = "card-img-top";
                LocationCard.Controls.Add(image);

                string title = "<h1 class='card-title'>" + location.Name + "</h1>";
                LocationCardBody.InnerHtml = title;

                if ((bool)Session[AppData.LOGGEDIN] == true)
                {
                    HtmlButton deleteFavorite = new HtmlButton
                    {
                        ID = "btnDelete_" + location.Name,
                        InnerText = "Remove Destination"
                    };
                    deleteFavorite.Attributes.Add("class", "btn btn-outline-danger btn-sm");
                    // Specify click event and create/call new event handler
                    deleteFavorite.ServerClick += (sender, EventArgs) => { DeleteDestinationBtn_Click(sender, EventArgs, location); };

                    //~~~~~~~~~~~~~~~~~~~~~~~~~ Removed this from update panel to force postback ~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
                    // Add button to UpdatePanel
                    // ButtonPanel.ContentTemplateContainer.Controls.Add(deleteFavorite);
                    LocationButtons.Controls.Add(deleteFavorite);
                }

                // Add Buttons to buttons div and card-body
                LocationCardBody.Controls.Add(LocationButtons);

                // Construct location card DIV
                LocationCard.Controls.Add(LocationCardBody);
                LocationContainer.Controls.Add(LocationCard);

                // Add location to placeholder
                loc_Placeholder.Controls.Add(LocationContainer);
            }
        }

        protected void LoadLocationFlightModals(LocationGeo location)
        {
            // Create a Bootstrap Modal to display full location information
            HtmlGenericControl Modal = new HtmlGenericControl("div")
            {
                ID = "flightsModal_" + location.ID
            };
            Modal.Attributes["class"] = "modal";
            HtmlGenericControl ModalDialog = new HtmlGenericControl("div");
            ModalDialog.Attributes["class"] = "modal-dialog";
            HtmlGenericControl ModalContent = new HtmlGenericControl("div");
            ModalContent.Attributes["class"] = "modal-content";
            HtmlGenericControl ModalHeader = new HtmlGenericControl("div");
            ModalHeader.Attributes["class"] = "modal-header";
            HtmlGenericControl ModalBody = new HtmlGenericControl("div");
            ModalBody.Attributes["class"] = "modal-body";
            HtmlGenericControl ModalFooter = new HtmlGenericControl("div");
            ModalFooter.Attributes["class"] = "modal-footer";

            // Add content to the Modal
            if (location != null)
            {
                
                HtmlButton ModalClose = new HtmlButton()
                {
                    InnerText = "x",
                };
                ModalClose.Attributes["class"] = "close";
                ModalClose.Attributes["data-dismiss"] = "modal";
                // Create an UpdatePanel for modal close
                UpdatePanel ModalPanelClose = new UpdatePanel()
                {
                    ID = "flightsModal_PanelClose" + location.ID,
                };
                ModalPanelClose.Attributes["class"] = "close";
                // Add unload event to prevent exception with dynamically created UpdatePanels
                ModalPanelClose.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };
                // Add close button to update panel
                ModalPanelClose.ContentTemplateContainer.Controls.Add(ModalClose);
                // Add to modal header
                ModalHeader.Controls.Add(ModalPanelClose);

                HtmlGenericControl title = new HtmlGenericControl("div")
                {
                    ID = "flightTitle" + location.ID,
                };
                title.Attributes["class"] = "form-group";
                ModalBody.Controls.Add(title);

                HtmlGenericControl FlightInfo = new HtmlGenericControl("div");
                FlightInfo.Attributes["class"] = "form-group";

                TextBox txtOrigin = new TextBox()
                {
                    CssClass = "form-control",
                    ID = "txtOrigin_" + location.ID,
                    Text = "PHL",
                };
                Label lblOrigin = new Label()
                {
                    ID = "lblOrigin_" + location.ID,
                    AssociatedControlID = txtOrigin.UniqueID,
                    Text = "Departing Airport <small class='text-muted'>" +
                            "(3 character " +
                            "<a target='_blank' href='http://www.webflyer.com/travel/milemarker/lookup.shtml'>" +
                            "airport code" +
                            "</a>)</small>",
                };

                FlightInfo.Controls.Add(lblOrigin);
                FlightInfo.Controls.Add(txtOrigin);

                TextBox txtDestination = new TextBox()
                {
                    CssClass = "form-control",
                    ID = "txtDestination_" + location.ID,
                    ReadOnly = true,
                    Text = location.AirportCode
                };
                Label lblDestination = new Label()
                {
                    ID = "lblDestination_" + location.ID,
                    AssociatedControlID = txtDestination.UniqueID,
                    Text = "Arrival Airport"
                };
                FlightInfo.Controls.Add(lblDestination);
                FlightInfo.Controls.Add(txtDestination);

                TextBox txtPassengers = new TextBox()
                {
                    CssClass = "form-control",
                    ID = "txtPassengers_" + location.ID,
                    Text = "1"
                };
                Label lblPassengers = new Label()
                {
                    ID = "lblPassengers_" + location.ID,
                    AssociatedControlID = txtPassengers.UniqueID,
                    Text = "Number of Passengers"
                };
                FlightInfo.Controls.Add(lblPassengers);
                FlightInfo.Controls.Add(txtPassengers);

                UpdatePanel dates = new UpdatePanel()
                {
                    ID = "datesPanel_" + location.ID,
                };
                // Add unload event to prevent exception with dynamically created UpdatePanels
                dates.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };

                ////////////// DEPART DATE /////////////////
                TextBox txtDepartDate = new TextBox()
                {
                    CssClass = "form-control",
                    ID = "txtDepartDate_" + location.ID,
                    Text = DateTime.Now.ToString("MM/dd/yyyy"),
                };
                txtDepartDate.Attributes["disabled"] = "disabled";
                Calendar departDate = new Calendar();
                departDate.ID = "departDate_" + location.ID;
                departDate.SelectionChanged += (sender, EventArgs) => { DepartDate_SelectionChanged(sender, EventArgs, txtDepartDate); };
                Label lblDepartDate = new Label()
                {
                    ID = "lblDepartDate_" + location.ID,
                    AssociatedControlID = txtDepartDate.UniqueID,
                    Text = "Depart Date"
                };
                dates.ContentTemplateContainer.Controls.Add(lblDepartDate);
                dates.ContentTemplateContainer.Controls.Add(txtDepartDate);
                dates.ContentTemplateContainer.Controls.Add(departDate);

                ////////////// RETURN DATE /////////////////
                TextBox txtReturnDate = new TextBox()
                {
                    CssClass = "form-control",
                    ID = "txtReturnDate_" + location.ID,
                    Text = DateTime.Now.ToString("MM/dd/yyyy"),
                };
                txtReturnDate.Attributes["disabled"] = "disabled";
                Calendar returnDate = new Calendar();
                returnDate.ID = "returnDate_" + location.ID;
                returnDate.SelectionChanged += (sender, EventArgs) => { ReturnDate_SelectionChanged(sender, EventArgs, txtReturnDate); };
                Label lblReturnDate = new Label()
                {
                    ID = "lblReturnDate_" + location.ID,
                    AssociatedControlID = txtReturnDate.UniqueID,
                    Text = "Return Date"
                };
                dates.ContentTemplateContainer.Controls.Add(lblReturnDate);
                dates.ContentTemplateContainer.Controls.Add(txtReturnDate);
                dates.ContentTemplateContainer.Controls.Add(returnDate);

                FlightInfo.Controls.Add(dates);

                ModalBody.Controls.Add(FlightInfo);

                Button FindFlights = new Button()
                {
                    ID = "btnFindFlights_" + location.ID,
                    Text = "Find Flights",
                };
                FindFlights.Attributes.Add("class", "btn btn-outline-success");

                ///////// CREATE DYNAMIC OBJECT TO SEND DATA TO EVENT HANDLER ///////////

                //Dictionary<string, object> properties = new Dictionary<string, object>
                //{
                //    { "Origin", txtOrigin.Text },
                //    { "Destination", txtDestination.Text },
                //    { "Passengers", txtPassengers.Text },
                //    { "DepartDate", txtDepartDate.Text },
                //    { "ReturnDate", txtReturnDate.Text }
                //};

                //dynamic dynObj = GetDynamicObject(properties);

                /////////////////////////////////////////////////////////////////////////

                // Set click event and pass flight query values from user
                FindFlights.Click += (sender, EventArgs) => 
                {
                    FindFlightsBtn_Click
                    (
                        sender, EventArgs, location, 
                        txtOrigin.Text, 
                        txtDestination.Text, 
                        txtDepartDate.Text, 
                        txtReturnDate.Text, 
                        txtPassengers.Text
                    );
                };

                // Create an UpdatePanel to update Favorites without refreshing page
                UpdatePanel ModalPanelBtns = new UpdatePanel()
                {
                    ID = "flightsModal_PanelBtns" + location.ID
                };
                // Add unload event to prevent exception with dynamically created UpdatePanels
                ModalPanelBtns.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };
                // Add FindFlights button to update panel
                ModalPanelBtns.ContentTemplateContainer.Controls.Add(FindFlights);
                // Add buttons to footer
                ModalFooter.Controls.Add(ModalPanelBtns);

                // Construct Modal
                ModalContent.Controls.Add(ModalHeader);
                ModalContent.Controls.Add(ModalBody);
                ModalContent.Controls.Add(ModalFooter);
                ModalDialog.Controls.Add(ModalContent);
                Modal.Controls.Add(ModalDialog);

                // Add modal to placeholder
                loc_Placeholder.Controls.Add(Modal);
            }
        }

        /// <summary>
        /// Creates a dynamic object with properties stored in a Dictionary
        /// </summary>
        /// <param name="properties">Properties of object in key-value pair</param>
        /// <returns></returns>
        public static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in properties)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            return dynamicObject;
        }

        private void ReturnDate_SelectionChanged(object sender, EventArgs eventArgs, Object txtReturnDate)
        {
            TextBox dp = (TextBox)txtReturnDate;
            Calendar cal = (Calendar)sender;
            dp.Text = cal.SelectedDate.ToString("MM/dd/yyyy");
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  EVENT HANDLERS  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        private void DepartDate_SelectionChanged(object sender, EventArgs e, Object txtDepartDate)
        {
            TextBox dp = (TextBox)txtDepartDate;
            Calendar cal = (Calendar)sender;
            dp.Text = cal.SelectedDate.ToString("MM/dd/yyyy");
        }

        protected void LoadLocationGeo(LocationGeo locationGeo)
        {
            HtmlGenericControl coordinate = new HtmlGenericControl("div");
            coordinate.Attributes["name"] = "coord";
            coordinate.Attributes["data-lat"] = locationGeo.Latitude.ToString();
            coordinate.Attributes["data-lon"] = locationGeo.Longitude.ToString();

            LocationController locationController = new LocationController();
            try
            {
                Location x = locationController.GetLocations(locationGeo.ID);
                coordinate.Attributes["data-name"] = x.Name;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                var msg = AppData.CreateAlert("Could not load Geo info for one of your destinations", "warning");
                err_Placeholder.Controls.Add(msg);
            }
            finally
            {
                locationController.Dispose();
            }

            coordsPlaceholder.Controls.Add(coordinate);
        }

        private void DeleteDestinationBtn_Click(object sender, EventArgs e, Location location)
        {
            TravellerController travellerController = new TravellerController();
            List<Favorite> TravellerFavorites = GetFavoriteList();
            try
            {
                bool contains = TravellerFavorites.Any(f => f.LocationID == location.ID);
                if (contains == true)
                {
                    bool deleted = travellerController.DeleteFavorite((int)Session[AppData.USERID], location.ID);
                    // Check if it is false
                }
                else
                {
                    string msg = "This location does not exist in your Destinations!";
                    string alert = "alert(" + msg + ");";
                    Response.Write("<script>alert('" + msg + "');</script>");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                var msg = AppData.CreateAlert("There was an error. The location could not be removed from your Destinations.", "danger");
                err_Placeholder.Controls.Add(msg);
                System.Threading.Thread.Sleep(2000);
            }
            finally
            {
                travellerController.Dispose();
                TravellerFavorites.Clear();
                Response.Redirect(Request.RawUrl);
            }
        }

        private void FindFlightsBtn_Click(object sender, EventArgs e, LocationGeo location, string origin, string destination, string departDate, string returnDate, string passengers)
        {
            //var sentObject = obj as IDictionary<string, Object>;

            string url = "https://www.orbitz.com/Flights-Search?trip=roundtrip&leg1=from:" +
                origin +
                ",to:" +
                destination +
                ",departure:" +
                departDate +
                "TANYT&leg2=from:" +
                destination +
                ",to:" +
                origin +
                ",departure:" +
                returnDate +
                "TANYT&" +
                "passengers=adults:" +
                passengers +
                "&mode=search";

            string script = string.Format("window.open('{0}');", url);

            //Page.ClientScript.RegisterStartupScript(this.GetType(),
            //    "newPage" + UniqueID, script, true);

            //Use this if post back is via Ajax
            ScriptManager.RegisterStartupScript(Page, Page.GetType(),
                "newPage" + UniqueID, script, true);
        }
    }
}