using NHibernate;

namespace Elf.Persistence.Configuration {
    /// <summary>
    /// Provides an NHibernate Session to the injection container.
    /// </summary>
    public class SessionProvider : Ninject.Activation.Provider<ISession> {
        readonly ISessionFactory sessionFactory;

        /// <summary>
        /// Create the session provider from an NHibernate SessionFactory
        /// </summary>
        /// <param name="sessionFactory">The factory that will be used to create the Session</param>
        public SessionProvider(ISessionFactory sessionFactory) {
            this.sessionFactory = sessionFactory;
        }
        /// <summary>
        /// Create the Session
        /// </summary>
        protected override ISession CreateInstance(Ninject.Activation.IContext context) {
            ISession session = this.sessionFactory.OpenSession();
            return session;
        }
    }
}
