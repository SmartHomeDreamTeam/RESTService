using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Service.Interface.NHibernateService;

namespace SmartHome.Service.Interface.RepositoryService
{
    public abstract class NHibernateRepositoryBase
    {
        private INHibernateService service;
        internal INHibernateService NHibernateService
        {
            get
            {
                if (service == null)
                {
//                    service = Platform.Get<INHibernateService>();
//                    if (service == null) // (even after the platform instantiation)
//                    {
//                        throw new NoNHibernateServiceAvailableException();
//                    }
                }
                return service;
            }
        }
    }
}
