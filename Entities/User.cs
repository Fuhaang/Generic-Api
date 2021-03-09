using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class User : IdentityUser
    {

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
        /// </summary>
        public List<Book> Books { get; set; }
    }
}
