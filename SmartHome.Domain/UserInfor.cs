using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain
{
    public class UserInfor : Entity
    {
        public virtual Guid ID { get; set; }

        public virtual string Pin { get; set; }
    }
}
