using System;
using NHibernate;
using SmartHome.Domain;
using SmartHome.Repository.Interface;

namespace SmartHome.Repository.Implementation
{
    public class UserInforRepository : IUserInforRepository
    {
        public void Insert(UserInfor userInfor)
        {
            var session = NHibernateHelper.GetCurrentSession();
            session.Save(userInfor);
        }

        public void Update(UserInfor userInfor)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserInfor userInfor)
        {
            throw new NotImplementedException();
        }
    }
}
