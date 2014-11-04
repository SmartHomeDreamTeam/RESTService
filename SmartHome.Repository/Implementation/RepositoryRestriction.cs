using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using SmartHome.Domain;
using SmartHome.Repository.Interface;

namespace SmartHome.Repository.Implementation
{
    public class RepositoryRestriction<TEntity> : IRepositoryRestriction<TEntity>
           where TEntity : Entity
    {
        private RepositoryQuery<TEntity> rootQuery;
        private Expression<Func<TEntity, object>> expression;

        public RepositoryRestriction(Expression<Func<TEntity, object>> expression, RepositoryQuery<TEntity> rootQuery)
        {
            this.expression = expression;
            this.rootQuery = rootQuery;
        }

        public IRepositoryQuery<TEntity> IsLike(string pattern)
        {
            this.rootQuery.query = this.rootQuery.query.WhereRestrictionOn(expression).IsLike(pattern, MatchMode.Anywhere);
            return rootQuery;
        }

        public IRepositoryQuery<TEntity> IsNull()
        {
            this.rootQuery.query = this.rootQuery.query.WhereRestrictionOn(expression).IsNull();
            return this.rootQuery;
        }

        public IRepositoryQuery<TEntity> IsIn(ICollection values)
        {
            this.rootQuery.query = this.rootQuery.query.WhereRestrictionOn(expression).IsIn(values);
            return this.rootQuery;
        }
    }
}
