using System.Collections.Generic;
using Xunit;


namespace Entities.UnitTests
{
    /// <summary>
    /// Unit test for the Categorie class (Sample for show how work the app)
    /// </summary>
    public class CategorieTests
    {
        [Fact]
        public void Categorie_HasID()
        {
            Common.HasProperty(typeof(Categorie), "Id");
        }

        [Fact]
        public void CategorieID_HasCorrectType()
        {
            Common.PropertyType(typeof(long), typeof(Categorie), "Id");
        }

        [Fact]
        public void Categorie_HasLibelle()
        {
            Common.HasProperty(typeof(Categorie), "Libelle");
        }

        [Fact]
        public void CategorieLibelle_HasCorrectType()
        {
            Common.PropertyType(typeof(string), typeof(Categorie), "Libelle");
        }

        [Fact]
        public void Categorie_HasBooks()
        {
            Common.HasProperty(typeof(Categorie), "Books");
        }

        [Fact]
        public void CategorieBooks_HasCorrectType()
        {
            Common.PropertyType(typeof(List<Book>), typeof(Categorie), "Books");
        }
    }
}
