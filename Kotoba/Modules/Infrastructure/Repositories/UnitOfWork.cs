using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;

namespace Kotoba.Modules.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KotobaDbContext _context;

        public UnitOfWork(KotobaDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
