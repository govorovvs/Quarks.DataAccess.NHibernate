using Quarks.Transactions;

namespace Quarks.DataAccess.NHibernate
{
    public static class NhTransaction
    {
        public static INhTransaction GetCurrent(INhSessionFactory sessionFactory)
        {
            Transaction transaction = Transaction.Current;
            if (transaction == null)
                return new NhTransactionImpl(sessionFactory);

            string key = sessionFactory.GetHashCode().ToString();
            return (INhTransaction)transaction.GetOrEnlist(key, () => new NhTransactionImpl(sessionFactory));
        }
    }
}