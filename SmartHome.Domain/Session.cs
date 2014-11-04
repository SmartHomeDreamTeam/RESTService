using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain
{
    public class Session : Entity
    {
        public virtual Guid ID { get; set; }

        public virtual Guid UserInforID { get; set; }

        public virtual string SecretKey { get; set; }

        public virtual DateTime CreatedDateTime { get; set; }

        public virtual string CreatedBy { get; set; }
    }
}
