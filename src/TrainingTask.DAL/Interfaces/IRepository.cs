namespace TrainingTask.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        /// <summary>
        /// Gets the item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        T Get(int id);

        /// <summary>
        /// Insert the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        int Create(T item);

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(T item);

        /// <summary>
        /// Deletes the item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);
    }
}
