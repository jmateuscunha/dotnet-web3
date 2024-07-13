using Moq.AutoMock;
using repository.api;
using repository.api.Interfaces;
using repository.api.Repositories;

namespace sample.test.api.Repositories;

public class AccountRepositoryTest
{
    private readonly SampleDbContext _context;
    private readonly AutoMocker _mocker;
    private readonly IAccountRepository _accountRepository;

    public AccountRepositoryTest()
    {
        _mocker = new AutoMocker();
        _context = SampleDbContextInMemory.CreateContext();
        _accountRepository = new AccountRepository(_context);
    }

    [Fact]
    public async Task AddAccount()
    {
        await _accountRepository.Add(new model.api.Account("a", "b"));        
    }
}
