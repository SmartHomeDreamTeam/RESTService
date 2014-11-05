using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace SmartHome.Service.Interface.NHibernateService
{
    public interface INHibernateService : IDisposable
    {
        ISession CurrentSession { get; }
        void BindNewSessionToContext();
        void UnbindAndCommitExistingSessionFromContext();
        void RunSessionInSimulationMode();
        void RunSessionInNormalMode();
        bool SessionIsInSimulationMode(ISession session);
        void ForceEagerLoadOfObject(object obj);

        /// <summary>
        /// Ends the current session by unbinding it from the current context and rollback the transaction.
        /// Only intended for usage in an HttpModule or test harness.
        /// </summary>
        void UnbindAndRollbackExistingSessionFromContext();
    }
}
