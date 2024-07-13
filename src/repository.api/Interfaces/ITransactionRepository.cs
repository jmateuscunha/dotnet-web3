using model.api;

namespace repository.api.Interfaces;

public interface ITransactionRepository
{
    Task Add(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAll();
}