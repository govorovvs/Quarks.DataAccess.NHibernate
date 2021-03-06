﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NHibernate;
using NUnit.Framework;
using Quarks.Transactions;
using ITransaction = NHibernate.ITransaction;

namespace Quarks.DataAccess.NHibernate.Tests
{
    public class NhTransactionTests
	{
		private CancellationToken _cancellationToken;
	    private INhSessionFactory _sessionManager;
	    private ISession _session;
	    private ITransaction _transaction;

	    [SetUp]
	    public void SetUp()
	    {
			_cancellationToken = new CancellationTokenSource().Token;
			_sessionManager = new Mock<INhSessionFactory>().Object;
			_session = new Mock<ISession>().Object;
		    _transaction = new Mock<ITransaction>().Object;

			Mock.Get(_sessionManager).Setup(x => x.CreateSession()).Returns(_session);
		    Mock.Get(_session).Setup(x => x.BeginTransaction()).Returns(_transaction);
	    }

		[Test]
		public void Can_Be_Constructed_With_ContextManager()
		{
			var transaction = new NhTransactionImpl(_sessionManager);

			Assert.That(transaction.SessionFactory, Is.EqualTo(_sessionManager));
		}

		[Test]
		public void Is_Instance_Of_IDependentTransaction()
		{
			INhTransaction transaction = CreateTransaction();

			Assert.That(transaction, Is.InstanceOf<IDependentTransaction>());
		}

		[Test]
		public void Session_Test()
		{
			var transaction = CreateTransaction();

			Assert.That(transaction.Session, Is.SameAs(_session));
		}

		[Test]
		public void Dispose_Disposes_Session()
		{
			Mock.Get(_session)
				.Setup(x => x.Dispose());

			var transaction = CreateTransaction();

			transaction.Dispose();

			Mock.Get(_session).VerifyAll();
		}

		[Test]
		public void Dispose_Disposes_Transaction()
		{
			Mock.Get(_transaction)
				.Setup(x => x.Dispose());

			var transaction = CreateTransaction();

			transaction.Dispose();

			Mock.Get(_transaction).VerifyAll();
		}

		[Test]
		public void Dispose_Throws_An_Exception_If_It_Was_Previously_Disposed()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();

			Assert.Throws<ObjectDisposedException>(() => transaction.Dispose());
		}

		[Test]
		public async Task Commit_Commits_Transaction()
		{
			Mock.Get(_transaction)
				.Setup(x => x.CommitAsync(_cancellationToken)).Returns(Task.CompletedTask);

			var transaction = CreateTransaction();

			await transaction.CommitAsync(_cancellationToken);

			Mock.Get(_transaction).VerifyAll();
		}

		[Test]
		public async Task Commit_Flushes_Session()
		{
		    Mock.Get(_session)
		        .Setup(x => x.FlushAsync(_cancellationToken)).Returns(Task.CompletedTask);
			var transaction = CreateTransaction();

			await transaction.CommitAsync(_cancellationToken);

			Mock.Get(_session).VerifyAll();
		}

		[Test]
		public void Commit_Throws_An_Exception_If_It_Was_Previously_Disposed()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();

			Assert.ThrowsAsync<ObjectDisposedException>(() => transaction.CommitAsync(_cancellationToken));
		}

		private NhTransactionImpl CreateTransaction()
		{
			var transaction = new NhTransactionImpl(_sessionManager);

			Assert.That(transaction.Session, Is.Not.Null);

			return transaction;
		}
	}
}
