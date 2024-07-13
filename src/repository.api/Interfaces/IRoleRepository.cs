using model.api;

namespace repository.api.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetRoleByName(string roleName);
}
