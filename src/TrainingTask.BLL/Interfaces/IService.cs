using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        /// <summary>
        /// Deletes the item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Gets the item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        T Get(int id);
    }
}
