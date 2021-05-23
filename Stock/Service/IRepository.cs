using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Service
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets a table
        /// </summary>
        IEnumerable<T> GetLists();

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Insert(T entity);


        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Update(T entity);

 

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Delete(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        bool Insert(IEnumerable<T> entities);


        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        int Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        int Delete(IEnumerable<T> entities);





    }
}
