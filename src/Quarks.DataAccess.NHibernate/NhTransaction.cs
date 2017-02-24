using Quarks.Transactions;

namespace Quarks.DataAccess.NHibernate
{
    public static class NhTransaction
    {
        public static INhTransaction GetCurrent(INhSessionFactory sessionFactory)
        {
            string key = sessionFactory.GetHashCode().ToString();
            Transaction transaction = Transaction.Current;

            IDependentTransaction current =
                transaction == null
                    ? new NhTransactionImpl(sessionFactory)
                    : transaction.GetOrEnlist(key, () => new NhTransactionImpl(sessionFactory));
            return (INhTransaction)current;
        }
    }
}