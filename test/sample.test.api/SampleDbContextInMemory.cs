using Microsoft.EntityFrameworkCore;
using repository.api;

namespace sample.test.api;

public sealed class SampleDbContextInMemory : IDisposable
{
    public static SampleDbContext CreateContext()
    {
        var option = new DbContextOptionsBuilder<SampleDbContext>().UseInMemoryDatabase("Sample_Db_Test").Options;

        var context = new SampleDbContext(option);

        if (context is not null)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        return context;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}