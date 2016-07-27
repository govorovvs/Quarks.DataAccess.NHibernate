using System;
using NHibernate;
using Quarks.DataAccess.NHibernate.SessionManagement;

namespace Quarks.DataAccess.NHibernate
{
	public abstract class NhRepository
	{
		private readonly INhSessionManager _sessionManager;

		protected NhRepository(INhSessionManager sessionManager)
		{
			if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));

			_sessionManager = sessionManager;
		}

		protected ISession Session
		{
			get { return UnitOfWork.Session; }
		}

		private NhUnitOfWork UnitOfWork
		{
			get { return NhUnitOfWork.GetCurrent(_sessionManager); }
		}
	}
}