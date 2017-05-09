using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
    public interface INhSessionFactory
    {
        ISession CreateSession();

        string GetKey();
    }
}