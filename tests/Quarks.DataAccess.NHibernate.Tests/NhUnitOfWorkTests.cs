using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NHibernate;
using NUnit.Framework;
using Quarks.DataAccess.NHibernate.SessionManagement;
using Quarks.DomainModel.Impl;

namespace Quarks.DataAccess.NHibernate.Tests
{
    public class NhUnitOfWorkTests
	{
		private CancellationToken _cancellationToken;
	    private INhSessionManager _sessionManager;
	    private ISession _session;
	    private ITransaction _transaction;

	    [SetUp]
	    public void SetUp()
	    {
			_cancellationToken = new CancellationTokenSource().Token;
			_sessionManager = new Mock<INhSessionManager>().Object;
			_session = new Mock<ISession>().Object;
		    _transaction = new Mock<ITransaction>().Object;

			Mock.Get(_sessionManager)
				.Setup(x => x.CreateSession())
				.Returns(_session);
		    Mock.Get(_session)
			    .Setup(x => x.BeginTransaction())
			    .Returns(_transaction);
	    }

		[Test]
		public void Can_Be_Constructed_With_ContextManager()
		{
			var unitOfWork = new NhUnitOfWork(_sessionManager);

			Assert.That(unitOfWork.SessionManager, Is.EqualTo(_sessionManager));
		}

		[Test]
		public void Is_Instance_Of_IDependentUnitOfWork()
		{
			NhUnitOfWork unitOfWork = CreateUnitOfWork();

			Assert.That(unitOfWork, Is.InstanceOf<IDependentUnitOfWork>());
		}

		[Test]
		public void Session_Test()
		{
			NhUnitOfWork unitOfWork = CreateUnitOfWork();

			Assert.That(unitOfWork.Session, Is.SameAs(_session));
		}

		[Test]
		public void Dispose_Disposes_Session()
		{
			Mock.Get(_session)
				.Setup(x => x.Dispose());

			NhUnitOfWork unitOfWork = CreateUnitOfWork();

			unitOfWork.Dispose();

			Mock.Get(_session).VerifyAll();
		}

		[Test]
		public void Dispose_Disposes_Transaction()
		{
			Mock.Get(_transaction)
				.Setup(x => x.Dispose());

			NhUnitOfWork unitOfWork = CreateUnitOfWork();

			unitOfWork.Dispose();

			Mock.Get(_transaction).VerifyAll();
		}

		[Test]
		public void Dispose_Throws_An_Exception_If_It_Was_Previously_Disposed()
		{
			var unitOfWork = CreateUnitOfWork();

			unitOfWork.Dispose();

			Assert.Throws<ObjectDisposedException>(() => unitOfWork.Dispose());
		}

		[Test]
		public async Task Commit_Commits_Transaction()
		{
			Mock.Get(_transaction)
				.Setup(x => x.Commit());

			var unitOfWork = CreateUnitOfWork();

			await unitOfWork.CommitAsync(_cancellationToken);

			Mock.Get(_transaction).VerifyAll();
		}

		[Test]
		public async Task Commit_Flushes_Session()
		{
			Mock.Get(_session)
				.Setup(x => x.Flush());
			var unitOfWork = CreateUnitOfWork();

			await unitOfWork.CommitAsync(_cancellationToken);

			Mock.Get(_session).VerifyAll();
		}

		[Test]
		public void Commit_Throws_An_Exception_If_It_Was_Previously_Disposed()
		{
			var unitOfWork = CreateUnitOfWork();

			unitOfWork.Dispose();

			Assert.ThrowsAsync<ObjectDisposedException>(() => unitOfWork.CommitAsync(_cancellationToken));
		}

		private NhUnitOfWork CreateUnitOfWork()
		{
			var unitOfWork = new NhUnitOfWork(_sessionManager);

			Assert.That(unitOfWork.Session, Is.Not.Null);

			return unitOfWork;
		}
	}
}
