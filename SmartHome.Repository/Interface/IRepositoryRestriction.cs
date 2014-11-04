using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.Repository.Interface
{
    public interface IRepositoryRestriction<TEntity>
           where TEntity : Entity
    {
        IRepositoryQuery<TEntity> IsLike(string pattern);
        IRepositoryQuery<TEntity> IsNull();
        IRepositoryQuery<TEntity> IsIn(ICollection values);
    }
}
