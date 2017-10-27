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
            SessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
        }

        public INhSessionFactory SessionFactory { get; }

        public ISession Session => GetOrCreateSession();

        public void Dispose()
        {
            ThrowIfDisposed();

            _transaction?.Dispose();
            _transaction = null;

            _session?.Dispose();
            _session = null;

            _disposed = true;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (_session == null)
                return;

            await _transaction.CommitAsync(cancellationToken)
                .ConfigureAwait(false);
            await _session.FlushAsync(cancellationToken)
                .ConfigureAwait(false);
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