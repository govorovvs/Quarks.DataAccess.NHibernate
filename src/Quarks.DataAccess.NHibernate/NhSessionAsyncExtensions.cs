using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
    public static class NhSessionAsyncExtensions
    {
        public static Task<TEntity> GetAsync<TEntity>(this ISession session, object id, CancellationToken cancellationToken)
        {
            return Task.FromResult(session.Get<TEntity>(id));
        }

        public static Task SaveOrUpdateAsync(this ISession session, object obj, CancellationToken cancellationToken)
        {
            session.SaveOrUpdate(obj);
            return Task.FromResult(0);
        }

        public static Task DeleteAsync(this ISession session, object obj, CancellationToken cancellationToken)
        {
            session.Delete(obj);
            return Task.FromResult(0);
        }
    }
}