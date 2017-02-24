using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
    public interface INhSessionProvider
    {
        ISession Session { get; }
    }
}