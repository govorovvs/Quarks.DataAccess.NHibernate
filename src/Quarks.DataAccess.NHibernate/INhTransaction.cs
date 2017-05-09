using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
    public interface INhTransaction
    {
        ISession Session { get; }
    }
}