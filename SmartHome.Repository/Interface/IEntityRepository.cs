using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using SmartHome.Domain;

namespace SmartHome.Repository.Interface
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        ISession Session { get; set; }

        /// <summary>
        /// Saves the IEntity object.
        /// </summary>
        /// <param name="type"></param>
        Guid Save(TEntity type);

        /// <summary>
        /// Deletes the IEntity object.
        /// </summary>
        /// <param name="type"></param>
        void Delete(TEntity type);

        /// <summary>
        /// Finds the IEntity by its associated ID, directly passed in.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        TEntity FindByID(Guid id);

        IList<TEntity> FindAll();

        IList<TEntity> FindAllAndSortBy(params string[] sortProperties);

        TEntity FindOneWhere(Expression<Func<TEntity, bool>> expression);

        IList<TEntity> FindManyWhere(Expression<Func<TEntity, bool>> expression);

        IList<TEntity> FindManyWhere(Expression<Func<TEntity, bool>> expression,
                                             params Expression<Func<TEntity, object>>[] sortCriteria);

        IList<TEntity> FindManyWhere(Expression<Func<TEntity, bool>> expression,
                                                    SortOrder sortOrder,
                                                    params Expression<Func<TEntity, object>>[] sortCriteria);

        IRepositoryQuery<TEntity> FindWhere(Expression<Func<TEntity, bool>> expression);
        IRepositoryQuery<TEntity> Find();

        IRepositoryRestriction<TEntity> FindWhereRestrictionOn(Expression<Func<TEntity, object>> expression);

        bool Exists(Guid entityID);
        int Count(Expression<Func<TEntity, bool>> expression);
    }
}
