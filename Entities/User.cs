using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// This class reprensent a user of the application
    /// It's use the IdentityUser of Microsoft.AspNetCore.Identity
    /// https://docs.microsoft.com/fr-fr/dotnet/api/microsoft.aspnetcore.identity?view=aspnetcore-5.0
    /// https://docs.microsoft.com/fr-fr/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio
    /// </summary>
    public class User : IdentityUser
    {

        //You can add or delete property

        /// <summary>
        /// First name of user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Birth date of user
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Connected to the app ?
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Characters of the user
        /// Use for show how the app work
        /// </summary>
        public List<Book> Books { get; set; }
    }
}
