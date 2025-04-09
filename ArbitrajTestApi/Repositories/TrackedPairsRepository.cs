using ArbitrajTestApi.Data;
using ArbitrajTestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArbitrajTestApi.Repositories
{
    public class TrackedPairsRepository : ITrackedPairsRepository
    {
        private readonly ApplicationDbContext _context;

        public TrackedPairsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TrackedPairs> AddAsync(TrackedPairs pair)
        {
            var entry = await _context.TrackedPairs.AddAsync(pair);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task UpdateLastDateOfEntryAsync(int id, DateTime lastDateOfEntry)
        {
            var pair = await _context.TrackedPairs.FindAsync(id);
            if (pair != null)
            {
                pair.LastDateOfEntry = lastDateOfEntry;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<TrackedPairs>> GetAllAsync()
        {
            return await _context.TrackedPairs.ToListAsync();
        }
    }
}