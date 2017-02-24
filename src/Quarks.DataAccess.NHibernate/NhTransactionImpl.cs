using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using Quarks.Transactions;
using ITransaction = NHibernate.ITransaction;

namespace Quarks.DataAccess.NHibernate
{
    internal class NhTransactionImpl : IDependentTransaction, INhTransaction
    {
        private readonly object _lock = new object();
        private bool _disposed;
        private volatile ISession _session;
        private ITransaction _transaction;

        internal NhTransactionImpl(INhSessionFactory sessionFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException(nameof(sessionFactory));

            SessionFactory = sessionFactory;
        }

        public INhSessionFactory SessionFactory { get; }

        public ISession Session
        {
            get { return GetOrCreateSession(); }
        }

        public void Dispose()
        {
            ThrowIfDisposed();

            _transaction?.Dispose();
            _transaction = null;

            _session?.Dispose();
            _session = null;

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
                        _session = SessionFactory.CreateSession();
                        _transaction = _session.BeginTransaction();
                    }
                }
            }

            return _session;
        }
    }
}