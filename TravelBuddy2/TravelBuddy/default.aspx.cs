using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TravelBuddy.code;
using TravelBuddyDataLayer;

namespace TravelBuddy
{
    public partial class _default : Page
    {
        // Start log file
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<Favorite> GetFavoriteList()
        {
            TravellerController tc = new TravellerController();
            List<Favorite> Favorites = new List<Favorite>();
            try
            {
                Favorites = tc.GetTravellerFavoritesByTravellerID((int)Session[AppData.USERID]);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                var msg = AppData.CreateAlert("Your saved destinations could not be loaded at this time. Please refresh the page or try logging out and logging back in", "warning");
                err_Placeholder.Controls.Add(msg);
            }
            finally
            {
                tc.Dispose();
            }

            return Favorites;
        }

        /// <summary>
        /// Generic list of location objects
        /// </summary>
        public List<Location> Locations = new List<Location>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            if (Session[AppData.LOGGEDIN] != null)
            {
                bool loggedIn = (bool)Session[AppData.LOGGEDIN];
                if (loggedIn == true)
                {
                    HtmlGenericControl welcome = new HtmlGenericControl("div")
                    {
                        InnerHtml = "<h4>" + (string)Session[AppData.FIRSTNAME] + ", welcome back to TravelBuddy</h4>" +
                        "<p class='form-text'>Pick up where you left off...</p><br />"
                    };
                    msgPlaceholder.Controls.Add(welcome);
                }
                else
                {
                    HtmlGenericControl welcome = new HtmlGenericControl("div")
                    {
                        InnerHtml = "<h4>Welcome to TravelBuddy</h4>" +
                        "<p class='form-text'><a href='./login.aspx'>Log in</a> or <a href='./register.aspx'>create an account</a> " +
                        "to begin saving destinations and planning your trips.</p><br />"
                    };
                    msgPlaceholder.Controls.Add(welcome);
                }
            }
            else
            {
                HtmlGenericControl welcome = new HtmlGenericControl("div")
                {
                    InnerHtml = "<h4>Welcome to TravelBuddy</h4>" +
                        "<p class='form-text'><a href='./login.aspx'>Log in</a> or <a href='./register.aspx'>create an account</a> " +
                        "to begin saving destinations and planning your trips.</p><br />"
                };
                msgPlaceholder.Controls.Add(welcome);
            }

            LocationController locationController = new LocationController();
            try
            { 
                Locations = locationController.GetLocations();
                loc_Placeholder.Controls.Clear();
                foreach (Location loc in Locations)
                {
                    PopulateLocations(loc);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                var msg = AppData.CreateAlert("One of the locations could not be loaded", "warning");
                err_Placeholder.Controls.Add(msg);
            }
            finally
            {
                locationController.Dispose();
            }
        }

        /// <summary>
        /// Adds locations to the page
        /// </summary>
        /// <param name="location">Name or index of location variable/object</param>
        protected void PopulateLocations(Location location)
        {
            LoadLocations(location);
            LoadLocationModals(location);
        }

        protected virtual void LoadLocations(Location location)
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
            moreInfo.Attributes.Add("class", "btn btn-outline-primary");
            moreInfo.Attributes.Add("data-toggle", "modal");
            moreInfo.Attributes.Add("Data-target", "#cpMainContent_modal_" + location.Name);
            ButtonPanel.ContentTemplateContainer.Controls.Add(moreInfo);


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

                // Limit description to 100 chars
                string locationDescription = location.Description.Substring(0, 100);
                int breakIndex = locationDescription.LastIndexOf(' ');
                locationDescription = locationDescription.Substring(0, breakIndex);
                locationDescription += "...";

                string title = "<h1 class='card-title'>" + location.Name + "</h1>";               
                string description = "<p class='card-text'>" + locationDescription + "</p>";
                var content = title + description;
                LocationCardBody.InnerHtml = content;

                if (Session[AppData.LOGGEDIN] != null)
                {
                    bool loggedIn = (bool)Session[AppData.LOGGEDIN];

                    if (loggedIn == true)
                    {
                        // Check if location already exists in favorites
                        List<Favorite> TravellerFavorites = GetFavoriteList();
                        bool contains = TravellerFavorites.Any(f => f.LocationID == location.ID);
                        if (contains != true)
                        {
                            // Instantiate a new 'Add to Favorites' button
                            HtmlButton addFavorite = new HtmlButton
                            {
                                ID = "btnAdd_" + location.Name,
                                InnerText = "Add to My Destinations",
                            };
                            addFavorite.Attributes.Add("class", "btn btn-outline-success");
                            // Specify click event and create/call new event handler
                            addFavorite.ServerClick += (sender, EventArgs) => { AddDestinationBtn_Click(sender, EventArgs, location); };

                            // Add button to UpdatePanel
                            ButtonPanel.ContentTemplateContainer.Controls.Add(addFavorite);
                        }
                        else
                        {
                            Label fv_exists = new Label
                            {
                                ID = "fv_exists_" + location.Name,
                                Text = "Saved in your destinations",
                                CssClass = "badge badge-success",
                            };
                            fv_exists.Style.Add("font-size", "12pt");
                            fv_exists.Style.Add("padding", "5pt");

                            ButtonPanel.ContentTemplateContainer.Controls.Add(fv_exists);
                        }
                    }
                }

                // Add UpdatePanel to buttons div and card-body
                LocationButtons.Controls.Add(ButtonPanel);
                LocationCardBody.Controls.Add(LocationButtons);

                // Construct location card DIV
                LocationCard.Controls.Add(LocationCardBody);
                LocationContainer.Controls.Add(LocationCard);

                // Add location to placeholder
                loc_Placeholder.Controls.Add(LocationContainer);
            }
        }

        protected void LoadLocationModals(Location location)
        {
            #region Create Modal
            // Create a Bootstrap Modal to display full location information
            HtmlGenericControl Modal = new HtmlGenericControl("div")
            {
                ID = "modal_" + location.Name
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

            // Create an UpdatePanel to update Favorites without refreshing page
            UpdatePanel modal_panel = new UpdatePanel()
            {
                ID = "modal_panel" + location.Name
            };
            // Add unload event to prevent exception with dynamically created UpdatePanels
            modal_panel.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };

            // Add content to the Modal
            if (location != null)
            {
                string modal_loc_Description = location.Description;

                string title = "<h1>" + location.Name + "</h1>";
                HtmlButton ModalClose = new HtmlButton()
                {
                    InnerText = "x",
                };
                ModalClose.Attributes["class"] = "close";
                ModalClose.Attributes["data-dismiss"] = "modal";

                ModalHeader.Controls.Add(ModalClose);

                HtmlGenericControl LocationContent = new HtmlGenericControl("div");
                string image = "<img src = \"" + location.ImageLink + "\" id = \"" + "img_" + location.Name + "\" />";
                string description = "<p>" + modal_loc_Description + "</p><br/>";
                var content = title + image + description;
                LocationContent.InnerHtml = content;
                ModalBody.Controls.Add(LocationContent);



                ///////////////////////////////// Start: Comment Section /////////////////////////////////////
                HtmlGenericControl Comments = new HtmlGenericControl("div");

                //             ///////////////////// Start: Previous Comments ////////////////////////////
                UpdatePanel panel_Comments = new UpdatePanel()
                {
                    ID = "comments_panel_" + location.Name,
                };

                panel_Comments.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };

                HtmlGenericControl PreviousComments = new HtmlGenericControl("div");
                PreviousComments.Attributes["class"] = "list-group";

                LocationController lc = new LocationController();
                List<Comment> CommentsForLocation = null;
                try
                {
                    CommentsForLocation = lc.GetCommentsByLocationID(location.ID);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    var msg = AppData.CreateAlert("Could not load the comments for this location: " + location.Name, "warning");
                    err_Placeholder.Controls.Add(msg);
                }
                finally
                {
                    lc.Dispose();
                }

                if (CommentsForLocation.Count != 0)
                {
                    HtmlGenericControl CommentsHeader = new HtmlGenericControl("h3")
                    {
                        InnerText = "Comments",
                    };
                    //CommentsHeader.Attributes["class"] = "container container-fluid";
                    PreviousComments.Controls.Add(CommentsHeader);
                    foreach (Comment c in CommentsForLocation)
                    {
                        string dateString = c.CommentDate;
                        string format = "yyyyMMddHHmmss";
                        DateTime dateTime = DateTime.ParseExact(dateString, format, CultureInfo.CurrentCulture);
                        string date = dateTime.ToString("MMMM dd, yyyy");
                        string month = dateTime.Month.ToString();
                        string day = dateTime.Day.ToString();
                        string year = dateTime.Year.ToString();

                        var name = "<p><strong>" + c.UserName + "</strong> " + "<small class='text-muted float-right' font-weight='regular'>(" + date + ")</small>" + "</p>";
                        var comment = "<p>" + c.UserComment + "</p>";
                        var singleComment = name + comment;
                        HtmlGenericControl EachComment = new HtmlGenericControl("div")
                        {
                            InnerHtml = singleComment,
                        };
                        EachComment.Attributes["class"] = "list-group-item";
                        PreviousComments.Controls.Add(EachComment);
                    }
                }
                else
                {
                    var none = "<p><strong>There are no comments yet.</strong></p>";
                    HtmlGenericControl NoComments = new HtmlGenericControl("div")
                    {
                        InnerHtml = none,
                    };
                    PreviousComments.Controls.Add(NoComments);
                }

                // Add "previous" comments to update panel
                panel_Comments.ContentTemplateContainer.Controls.Add(PreviousComments);
                //            ///////////////////// End: Previous Comments ////////////////////////////

                //            //////////////////// Start: New User Comments ///////////////////////////
                HtmlGenericControl NewComment = new HtmlGenericControl("div");
                NewComment.Attributes["class"] = "form-group";

                HtmlGenericControl NewCommentHeader = new HtmlGenericControl("h4")
                {
                    InnerText = "Add Comment",
                };


                HtmlTextArea UserComment = new HtmlTextArea()
                {
                    ID = "Comment_" + location.Name,
                    Rows = 3,
                };
                UserComment.Attributes["name"] = "Comment";
                UserComment.Attributes["maxlength"] = "200";
                UserComment.Attributes["class"] = "form-control";
                UserComment.Attributes["placeholder"] = "Say something nice...";

                NewComment.Controls.Add(NewCommentHeader);
                NewComment.Controls.Add(UserComment);

                HtmlGenericControl CharsRemaining = new HtmlGenericControl("h6")
                {
                    ID = "CharsLeft_" + location.Name,
                };
                CharsRemaining.Attributes["class"] = "text-muted float-right";
                NewComment.Controls.Add(CharsRemaining);

                //Update panel for comment submit button
                UpdatePanel panel_Comment_Buttons = new UpdatePanel
                {
                    ID = "comment_buttons_panel_" + location.Name,
                    ChildrenAsTriggers = true,
                };
                panel_Comment_Buttons.Unload += (sender, EventArgs) => { UpdatePanel_Unload(sender, EventArgs); };
                Button modal_addComment = new Button
                {
                    ID = "modal_comment_btn" + location.Name,
                    Text = "Add Comment",
                };
                modal_addComment.Attributes["type"] = "button";
                modal_addComment.Attributes["class"] = "btn btn-primary";

                string commentId = DateTime.Now.ToString("yyyyMMddHHmmss");
                modal_addComment.Click += (sender, EventArgs) => { AddCommentBtn_Click(sender, EventArgs, location, new Comment(location.ID, commentId, (string)Session[AppData.USERNAME], UserComment.InnerText)); };

                panel_Comment_Buttons.ContentTemplateContainer.Controls.Add(modal_addComment);

                //            ///////////////////// End: New User Comments ////////////////////////////
                Comments.Controls.Add(panel_Comments);
                if (Session[AppData.LOGGEDIN] != null)
                {
                    bool loggedIn = (bool)Session[AppData.LOGGEDIN];

                    if (loggedIn == true)
                    {
                        Comments.Controls.Add(NewComment);
                        Comments.Controls.Add(panel_Comment_Buttons);
                    }
                    else
                    {
                        string loginLink = "<a href='./login.aspx'>log in</a>";
                        var msg = AppData.CreateAlert("Please " + loginLink + " to leave a comment", "warning");
                        Comments.Controls.Add(msg);
                    }
                } 
                else
                {
                    string loginLink = "<a href='./login.aspx'>log in</a>";
                    var msg = AppData.CreateAlert("Please " + loginLink + " to leave a comment", "warning");
                    Comments.Controls.Add(msg);
                }
                
                ModalBody.Controls.Add(Comments);
                ///////////////////////////////// End: Comment Section /////////////////////////////////////

                if (Session[AppData.LOGGEDIN] != null)
                {
                    bool loggedIn = (bool)Session[AppData.LOGGEDIN];

                    if (loggedIn == true)
                    {
                        // Check if location already exists in favorites
                        List<Favorite> TravellerFavorites = GetFavoriteList();
                        bool contains = TravellerFavorites.Any(f => f.LocationID == location.ID);
                        if (contains != true)
                        {
                            // Instantiate a new 'Add to Favorites' button
                            HtmlButton modal_addFavorite = new HtmlButton
                            {
                                ID = "modal_btn_" + location.Name,
                                InnerText = "Add to My Destinations",
                            };
                            modal_addFavorite.Attributes.Add("class", "btn btn-outline-success");
                            // Specify click event and create/call new event handler
                            modal_addFavorite.ServerClick += (sender, EventArgs) => { AddDestinationBtn_Click(sender, EventArgs, location); };

                            // Add button to UpdatePanel
                            modal_panel.ContentTemplateContainer.Controls.Add(modal_addFavorite);
                        }
                        else
                        {
                            Label modal_fv_exists = new Label
                            {
                                ID = "modal_fv_exists_" + location.Name,
                                Text = "Saved in your destinations",
                                CssClass = "badge badge-success",
                            };
                            modal_fv_exists.Style.Add("font-size", "12pt");
                            modal_fv_exists.Style.Add("padding", "5pt");

                            modal_panel.ContentTemplateContainer.Controls.Add(modal_fv_exists);
                        }
                    }
                }

                // Add buttons to footer
                ModalFooter.Controls.Add(modal_panel);

                // Construct Modal
                ModalContent.Controls.Add(ModalHeader);
                ModalContent.Controls.Add(ModalBody);
                ModalContent.Controls.Add(ModalFooter);
                ModalDialog.Controls.Add(ModalContent);
                Modal.Controls.Add(ModalDialog);

                // Add modal to placeholder
                loc_Placeholder.Controls.Add(Modal);
            }
            #endregion Create Modal
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  EVENT HANDLERS  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        protected void AddDestinationBtn_Click(object sender, EventArgs e, Location location)
        {
            TravellerController travellerController = new TravellerController();
            List<Favorite> TravellerFavorites = GetFavoriteList();
            try
            {
                bool contains = TravellerFavorites.Any(f => f.LocationID == location.ID);
                if (contains != true)
                {
                    bool favCreated = travellerController.CreateNewFavorite((int)Session[AppData.USERID], location.ID);
                    // Check if it is false
                }
                else
                {
                    string msg = "This location is already in your destinations!";
                    string alert = "alert(" + msg + ");";
                    Response.Write("<script>alert('" + msg + "');</script>");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                string msg = "There was an error. The location could not be added to your Destinations.";
                string alert = "alert(" + msg + ");";
                Response.Write("<script>alert('" + msg + "');</script>");
            }
            finally
            {
                travellerController.Dispose();
                TravellerFavorites.Clear();
                LoadLocations(location);
                LoadLocationModals(location);
            }
        }

        protected void AddCommentBtn_Click(object sender, EventArgs e, Location location, Comment comment)
        {
            TravellerController travellerController = new TravellerController();
            try
            {
                bool commentCreated = travellerController.CreateNewComment((int)Session[AppData.USERID], comment.LocationID, comment.CommentDate, comment.UserComment);
                // Check if it is false
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                string msg = "There was an error. Your comment could not be added.";
                string alert = "alert(" + msg + ");";
                Response.Write("<script>alert('" + msg + "');</script>");
            }
            finally
            {
                travellerController.Dispose();
                LoadLocationModals(location);
            }
        }

        /// <summary>
        /// This method handles the unload of an UpdatePanel when created dynamically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page),
                new object[] { sender as UpdatePanel });
        }
    }
}