using System;
using System.Threading;
using System.Threading.Tasks;
using Quarks.DataAccess.NHibernate.SessionManagement;
using NHibernate;
using Quarks.Transactions;
using ITransaction = NHibernate.ITransaction;

namespace Quarks.DataAccess.NHibernate
{
	internal class NhTransaction : IDependentTransaction
	{
		private static readonly object StaticLock = new object();
		private readonly object _lock = new object();
		private bool _disposed;
		private volatile ISession _session;
		private ITransaction _transaction;

		internal NhTransaction(INhSessionManager sessionManager)
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

			return Task.FromResult(0);
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

		public static NhTransaction GetCurrent(INhSessionManager sessionManager)
		{
			Transaction transaction = Transaction.Current;
			if (transaction == null)
			{
				return new NhTransaction(sessionManager);
			}

			string key = sessionManager.GetHashCode().ToString();

			IDependentTransaction current;
			if (!transaction.DependentTransactions.TryGetValue(key, out current))
			{
				lock (StaticLock)
				{
					if (!transaction.DependentTransactions.TryGetValue(key, out current))
					{
						current = new NhTransaction(sessionManager);
						transaction.Enlist(key, current);
					}
				}
			}

			return (NhTransaction)current;
		}
	}
}