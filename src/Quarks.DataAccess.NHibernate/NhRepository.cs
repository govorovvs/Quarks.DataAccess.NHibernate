using System;
using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
	public abstract class NhRepository
	{
		private readonly INhSessionProvider _sessionProvider;

		protected NhRepository(INhSessionProvider sessionProvider)
		{
			if (sessionProvider == null) throw new ArgumentNullException(nameof(sessionProvider));

            _sessionProvider = sessionProvider;
		}

		protected ISession Session
		{
			get { return _sessionProvider.Session; }
		}
	}
}