﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TravelBuddyDataLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TravelBuddyEntities : DbContext
    {
        public TravelBuddyEntities()
            : base("name=TravelBuddyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Airport> Airport { get; set; }
        public virtual DbSet<Country> Country { get; set; }
    
        public virtual ObjectResult<my_spS_LocationByName_OrAllLocationsByNull_Result> my_spS_LocationByName_OrAllLocationsByNull(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<my_spS_LocationByName_OrAllLocationsByNull_Result>("my_spS_LocationByName_OrAllLocationsByNull", nameParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> my_spS_VerifyTraveller(string userName, string passwordHash)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordHashParameter = passwordHash != null ?
                new ObjectParameter("PasswordHash", passwordHash) :
                new ObjectParameter("PasswordHash", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("my_spS_VerifyTraveller", userNameParameter, passwordHashParameter);
        }
    
        public virtual int my_spI_CreateNewTraveller(string firstName, string lastName, string country, string userName, string email, string passwordHash)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var passwordHashParameter = passwordHash != null ?
                new ObjectParameter("PasswordHash", passwordHash) :
                new ObjectParameter("PasswordHash", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spI_CreateNewTraveller", firstNameParameter, lastNameParameter, countryParameter, userNameParameter, emailParameter, passwordHashParameter);
        }
    
        public virtual ObjectResult<string> my_spS_CheckIfTravellerUsernameExists(string userName)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("my_spS_CheckIfTravellerUsernameExists", userNameParameter);
        }
    
        public virtual int my_spU_UpdateTravellerEmail(Nullable<int> travellerID, string email)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spU_UpdateTravellerEmail", travellerIDParameter, emailParameter);
        }
    
        public virtual int my_spU_UpdateTravellerInfo(Nullable<int> travellerID, string firstName, string lastName, string country)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spU_UpdateTravellerInfo", travellerIDParameter, firstNameParameter, lastNameParameter, countryParameter);
        }
    
        public virtual int my_spU_UpdateTravellerPassword(Nullable<int> travellerID, string passwordHash)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var passwordHashParameter = passwordHash != null ?
                new ObjectParameter("PasswordHash", passwordHash) :
                new ObjectParameter("PasswordHash", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spU_UpdateTravellerPassword", travellerIDParameter, passwordHashParameter);
        }
    
        public virtual int my_spU_UpdateTravellerUsername(Nullable<int> travellerID, string userName)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spU_UpdateTravellerUsername", travellerIDParameter, userNameParameter);
        }
    
        public virtual ObjectResult<string> my_spS_VerifyTravellerEmail(Nullable<int> travellerID, string email)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("my_spS_VerifyTravellerEmail", travellerIDParameter, emailParameter);
        }
    
        public virtual int my_spD_DeleteTravellerByTravellerID(Nullable<int> travellerID)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spD_DeleteTravellerByTravellerID", travellerIDParameter);
        }
    
        public virtual ObjectResult<my_spS_LocationByID_OrAllLocationsByNull_Result> my_spS_LocationByID_OrAllLocationsByNull(Nullable<int> locationID)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<my_spS_LocationByID_OrAllLocationsByNull_Result>("my_spS_LocationByID_OrAllLocationsByNull", locationIDParameter);
        }
    
        public virtual ObjectResult<my_spS_CommentsByLocationID_Result> my_spS_CommentsByLocationID(Nullable<int> locationID)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<my_spS_CommentsByLocationID_Result>("my_spS_CommentsByLocationID", locationIDParameter);
        }
    
        public virtual int my_spI_NewComment(Nullable<int> travellerID, Nullable<int> locationID, string dateCreated, string comment)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var dateCreatedParameter = dateCreated != null ?
                new ObjectParameter("DateCreated", dateCreated) :
                new ObjectParameter("DateCreated", typeof(string));
    
            var commentParameter = comment != null ?
                new ObjectParameter("Comment", comment) :
                new ObjectParameter("Comment", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spI_NewComment", travellerIDParameter, locationIDParameter, dateCreatedParameter, commentParameter);
        }
    
        public virtual int my_spD_Favorite(Nullable<int> travellerID, Nullable<int> locationID)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spD_Favorite", travellerIDParameter, locationIDParameter);
        }
    
        public virtual int my_spI_NewFavorite(Nullable<int> travellerID, Nullable<int> locationID)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("my_spI_NewFavorite", travellerIDParameter, locationIDParameter);
        }
    
        public virtual ObjectResult<my_spS_TravellerFavoritesByTravellerID_Result> my_spS_TravellerFavoritesByTravellerID(Nullable<int> travellerID)
        {
            var travellerIDParameter = travellerID.HasValue ?
                new ObjectParameter("TravellerID", travellerID) :
                new ObjectParameter("TravellerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<my_spS_TravellerFavoritesByTravellerID_Result>("my_spS_TravellerFavoritesByTravellerID", travellerIDParameter);
        }
    
        public virtual ObjectResult<my_spS_LocationCoordinatesByLocationID_Result> my_spS_LocationCoordinatesByLocationID(Nullable<int> locationID)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<my_spS_LocationCoordinatesByLocationID_Result>("my_spS_LocationCoordinatesByLocationID", locationIDParameter);
        }
    
        public virtual ObjectResult<my_spS_TravellerByUserName_Result> my_spS_TravellerByUserName(string userName)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<my_spS_TravellerByUserName_Result>("my_spS_TravellerByUserName", userNameParameter);
        }
    }
}