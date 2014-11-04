using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using SmartHome.Domain;
using SmartHome.Repository.Interface;

namespace SmartHome.Repository.Implementation
{
    public class RepositoryQuery<TEntity> : IRepositoryQuery<TEntity> where TEntity : Entity
    {
        internal IQueryOver<TEntity, TEntity> query;

        internal RepositoryQuery()
        {

        }

        public RepositoryQuery(IQueryOver<TEntity, TEntity> query)
        {
            this.query = query;
        }

        public virtual IRepositoryRestriction<TEntity> WhereRestriction(Expression<Func<TEntity, object>> expression)
        {
            return new RepositoryRestriction<TEntity>(expression, this);
        }

        public virtual IRepositoryQuery<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            query = query.Where(expression);
            return this;
        }

        public IRepositoryQuery<TEntity, U> JoinOn<U>(Expression<Func<TEntity, IEnumerable<U>>> joinExpression)
            where U : Entity
        {
            return new RepositoryQuery<TEntity, U>(query.JoinQueryOver(joinExpression));
        }

        public IRepositoryQuery<TEntity, U> JoinOnSingle<U>(Expression<Func<TEntity, U>> joinExpression) where U : Entity
        {
            return new RepositoryQuery<TEntity, U>(query.JoinQueryOver(joinExpression));
        }

        public IRepositoryQuery<TEntity> SortAscendingOn(params Expression<Func<TEntity, object>>[] sortCriteria)
        {

            foreach (var sortCriterion in sortCriteria)
            {
                query = query.OrderBy(sortCriterion).Asc;
            }
            return this;
        }

        public IRepositoryQuery<TEntity> SortDescendingOn(params Expression<Func<TEntity, object>>[] sortExpressions)
        {
            foreach (var sortCriterion in sortExpressions)
            {
                query = query.OrderBy(sortCriterion).Desc;
            }
            return this;
        }

        public virtual TEntity Unique()
        {
            return query.SingleOrDefault<TEntity>();
        }

        public virtual IList<TEntity> List()
        {
            return query.List<TEntity>();
        }
        public virtual IList<TEntity> List(int maxResults)
        {
            return query.Take(maxResults).List<TEntity>();
        }
    }

    public class RepositoryQuery<TEntity, U> : IRepositoryQuery<TEntity, U>
        where TEntity : Entity
        where U : Entity
    {
        private IQueryOver<TEntity, U> query;

        public RepositoryQuery(IQueryOver<TEntity, U> query)
        {
            this.query = query;
        }

        public IRepositoryQuery<TEntity, U> Where(Expression<Func<U, bool>> expression)
        {
            query = query.Where(expression);
            return this;
        }

        public IRepositoryQuery<TEntity, U> SortAscendingOn(params Expression<Func<U, object>>[] sortExpressions)
        {
            foreach (var sortCriterion in sortExpressions)
            {
                query = query.OrderBy(sortCriterion).Asc;
            }
            return this;
        }

        public IRepositoryQuery<TEntity, U> SortDescendingOn(params Expression<Func<U, object>>[] sortExpressions)
        {
            foreach (var sortCriterion in sortExpressions)
            {
                query = query.OrderBy(sortCriterion).Asc;
            }
            return this;
        }


        public TEntity Unique()
        {
            return query.SingleOrDefault<TEntity>();
        }

        public IList<TEntity> List()
        {
            return query.List<TEntity>();
        }
        public IList<TEntity> List(int maxResults)
        {
            return query.Take(maxResults).List<TEntity>();
        }

        public IRepositoryQuery<TEntity, TAnotherEntity> JoinOn<TAnotherEntity>(
            Expression<Func<U, IEnumerable<TAnotherEntity>>> joinExpression) where TAnotherEntity : Entity
        {
            return new RepositoryQuery<TEntity, TAnotherEntity>(query.JoinQueryOver(joinExpression));
        }

        public IRepositoryQuery<TEntity, TAnotherEntity> JoinOnSingle<TAnotherEntity>(
            Expression<Func<U, TAnotherEntity>> joinExpression) where TAnotherEntity : Entity
        {
            return new RepositoryQuery<TEntity, TAnotherEntity>(query.JoinQueryOver(joinExpression));
        }
    }
}
