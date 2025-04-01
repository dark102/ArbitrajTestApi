using ArbitrajTestApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ArbitrajTestApi.Repositories
{
    public interface ITrackedPairsRepository
    {
        Task<TrackedPairs> AddAsync(TrackedPairs pair);
        Task UpdateLastDateOfEntryAsync(int id, DateTime lastDateOfEntry);
        Task UpdateIsNewAsync(int id, bool isNew);
        Task<List<TrackedPairs>> GetAllAsync();
    }
}