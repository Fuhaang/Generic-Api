using System;
using System.Collections.Generic;
using Xunit;

namespace Entities.UnitTests
{
    public class UserTests
    {
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
