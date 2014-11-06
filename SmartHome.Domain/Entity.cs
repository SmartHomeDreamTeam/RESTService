using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain
{
    public interface IEntity
    {
        Guid ID { get; set; }
    }
}
