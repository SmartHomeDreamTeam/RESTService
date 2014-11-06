using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.Repository.Interface
{
    public interface IRepository<T> : IQueryable<T>
    {
        void Add(T entity);
        T Get(Guid id);
        void Remove(T entity);
    }
}
