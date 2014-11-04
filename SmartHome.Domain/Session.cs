using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain
{
    public class Session : Domain
    {
        public virtual Guid ID { get; set; }

        public virtual Guid UserInforID { get; set; }

        public virtual string Secretkey { get; set; }

        public virtual DateTime CreateDateTime { get; set; }

        public virtual string CreatedBy { get; set; }
    }
}
