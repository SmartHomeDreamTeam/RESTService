using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.Repository.Interface
{
    public interface IUserInforRepository
    {
        void Insert(UserInfor userInfor);
        void Update(UserInfor userInfor);
        void Delete(UserInfor userInfor);
    }
}
