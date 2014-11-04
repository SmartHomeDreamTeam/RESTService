using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using SmartHome.Domain;
using SmartHome.Service.Interface.NHibernateService;

namespace SmartHome.Repository.NHibernateService
{
    public class NHibernateService : INHibernateService
    {
        private readonly ISessionFactory factory;
        private HashSet<Type> ignoreSet;
        public NHibernateService(IEnumerable<string> assemblyNames)
        {
            var configuration = new Configuration();

            foreach (var assemblyName in assemblyNames)
            {
                configuration.AddAssembly(assemblyName);
            }

//            configuration.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new LastModifiedByEventListener() };
//            configuration.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new LastModifiedByEventListener(), new PreInsertDisbursementEventListener() };
//
//            configuration.EventListeners.DeleteEventListeners = new IDeleteEventListener[] { new DeleteEventListener() };
            factory = configuration.BuildSessionFactory();
        }

        /// <summary>
        /// Required for any repository class.
        /// </summary>
        public ISession CurrentSession
        {
            get
            {
                if (!CurrentSessionContext.HasBind(factory))
                {
                    BindNewSessionToContext();
                }

                return factory.GetCurrentSession();
            }
        }

        /// <summary>
        /// Starts an NHibernate session and transaction, then binds it to the current context. 
        /// Only intended for usage in an HttpModule or test harness.
        /// </summary>
        public void BindNewSessionToContext()
        {
            var session = factory.OpenSession();
            session.FlushMode = FlushMode.Commit;
            session.BeginTransaction();
            CurrentSessionContext.Bind(session);
        }


        /// <summary>
        /// Ends the current session by unbinding it from the current context and committing the transaction.
        /// Only intended for usage in an HttpModule or test harness.
        /// </summary>
        public void UnbindAndCommitExistingSessionFromContext()
        {
            var session = CurrentSessionContext.Unbind(factory);

            if (session != null && session.Transaction.IsActive)
            {
                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception e)
                {
                    if (session.Transaction != null)
                        session.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    session.Close();
                }
            }
        }


        public void InitializeFields(object obj, HashSet<Guid> hs)
        {
            if (obj == null)
                return;

            var entity = obj as Entity;
            if (entity != null && hs.Contains(entity.ID))
                return;
            if (entity != null)
            {
                NHibernateUtil.Initialize(obj);
                hs.Add(entity.ID);
            }
            foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.FieldType.IsByRef))
            {
                if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string)) continue;
                var objVale = fieldInfo.GetValue(obj);
                if (objVale is IEnumerable)
                {
                    foreach (object newobj in (IEnumerable)objVale)
                    {
                        InitializeFields(newobj, hs);
                    }
                }
                else if (objVale != null)
                    InitializeFields(fieldInfo.GetValue(objVale) as Entity, hs);
            }
        }
        public static T DeepCopy<T>(T obj)
        {
            object result = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                result = (T)formatter.Deserialize(ms);
                ms.Close();
            }
            return (T)result;
        }
        public void InitializeFieldsWithDepth(object obj, HashSet<Guid> hs, IList<string> list)
        {
            if (obj == null)
                return;
            var entity = obj as Entity;
            if (entity != null && hs.Contains(entity.ID))
                return;

            if (entity != null)
            {
                list.Add(entity.GetType().Name);
                NHibernateUtil.Initialize(obj);
                hs.Add(entity.ID);
            }
            foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.FieldType.IsByRef))
            {
                if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string)) continue;
                var objVale = fieldInfo.GetValue(obj);
                if (objVale is IEnumerable)
                {
                    foreach (object newobj in (IEnumerable)objVale)
                    {
                        var newlist = DeepCopy<IList<string>>(list);
                        InitializeFieldsWithDepth(newobj, hs, newlist);
                    }
                }
                else if (objVale != null)
                {
                    var newlist = DeepCopy<IList<string>>(list);
                    InitializeFieldsWithDepth(fieldInfo.GetValue(objVale) as Entity, hs, newlist);
                }
            }
            var buffer = new StringBuilder();
            foreach (var item in list)
            {
                buffer.Append(item + ".");
            }
            Console.WriteLine(buffer.ToString());
        }
        // output the dept and object calls to file -
        public void InitializePropertiesWithDepth(object obj, HashSet<Guid> hs, IList<string> list)
        {
            if (obj == null)
                return;
            var entity = obj as Entity;
            if (entity != null && hs.Contains(entity.ID))
                return;
            if (entity != null)
            {
                list.Add(entity.GetType().Name);
                NHibernateUtil.Initialize(obj);
                hs.Add(entity.ID);
            }

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.CanWrite))
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)) continue;
                var objVale = propertyInfo.GetValue(obj, null);
                if (objVale is IEnumerable)
                {
                    foreach (object newob in (IEnumerable)objVale)
                    {
                        var newlist = DeepCopy<IList<string>>(list);
                        InitializePropertiesWithDepth(newob, hs, newlist);
                    }
                }
                else if (objVale != null)
                {
                    var newlist = DeepCopy<IList<string>>(list);
                    InitializePropertiesWithDepth(objVale, hs, newlist);
                }
            }
            // Output buffer to flat file for closer inspection here
            //var buffer = new StringBuilder();
            //foreach (var item in list)
            //{
            //    buffer.Append(item + ".");
            //}
            //StreamWriter streamWriter = new StreamWriter(new FileStream(@"c:/object-calls.txt",FileMode.Append));
            //streamWriter.WriteLine(buffer);
            //streamWriter.Close();
        }
        public void InitializeProperties(object obj, HashSet<Guid> hs)
        {
            if (obj == null)
                return;

            var entity = obj as Entity;
            if (entity != null && hs.Contains(entity.ID))
                return;
            if (entity != null)
            {
                NHibernateUtil.Initialize(obj);
                hs.Add(entity.ID);
            }

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.CanWrite))
            {

                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)) continue;
                var objVale = propertyInfo.GetValue(obj, null);
                if (objVale is IEnumerable)
                {
                    foreach (object newob in (IEnumerable)objVale)
                    {
                        InitializeProperties(newob, hs);
                    }
                }
                else if (objVale != null)
                    InitializeProperties(objVale, hs);
            }
        }
        public void ForceEagerLoadOfObject(object obj)
        {
            InitializeProperties(obj as Entity, new HashSet<Guid>());
            InitializeFields(obj as Entity, new HashSet<Guid>());
        }

        /// <summary>
        /// Ends the current session by unbinding it from the current context and rollback the transaction.
        /// Only intended for usage in an HttpModule or test harness.
        /// </summary>
        public void UnbindAndRollbackExistingSessionFromContext()
        {
            var session = CurrentSessionContext.Unbind(factory);

            if (session != null)
            {
                try
                {
                    if (session.Transaction.IsActive)
                        session.Transaction.Rollback();
                }
                finally
                {
                    session.Close();
                }
            }
        }

        public void RunSessionInSimulationMode()
        {
            CurrentSession.FlushMode = FlushMode.Never;
        }

        public void RunSessionInNormalMode()
        {
            CurrentSession.FlushMode = FlushMode.Commit;
        }

        public bool SessionIsInSimulationMode(ISession session)
        {
            return session.FlushMode == FlushMode.Never;
        }

        /// <summary>
        /// Don't call the method directly in any tests
        /// </summary>
        public void Dispose()
        {
            //factory.GetCurrentSession().Dispose();
            factory.Dispose();
        }
    }
}
