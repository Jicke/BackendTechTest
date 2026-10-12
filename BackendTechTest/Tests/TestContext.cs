using Microsoft.EntityFrameworkCore;
using BackendTechTest.Data;

namespace BackendTechTest.Tests
{
    public class TestContext
    {
        public readonly AppDbContext Context = new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
    }
}
