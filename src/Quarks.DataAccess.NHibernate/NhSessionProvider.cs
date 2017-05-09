using System;
using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
    public class NhSessionProvider : INhSessionProvider
    {
        private readonly INhSessionFactory _sessionFactory;

        public NhSessionProvider(INhSessionFactory sessionFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException(nameof(sessionFactory));

            _sessionFactory = sessionFactory;
        }

        public virtual ISession Session
        {
            get { return Transaction.Session; }
        }

        private INhTransaction Transaction
        {
            get { return NhTransaction.GetCurrent(_sessionFactory); }
        }
    }
}