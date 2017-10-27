using System;
using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
    public class NhSessionProvider : INhSessionProvider
    {
        private readonly INhSessionFactory _sessionFactory;

        public NhSessionProvider(INhSessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
        }

        public virtual ISession Session => Transaction.Session;

        private INhTransaction Transaction => NhTransaction.GetCurrent(_sessionFactory);
    }
}