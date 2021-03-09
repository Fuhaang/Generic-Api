using System;
using System.Collections.Generic;
using Xunit;

namespace Entities.UnitTests
{
    public class BookTests
    {
        [Fact]
        public void Book_HasID()
        {
            Common.HasProperty(typeof(Book), "Id");
        }

        [Fact]
        public void BookID_HasCorrectType()
        {
            Common.PropertyType(typeof(long), typeof(Book), "Id");
        }

        [Fact]
        public void Book_HasName()
        {
            Common.HasProperty(typeof(Book), "Name");
        }

        [Fact]
        public void BookName_HasCorrectType()
        {
            Common.PropertyType(typeof(string), typeof(Book), "Name");
        }

        [Fact]
        public void Book_HasPublicationDate()
        {
            Common.HasProperty(typeof(Book), "PublicationDate");
        }

        [Fact]
        public void BookPublicationDate_HasCorrectType()
        {
            Common.PropertyType(typeof(DateTime), typeof(Book), "PublicationDate");
        }

        [Fact]
        public void Book_HasCategories()
        {
            Common.HasProperty(typeof(Book), "Categories");
        }

        [Fact]
        public void BookCategories_HasCorrectType()
        {
            Common.PropertyType(typeof(List<Categorie>), typeof(Book), "Categories");
        }

        [Fact]
        public void Book_HasUser()
        {
            Common.HasProperty(typeof(Book), "User");
        }

        [Fact]
        public void BookUser_HasCorrectType()
        {
            Common.PropertyType(typeof(User), typeof(Book), "User");
        }

        [Fact]
        public void Book_HasUserID()
        {
            Common.HasProperty(typeof(Book), "UserId");
        }

        [Fact]
        public void BookUserID_HasCorrectType()
        {
            Common.PropertyType(typeof(long), typeof(Book), "UserId");
        }
    }
}
