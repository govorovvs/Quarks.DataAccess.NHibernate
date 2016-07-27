using NHibernate;

namespace Quarks.DataAccess.NHibernate.SessionManagement
{
	public interface INhSessionManager
	{
		ISession CreateSession();

		int GetHashCode();
	}
}