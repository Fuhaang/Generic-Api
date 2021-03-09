using System;
using System.Collections.Generic;

namespace Entities
{
    public class User : Entity
    {
        /// <summary>
        /// Mail of user
        /// </summary>
        public string Mail { get; set; }

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
        /// HashedPassword
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// Salt for the hashed password
        /// </summary>
        public byte[] PasswordSalt { get; set; }

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
