using model.api;

namespace repository.api.Interfaces;

public interface IAccountRepository
{
    Task Add(Account account);
    Task<Account> GetById(int id);
    Task<Account> GetByEmail(string email);
}
