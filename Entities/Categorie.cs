using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// This is a exemple of entity for show how work the api sample
    /// </summary>
    public class Categorie : Entity
    {
        public string Libelle { get; set; }

        public List<Book> Books { get; set; }
    }
}