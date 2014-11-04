using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.Repository.Interface
{
    public interface IRepositoryQuery<TEntity> where TEntity : Entity
    {
        IRepositoryQuery<TEntity> SortAscendingOn(params Expression<Func<TEntity, object>>[] sortExpressions);
        IRepositoryQuery<TEntity> SortDescendingOn(params Expression<Func<TEntity, object>>[] sortExpressions);
        IRepositoryQuery<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        IRepositoryRestriction<TEntity> WhereRestriction(Expression<Func<TEntity, object>> expression);
        IRepositoryQuery<TEntity, TSecondEntity> JoinOn<TSecondEntity>(
            Expression<Func<TEntity, IEnumerable<TSecondEntity>>> joinExpression)
            where TSecondEntity : Entity;

        IRepositoryQuery<TEntity, TSecondEntity> JoinOnSingle<TSecondEntity>(
            Expression<Func<TEntity, TSecondEntity>> joinExpression)
            where TSecondEntity : Entity;

        TEntity Unique();
        IList<TEntity> List();
        IList<TEntity> List(int maxResults);
    }

    public interface IRepositoryQuery<TEntity, TSecondEntity>
        where TEntity : Entity
        where TSecondEntity : Entity
    {
        IRepositoryQuery<TEntity, TSecondEntity> Where(Expression<Func<TSecondEntity, bool>> expression);

        IRepositoryQuery<TEntity, TSecondEntity> SortAscendingOn(params Expression<Func<TSecondEntity, object>>[] sortExpressions);
        IRepositoryQuery<TEntity, TSecondEntity> SortDescendingOn(params Expression<Func<TSecondEntity, object>>[] sortExpressions);
        TEntity Unique();
        IList<TEntity> List();
        IList<TEntity> List(int maxResults);
        IRepositoryQuery<TEntity, TAnotherEntity> JoinOn<TAnotherEntity>(
           Expression<Func<TSecondEntity, IEnumerable<TAnotherEntity>>> joinExpression)
           where TAnotherEntity : Entity;
        IRepositoryQuery<TEntity, TAnotherEntity> JoinOnSingle<TAnotherEntity>(
              Expression<Func<TSecondEntity, TAnotherEntity>> joinExpression)
              where TAnotherEntity : Entity;
    }
}
