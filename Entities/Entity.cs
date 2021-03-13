namespace Entities
{
    /// <summary>
    /// This class need to be a mother of all your entity
    /// if your entity need to be in the database
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        public long Id { get; set; }
    }
}
