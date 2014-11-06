using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain
{
    public class UserInfor : IEntity
    {
        public UserInfor()
        {

        }

        public virtual Guid ID { get; set; }

        public virtual string UserID { get; set; }

        public virtual string Pin { get; set; }

        public virtual IList<Session> Sessions { get; protected set; }

        public virtual void Add(Session session)
        {
            session.UserInfor = this;
            if (Sessions == null)
            {
                Sessions = new List<Session>();
            }
            this.Sessions.Add(session);
        }

    }
}
