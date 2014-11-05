using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;


namespace SmartHome.Repository.Implementation
{
    public sealed class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        public static void Initial(IEnumerable<string> assemblynames)
        {
             var configuration = new Configuration().Configure();
            foreach (var assemblyName in assemblynames)
            {
                configuration.AddAssembly(assemblyName);
            }
            sessionFactory = configuration.BuildSessionFactory();
        }

        public static ISession GetCurrentSession()
        {
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.OpenSession();
                session.FlushMode = FlushMode.Commit;
                CurrentSessionContext.Bind(session);
            }

            return sessionFactory.GetCurrentSession();
        }

        public static void CloseSession()
        {
            var session = CurrentSessionContext.Unbind(sessionFactory);
            if (session != null && session.Transaction.IsActive)
            {
                session.Close();
            }
        }

        public static void BeginTransaction()
        {
            var session = GetCurrentSession();
            session.BeginTransaction();
        }

        public static void Commit()
        {
            var session = CurrentSessionContext.Unbind(sessionFactory);
            if (session != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
        }

        public static void RollBack()
        {
            var session = CurrentSessionContext.Unbind(sessionFactory);
            if (session != null && session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }
        }

        public static void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }

    }
}
