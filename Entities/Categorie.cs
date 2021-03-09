using System.Collections.Generic;

namespace Entities
{
    public class Categorie : Entity
    {
        public string Libelle { get; set; }

        public List<Book> Books { get; set; }
    }
}