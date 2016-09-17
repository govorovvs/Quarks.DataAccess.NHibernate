# Quarks.DataAccess.NHibernate

[![Version](https://img.shields.io/nuget/v/Quarks.DataAccess.NHibernate.svg)](https://www.nuget.org/packages/Quarks.DataAccess.NHibernate)

## Example

Here is an example that describes how to use HNibernate with Quarks.Transactions.

```csharp
public class User : IEntity, IAggregate
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class UserMap : ClassMap<User>
{
	public UserMap()
	{
		Table("Users");

		Id(x => x.Id, "Id").GeneratedBy().Identity();
		Map(x => x.Name, "Name");
	}
}

public class UserManagementSessionManager : INhSessionManager
{
	private ISessionFactory _sessionFactory;

	public UserManagementSessionManager(string connectionString)
	{
		_sessionFactory = 
			Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();
	}

	public ISessionFactory CreateSession() => _sessionFactory.OpenSession();
	
	public int GetHashCode() => _sessionFactory.GetHashCode();
}

public class NhUserRepository : NhRepository, IUserRepository
{
	public NhUserRepository(UserManagementSessionManager manager) : base(manager) { }

	public User FindById(int id) => Session.Get<User>(id);

	public void Modify(User user) => Session.SaveOrUpdate(user);
}

public class RenameUserCommandHandler : ICommandHandler<RenameUserCommand>
{
	private readonly IUserRepository _userRepository;

	public async Task HandleAsync(RenameUserCommand command, CancellationToken ct)
	{
		using(ITransaction transaction = Transaction.BeginTransaction())
		{
			User user = _userRepository.FindById(command.Id);
			user.Name = command.Name;
			_userRepository.Modify(user);
			await transaction.CommitAsync(ct);
		}
	}
}
```

## How it works

*NhRepository* internally uses *NhTransaction* and gets it from the current *Quarks.Transaction*.

```csharp
public abstract class NhRepository(INhSessionManager sessionManager)
{
	private readonly INhSessionManager _sessionManager = sessionManager;

	protected ISession Session => Transaction.Session;
	
	private NhTransaction Transaction => NhTransaction.GetCurrent(_sessionManager);
}

internal class NhTransaction(ISession session) : IDependentTransaction
{
	public ISession Session { get;} = session;

	public static NhTransaction GetCurrent(INhSessionManager sessionManager)
	{
		int key = sessionManager.GetHashCode().ToString();
		IDependentTransaction current;
		if (!Transaction.Current.DependentTransactions.TryGetValue(key, out current))
		{
			current = new NhTransaction(sessionManager.CreateSession());
			Transaction.Current.Enlist(key, current);
		}

		return (NhTransaction)current;
	}
}
```