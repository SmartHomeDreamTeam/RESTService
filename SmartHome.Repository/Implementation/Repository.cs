using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using SmartHome.Repository.Interface;


// Todo: how to support var orders = customer.Orders.Where(order => order.Price > 10.0);
namespace SmartHome.Repository.Implementation
{
    public class Repository<T>: IRepository<T>
    {
        private readonly ISession session;

        public Repository(ISession session)
        {
            this.session = session;
        }

        public Type ElementType
        {
            get { return session.Query<T>().ElementType; }
        }

        public Expression Expression
        {
            get { return session.Query<T>().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return session.Query<T>().Provider; }
        }

        public void Add(T entity)
        {
            session.Save(entity);
        }

        public T Get(Guid id)
        {
            return session.Get<T>(id);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return session.Query<T>().GetEnumerator();
        }

        public void Remove(T entity)
        {
            session.Delete(entity);
        }

    }
}
