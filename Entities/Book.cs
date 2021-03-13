using System;
using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// This is a exemple of entity for show how work the api sample
    /// </summary>
    public class Book : Entity
    {
        public string Name { get; set; }

        public DateTime PublicationDate { get; set; }
        public List<Categorie> Categories { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
