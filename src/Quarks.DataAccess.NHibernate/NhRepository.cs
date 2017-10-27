using System;
using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
	public abstract class NhRepository
	{
		private readonly INhSessionProvider _sessionProvider;

		protected NhRepository(INhSessionProvider sessionProvider)
		{
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
		}

		protected ISession Session => _sessionProvider.Session;
	}
}