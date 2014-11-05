using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;
using SmartHome.Repository.Interface;

namespace SmartHome.Service.Interface.RepositoryService
{
    public interface IRepositoryService
    {
        TEntity FindByID<TEntity>(Guid ID) where TEntity : Entity;

        IList<TEntity> FindAll<TEntity>()
            where TEntity : Entity;

        IList<TEntity> FindAllAndSortBy<TEntity>(params string[] sortProperties)
            where TEntity : Entity;

        Guid Save<TEntity>(TEntity entity) where TEntity : Entity;

        void Delete<TEntity>(TEntity entity) where TEntity : Entity;

        void DeleteByID<TEntity>(Guid ID) where TEntity : Entity;

        void ForceFlush();

        void UpdateByNamedQuery(string name, object parameters);

        /// <summary>
        /// Note: DO NOT use this method for anything other than the TaxSlipRecoveryBatchProcess without talking to Justice.
        /// Some SQL commands require more time than allowed by our NHibernate configuration (command_timeout).
        /// This method can be used to set a custom timeout in miliseconds. 
        /// </summary>
        /// <param name="name">Name of the update-query to execute.</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <param name="timeout">Timeout in miliseconds</param>
        void UpdateByNamedQueryWithCustomTimeout(string name, object parameters, int timeout);

        IList<TEntity> FindByProc<TEntity>(string name, object parameters);
        TEntity FindOneByProc<TEntity>(string name, object parameters);

        TEntity FindOneWhere<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity;

        IList<TEntity> FindManyWhere<TEntity>(Expression<Func<TEntity, bool>> expression,
                                              params Expression<Func<TEntity, object>>[] sortCriteria) where TEntity : Entity;

        IList<TEntity> FindManyWhere<TEntity>(Expression<Func<TEntity, bool>> expression, SortOrder sortOrder,
                                              params Expression<Func<TEntity, object>>[] sortCriteria) where TEntity : Entity;

        IList<TEntity> FindManyWhere<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity;

        IRepositoryQuery<TEntity> FindWhere<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity;

        IRepositoryRestriction<TEntity> FindWhereRestrictionOn<TEntity>(Expression<Func<TEntity, object>> expression)
            where TEntity : Entity;

        IRepositoryQuery<TEntity> Find<TEntity>() where TEntity : Entity;

        void Rollback();

        bool Exists<TEntity>(Guid entityID) where TEntity : Entity;
        int Count<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity;
        bool Exists<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity;
    }
}
