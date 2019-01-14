namespace TrainingTask.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets the item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        T Get(int id);

        /// <summary>
        /// Deletes the item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);
    }
}
