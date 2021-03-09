using System;
using System.Collections.Generic;
using Xunit;

namespace Entities.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void User_HasID()
        {
            Common.HasProperty(typeof(User), "Id");
        }

        [Fact]
        public void UserID_HasCorrectType()
        {
            Common.PropertyType(typeof(long), typeof(User), "Id");
        }

        [Fact]
        public void User_HasMail()
        {
            Common.HasProperty(typeof(User), "Mail");
        }

        [Fact]
        public void UserMail_HasCorrectType()
        {
            Common.PropertyType(typeof(string), typeof(User), "Mail");
        }

        [Fact]
        public void User_HasFirstName()
        {
            Common.HasProperty(typeof(User), "FirstName");
        }

        [Fact]
        public void UserFirstName_HasCorrectType()
        {
            Common.PropertyType(typeof(string), typeof(User), "FirstName");
        }

        [Fact]
        public void User_HasLastName()
        {
            Common.HasProperty(typeof(User), "LastName");
        }

        [Fact]
        public void UserLastName_HasCorrectType()
        {
            Common.PropertyType(typeof(string), typeof(User), "LastName");
        }

        [Fact]
        public void User_HasBirthDate()
        {
            Common.HasProperty(typeof(User), "BirthDate");
        }

        [Fact]
        public void UserBirthDate_HasCorrectType()
        {
            Common.PropertyType(typeof(DateTime), typeof(User), "BirthDate");
        }

        [Fact]
        public void User_HasPasswordHash()
        {
            Common.HasProperty(typeof(User), "PasswordHash");
        }

        [Fact]
        public void UserPasswordHash_HasCorrectType()
        {
            Common.PropertyType(typeof(byte[]), typeof(User), "PasswordHash");
        }
        [Fact]
        public void User_HasPasswordSalt()
        {
            Common.HasProperty(typeof(User), "PasswordSalt");
        }

        [Fact]
        public void UserPasswordSalt_HasCorrectType()
        {
            Common.PropertyType(typeof(byte[]), typeof(User), "PasswordSalt");
        }

        [Fact]
        public void User_HasIsConnected()
        {
            Common.HasProperty(typeof(User), "IsConnected");
        }

        [Fact]
        public void UserIsConnected_HasCorrectType()
        {
            Common.PropertyType(typeof(bool), typeof(User), "IsConnected");
        }

        [Fact]
        public void User_HasBooks()
        {
            Common.HasProperty(typeof(User), "Books");
        }

        [Fact]
        public void UserBooks_HasCorrectType()
        {
            Common.PropertyType(typeof(List<Book>), typeof(User), "Books");
        }
    }
}
