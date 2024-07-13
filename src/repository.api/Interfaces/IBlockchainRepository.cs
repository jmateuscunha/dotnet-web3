using model.api;

namespace repository.api.Interfaces;

public interface IBlockchainRepository
{
    Task<Blockchain> GetBlockchainById(Guid id);
}
