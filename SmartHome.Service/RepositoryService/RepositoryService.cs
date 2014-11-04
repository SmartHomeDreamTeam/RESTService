using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using SmartHome.Domain;
using SmartHome.Repository.Interface;
using SmartHome.Service.Interface.NHibernateService;
using SmartHome.Service.Interface.RepositoryService;

namespace SmartHome.Repository.RepositoryService
{
    public class RepositoryService : IRepositoryService
    {
        private readonly INHibernateService nHibernateService;

        public RepositoryService(INHibernateService nHibernateService)
        {
            this.nHibernateService = nHibernateService;
        }

        public void ForceFlush()
        {
            if (nHibernateService.CurrentSession != null)
            {

                nHibernateService.CurrentSession.Flush();

            }
        }

        public void Rollback()
        {
            nHibernateService.UnbindAndRollbackExistingSessionFromContext();
            nHibernateService.BindNewSessionToContext();
        }

        private IQuery InitializeQuery(string name, object parameters)
        {
            var query = nHibernateService.CurrentSession.GetNamedQuery(name);
            query.SetProperties(parameters);
            return query;
        }

        public TEntity FindOneByProc<TEntity>(string name, object parameters)
        {
            if (parameters != null)
            {
                var query = InitializeQuery(name, parameters);
                return query.UniqueResult<TEntity>();
            }

            return default(TEntity);
        }

        public void UpdateByNamedQuery(string name, object parameters)
        {
            var query = InitializeQuery(name, parameters);
            query.ExecuteUpdate();
        }

        public void UpdateByNamedQueryWithCustomTimeout(string name, object parameters, int timeout)
        {
            var query = InitializeQuery(name, parameters);
            if (timeout > 0)
                query.SetTimeout(timeout);
            query.ExecuteUpdate();
        }

        public IList<TEntity> FindByProc<TEntity>(string name, object parameters)
        {
            if (parameters != null)
            {
                var query = InitializeQuery(name, parameters);
                return query.List<TEntity>();
            }

            return new List<TEntity>();
        }

        public TEntity FindByID<TEntity>(Guid ID) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindByID(ID);
        }

        public TEntity FindOneWhere<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindOneWhere(expression);
        }

        public IList<TEntity> FindManyWhere<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindManyWhere(expression);
        }

        public IList<TEntity> FindManyWhere<TEntity>(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] sortCriteria) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindManyWhere(expression, sortCriteria);
        }

        public IList<TEntity> FindManyWhere<TEntity>(Expression<Func<TEntity, bool>> expression, SortOrder sortOrder, params Expression<Func<TEntity, object>>[] sortCriteria) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindManyWhere(expression, sortOrder, sortCriteria);
        }

        public IRepositoryQuery<TEntity> FindWhere<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindWhere(expression);
        }

        public IRepositoryRestriction<TEntity> FindWhereRestrictionOn<TEntity>(Expression<Func<TEntity, object>> expression)
            where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindWhereRestrictionOn(expression);
        }

        public IRepositoryQuery<TEntity> Find<TEntity>()
            where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.Find();
        }

        public IList<TEntity> FindAll<TEntity>() where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindAll();
        }

        public IList<TEntity> FindAllAndSortBy<TEntity>(params string[] sortProperties)
            where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.FindAllAndSortBy(sortProperties);
        }

        public Guid Save<TEntity>(TEntity entity) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.Save(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            repository.Delete(entity);
        }

        public void DeleteByID<TEntity>(Guid ID) where TEntity : Entity
        {
            TEntity entity = this.FindByID<TEntity>(ID);
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public bool Exists<TEntity>(Guid entityID) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.Exists(entityID);
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity
        {
            var count = Count(expression);
            return count > 0;
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity
        {
            IEntityRepository<TEntity> repository = new NHibernateEntityRepository<TEntity>();
            return repository.Count(expression);
        }

    }
}
