using System;
using System.Threading;
using System.Threading.Tasks;
using Quarks.DataAccess.NHibernate.SessionManagement;
using Quarks.DomainModel.Impl;
using NHibernate;

namespace Quarks.DataAccess.NHibernate
{
	internal class NhUnitOfWork : IDependentUnitOfWork
	{
		private static readonly object StaticLock = new object();
		private readonly object _lock = new object();
		private bool _disposed;
		private volatile ISession _session;
		private ITransaction _transaction;

		internal NhUnitOfWork(INhSessionManager sessionManager)
		{
			if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));

			SessionManager = sessionManager;
		}

		public INhSessionManager SessionManager { get; }

		public ISession Session => GetOrCreateSession();

		public void Dispose()
		{
			ThrowIfDisposed();

			if (_session != null)
			{
				_transaction.Dispose();
				_session.Dispose();
			}

			_disposed = true;
		}

		public Task CommitAsync(CancellationToken cancellationToken)
		{
			ThrowIfDisposed();

			if (_session != null)
			{
				_transaction.Commit();
				_session.Flush();
			}

			return Task.CompletedTask;
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		private ISession GetOrCreateSession()
		{
			if (_session == null)
			{
				lock (_lock)
				{
					if (_session == null)
					{
						_session = SessionManager.CreateSession();
						_transaction = _session.BeginTransaction();
					}
				}
			}

			return _session;
		}

		public static NhUnitOfWork GetCurrent(INhSessionManager sessionManager)
		{
			if (UnitOfWork.Current == null)
			{
				return new NhUnitOfWork(sessionManager);
			}

			string key = sessionManager.GetHashCode().ToString();

			IDependentUnitOfWork current;
			if (!UnitOfWork.Current.DependentUnitOfWorks.TryGetValue(key, out current))
			{
				lock (StaticLock)
				{
					if (!UnitOfWork.Current.DependentUnitOfWorks.TryGetValue(key, out current))
					{
						current = new NhUnitOfWork(sessionManager);
						UnitOfWork.Current.Enlist(key, current);
					}
				}
			}

			return (NhUnitOfWork)current;
		}
	}
}