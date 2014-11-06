using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SmartHome.Domain;
using SmartHome.Repository.Implementation;
using SmartHome.Repository.Interface;

namespace SmartHome.Repository.UnitTest.Mappings
{
    [TestFixture]
    public class UserInforTests : nHibernateMappingTestBase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.TestFixtureSetUp();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            base.TestFixtureTearDown();
        }

        [SetUp]
        public void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void InsertTest()
        {
            var userInfor = new UserInfor() {ID = new Guid(), UserID ="TestUserID", Pin = "pin"};
            userInfor.Add(new Session(){ CreatedBy = "TestUser", SecretKey = "secretkey", CreatedDateTime = DateTime.Now });
            IUserInforRepository repository = new UserInforRepository();
            repository.Insert(userInfor);
        }
    }
}
