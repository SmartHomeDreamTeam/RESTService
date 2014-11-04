using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;
using SmartHome.Repository.Implementation;

namespace SmartHome.Repository.UnitTest.Mappings
{
    public abstract class nHibernateMappingTestBase
    {
        private ITransaction tx;
        private ISession session;
        private NHibernateHelper nHibernateHelper = new NHibernateHelper(new[] { "SmartHome.Repository" });

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            session =  nHibernateHelper.GetCurrentSession();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            //nHibernateHelper.CloseSession();
        }

        [SetUp]
        public void SetUp()
        {
            tx = session.BeginTransaction();

        }

        [TearDown]
        public void TearDown()
        {
            tx.Rollback();
        }

    }
}
