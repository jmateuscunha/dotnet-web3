using Microsoft.EntityFrameworkCore;
using model.api;
using repository.api.Interfaces;
using shared.api.Exceptions;

namespace repository.api.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly SampleDbContext _db;
    public RoleRepository(SampleDbContext db)
    {
        _db = db;
    }
    public async Task<Role> GetRoleByName(string roleName)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(c => c.Name.ToUpper() == roleName.ToUpper());

        return role is null ? throw new DomainException("SAMPLE-001", "Role not found.") : role;
    }
}
