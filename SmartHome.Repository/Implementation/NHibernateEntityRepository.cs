using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using SmartHome.Domain;
using SmartHome.Repository.Interface;

namespace SmartHome.Repository.Implementation
{
    public class NHibernateEntityRepository<TImplementation> : NHibernateRepositoryBase, IEntityRepository<TImplementation>
         where TImplementation : Entity
    {
        public ISession Session { get; set; }

        public virtual Guid Save(TImplementation type)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            if (NHibernateService.SessionIsInSimulationMode(session))
            {
                throw new Exception(
                    "There was an attempt to save data explicitly while the application was running in simulation mode.");
            }
            session.SaveOrUpdate(type);
            return type.ID;
        }

        public void Delete(TImplementation type)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            if (NHibernateService.SessionIsInSimulationMode(session))
            {
                throw new Exception(
                    "There was an attempt to delete data explicitly while the application was running in simulation mode.");
            }
            session.Delete(type);
        }

        public TImplementation FindByID(Guid id)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            return session.Get<TImplementation>(id);
        }

        public virtual IList<TImplementation> FindAll()
        {
            var session = Session ?? NHibernateService.CurrentSession;
            return session.CreateCriteria(typeof(TImplementation)).SetCacheable(true).List<TImplementation>();
        }

        public virtual IList<TImplementation> FindAllAndSortBy(params string[] sortCriteria)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            var criteria = session.CreateCriteria(typeof(TImplementation)).SetCacheable(true);
            foreach (string sortParameter in sortCriteria)
            {
                criteria = criteria.AddOrder(Order.Asc(sortParameter));
            }
            return criteria.List<TImplementation>();
        }

        public TImplementation FindOneWhere(Expression<Func<TImplementation, bool>> expression)
        {

            var session = Session ?? NHibernateService.CurrentSession;
            return session.QueryOver<TImplementation>()
                .Where(expression)
                .SingleOrDefault<TImplementation>();
        }


        public IList<TImplementation> FindManyWhere(Expression<Func<TImplementation, bool>> expression)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            return session.QueryOver<TImplementation>()
                .Where(expression)
                .List<TImplementation>();
        }

        public IList<TImplementation> FindManyWhere(Expression<Func<TImplementation, bool>> expression, params Expression<Func<TImplementation, object>>[] sortCriteria)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            var criteria = session.QueryOver<TImplementation>().Where(expression);
            foreach (var sortCriterion in sortCriteria)
            {
                criteria.OrderBy(sortCriterion).Asc();
            }
            return criteria.List<TImplementation>();
        }

        public IList<TImplementation> FindManyWhere(Expression<Func<TImplementation, bool>> expression, SortOrder sortOrder, params Expression<Func<TImplementation, object>>[] sortCriteria)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            var criteria = session.QueryOver<TImplementation>().Where(expression);
            if (sortOrder == SortOrder.Unspecified)
                return criteria.List<TImplementation>();

            foreach (var sortCriterion in sortCriteria)
            {
                if (sortOrder == SortOrder.Ascending)
                    criteria.OrderBy(sortCriterion).Asc();
                else
                {
                    criteria.OrderBy(sortCriterion).Desc();
                }
            }

            return criteria.List<TImplementation>();
        }

        public IRepositoryQuery<TImplementation> FindWhere(Expression<Func<TImplementation, bool>> expression)
        {
            var session = Session ?? NHibernateService.CurrentSession;

            return new RepositoryQuery<TImplementation>(session.QueryOver<TImplementation>().Where(expression));
        }

        public IRepositoryQuery<TImplementation> Find()
        {
            var session = Session ?? NHibernateService.CurrentSession;
            return new RepositoryQuery<TImplementation>(session.QueryOver<TImplementation>());
        }

        public IRepositoryRestriction<TImplementation> FindWhereRestrictionOn(Expression<Func<TImplementation, object>> expression)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            var repoQuery = new RepositoryQuery<TImplementation>(session.QueryOver<TImplementation>());
            return repoQuery.WhereRestriction(expression);
        }

        public bool Exists(Guid entityID)
        {
            var session = Session ?? NHibernateService.CurrentSession;
            var count = session.QueryOver<TImplementation>().Where(x => x.ID == entityID).RowCount();
            return count > 0;
        }

        public int Count(Expression<Func<TImplementation, bool>> expression)
        {
            var session = Session ?? NHibernateService.CurrentSession;

            return session.QueryOver<TImplementation>()
                .Where(expression)
                .RowCount();
        }
    }
}
