<%@ Page Title="" Language="C#" MasterPageFile="~/TravelBuddy.master" AutoEventWireup="true" CodeBehind="destinations.aspx.cs" Inherits="TravelBuddy._destinations" %>
<%@ Import Namespace="TravelBuddy.code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
      /* Always set the map height explicitly to define the size of the div
       * element that contains the map. */
      #map {
        height: 500px;
        width: 100%;
        background-color: grey;
      }
      /* Optional: Makes the sample page fill the window. */
      /*html, body {
        height: 100%;
        margin: 0;
        padding: 0;
      }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    <div>
        <asp:Panel ID="favoritesPanel" runat="server">
            <asp:PlaceHolder ID="err_Placeholder" runat="server" />

            <h4><%=(string)Session[AppData.FIRSTNAME] %>, welcome to your Destinations page</h4>
            <p>Here you can view and manage your saved destinations.</p>
            <br />

            <%-- Google Maps API --%>
            <div id="map"></div>
            <div class="alert alert-info">
            <small class="form-text">Click on a location marker to zoom in</small>
            </div>
            <br />

            <%-- Iterate over locations --%>
            <div class="row">
                <asp:PlaceHolder ID="loc_Placeholder" runat="server" />
            </div>
        </asp:Panel>

        <%-- Hidden Coordinates for Google API --%>
        <div id="divCoords" style="display:none;">
            <asp:PlaceHolder runat="server" ID="coordsPlaceholder" />
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpJQuery" runat="server">
    <script>
        // Google Maps API
        var map;
        function initMap() {
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 2,
                center: { lat: 20, lng: 10 }
            });

            // Add a map marker for each location saved in My Destinations
            $(function () {
                $('div[name^=coord]').each(function () {
                    var latitude = parseFloat(this.getAttribute('data-lat'));
                    var longitude = parseFloat(this.getAttribute('data-lon'));
                    var coord = { lat: latitude, lng: longitude };
                    var marker = new google.maps.Marker({
                        position: coord,
                        map: map,
                        title: this.getAttribute('data-name')
                    });
                    // Add click listener for zoom
                    marker.addListener('click', function () {
                        map.setZoom(8);
                        map.setCenter(marker.getPosition());
                    });
                });
            });

            map.addListener('mouseout', function () {
                map.setZoom(2);
                map.panTo({ lat: 20, lng: 10 });
            });

            // Add event listeners
            //map.addListener('center_changed', function () {
            //    // 3 seconds after the center of the map has changed, pan back to the
            //    // marker.
            //    window.setTimeout(function () {
            //        map.panTo(marker.getPosition());
            //    }, 3000);
            //});

            
        }
    </script>

    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCWzXDSky-bMY80kJjbsWmQti5YM1vY5AQ&callback=initMap"></script>

    <script type="text/javascript">
        $(function () {
            // Character counter for comment box
            var text_max = 200;
            $('h6[id*="CharsLeft_"]').html(text_max + ' characters remaining');
            $('textarea[id*="Comment_"]').keyup(function () {
                var text_length = $(this).val().length;
                var text_remaining = text_max - text_length;

                $('h6[id*="CharsLeft_"]').html(text_remaining + ' characters remaining');
            });

            // Clear modal inputs on dismiss
            $('[data-dismiss=modal]').on('click', function (e) {
                var $t = $(this),
                    target = $t[0].href || $t.data("target") || $t.parents('.modal') || [];
                $(target)
                    .find("input,textarea,select")
                    .val('')
                    .end()
                    .find("input[type=checkbox], input[type=radio]")
                    .prop("checked", "")
                    .end();
                // Reset character count
                var text_max = 200;
                $('h6[id*="CharsLeft_"]').html(text_max + ' characters remaining');
            });

            // Prevent "enter" key in comment section
            $('textarea').keypress(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                }
            });
            $('input[type="text"]').keypress(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                }
            });

            //// Set name of flights modal title
            //$('button[id*="btnFindFlights_"]').click(function () {
            //    var title = this.id;
            //    title = title.substring(title.indexOf("Flights_") + 8);
            //    var titleid = this.getAttribute('data-id');
            //    $("div[id*='flightTitle" + titleid + "']").append("<h4>Find a flight to " + title + "</h4>");
            //});
        });
    </script>
</asp:Content>
