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
        protected NHibernateHelper nHibernateHelper = new NHibernateHelper(new[] { "SmartHome.Repository" });

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            nHibernateHelper.CloseSession();
        }

        [SetUp]
        public void SetUp()
        {
            nHibernateHelper.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            nHibernateHelper.RollBack();
        }

    }
}
