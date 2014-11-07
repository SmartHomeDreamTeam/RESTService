using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using NHibernate.Type;
using NUnit.Framework;
using SmartHome.Domain;
using SmartHome.Repository.Implementation;
using SmartHome.Repository.UnitTest.Mappings;
using System.Linq;


namespace SmartHome.Repository.UnitTest
{
    /// <summary>
    /// The test result is: using nHibernate Linq is simple way, the Repository provides the way to Insert, Update, Delete
    /// when query for a complicated critial, use nHibernate Linq
    /// </summary>

    [TestFixture]
    public class RepositoryTests : nHibernateMappingTestBase
    {
        [Test]
        public void Linq_First_Test()
        {
            var repository = new Repository<UserInfor>(NHibernateHelper.GetCurrentSession());

            var result = repository.First(x => x.UserID == "userid");
            Assert.That(result.UserID, Is.EqualTo("userid"));
            Assert.That(result.Sessions[0], Is.Not.Null);

        }

        [Test]
        public void Linq_Alias_Test()
        {
            IList<Session> sessionsAlias = null;

            var result =
                NHibernateHelper.GetCurrentSession()
                    .QueryOver<UserInfor>()
                    .JoinAlias(u => u.Sessions, () => sessionsAlias)
                    .Where(x => x.Pin == "1234");

            Assert.That(result.RowCount(), Is.EqualTo(2));

        }


        [Test]
        public void Linq_Join_Test()
        {
            var result = new Repository<UserInfor>(NHibernateHelper.GetCurrentSession())
                .First(x => x.Sessions.Any(y => y.CreatedBy == "CreatedBy1"));
            Assert.That(result.Sessions[0].CreatedBy, Is.EqualTo("CreatedBy1"));
        }

        [Test]
        public void Linq_MultipleJoin1_Test()
        {
            var eventLogs = new Repository<EventLog>(NHibernateHelper.GetCurrentSession());

            var result = new Repository<UserInfor>(NHibernateHelper.GetCurrentSession()).Join(eventLogs, e => e.ID,
                u => u.UserInforID, (e, u) => new MultipleTableQueryResult()
                {
                    UserInforID = u.ID,
                    LogID = u.LogID,
                    Description = u.Description,
                    Pin = e.Pin
                })
                .Where(x => x.Pin == "1234");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }


        /// <summary>
        /// Test DefaultIfEmpty(), it still doesn't work on Left Join
        /// </summary>
        [Test]
        public void Linq_MultipleJoin2_Test()
        {
            var session = NHibernateHelper.GetCurrentSession();

            var result = from u in session.Query<UserInfor>()
                         from s in u.Sessions.DefaultIfEmpty()
                         select u;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToArray()[0].UserID, Is.Not.Null);
        }

        [Test]
        public void Linq_MultipleJoin3_Test()
        {
            var userInfors = new Repository<UserInfor>(NHibernateHelper.GetCurrentSession());
            var eventLogs = new Repository<EventLog>(NHibernateHelper.GetCurrentSession());
            var sessions = new Repository<Session>(NHibernateHelper.GetCurrentSession());

            var session = NHibernateHelper.GetCurrentSession();

            var result = from u in session.Query<UserInfor>()
                         join e in session.Query<EventLog>() on u.ID equals e.UserInforID
                         join s in session.Query<Session>() on u.ID equals s.UserInfor.ID
                         select new MultipleTableQueryResult
                         {
                             UserInforID = u.ID,
                             LogID = e.LogID,
                             Description = e.Description,
                             Pin = u.Pin,
                             SessionCreatedDateTime = s.CreatedDateTime
                         };

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToArray()[0].Pin, Is.Not.Null);
        }



        /// <summary>
        /// Test Left Join
        /// </summary>
        [Test, ExpectedException(typeof(NotImplementedException))]
        public void Linq_MultipleJoin4_Test()
        {
            var session = NHibernateHelper.GetCurrentSession();

            var result = from u in session.Query<UserInfor>()
                         join e in session.Query<EventLog>() on u.ID equals e.UserInforID into tempResult1
                         from t1 in tempResult1.DefaultIfEmpty()
                         join s in session.Query<Session>() on t1.ID equals s.UserInfor.ID into tempResult2
                         from t2 in tempResult2.DefaultIfEmpty()
                         select new MultipleTableQueryResult
                         {
                             UserInforID = t2.ID,
                             LogID = t1.LogID,
                             Description = t1.Description,
                             Pin = u.Pin,
                             SessionCreatedDateTime = t2.CreatedDateTime
                         };

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToArray()[0].Pin, Is.Not.Null);
        }


        
        [Test]
        public void SaveTest()
        {
            var userInfor = new UserInfor() { ID = new Guid(), UserID = "TestUserID", Pin = "pin" };
            userInfor.Add(new Session() { CreatedBy = "TestUser", SecretKey = "secretkey", CreatedDateTime = DateTime.Now });

            var session = NHibernateHelper.GetCurrentSession();
            session.Save(userInfor);
        }

        [Test]
        public void DeleteTest()
        {
            var userInfor = CreateData();
            var session = NHibernateHelper.GetCurrentSession();
            var id = session.Save(userInfor);
            var newUserInfor = session.Get<UserInfor>(id);
            session.Delete(newUserInfor);
        }


        [Test]
        public void UpdateTest()
        {
            var userInfor = CreateData();
            var session = NHibernateHelper.GetCurrentSession();
            var id = session.Save(userInfor);
            var newUserInfor = session.Get<UserInfor>(id);
            newUserInfor.Pin = "8888";
            session.Save(newUserInfor);
            userInfor = session.Get<UserInfor>(id);

            Assert.That(userInfor.Pin, Is.EqualTo("8888") );

        }

        private UserInfor CreateData()
        {
            var userInfor = new UserInfor() { ID = new Guid(), UserID = "TestUserID", Pin = "pin" };
            userInfor.Add(new Session() { CreatedBy = "TestUser", SecretKey = "secretkey", CreatedDateTime = DateTime.Now });
            return userInfor;
        }

    }

    internal class MultipleTableQueryResult
    {
        public Guid UserInforID { get; set; }
        public Guid? LogID { get; set; }
        public string Description { get; set; }
        public string Pin { get; set; }
        public DateTime SessionCreatedDateTime { get; set; }
    }
}
