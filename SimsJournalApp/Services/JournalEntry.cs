using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimsJournalApp.Models;
using SQLite;

namespace SimsJournalApp.Services
{
    public class JournalEntryService
    {
        private readonly SQLiteAsyncConnection _db;

        public JournalEntryService(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        // CREATE
        public async Task CreateEntryAsync(JournalEntry entry)
        {
            int count = await _db.Table<JournalEntry>()
                 .Where(e => e.EntryDate == entry.EntryDate.Date
             && e.UserId == entry.UserId)
              .CountAsync();

            if (count > 0)
                throw new InvalidOperationException(
                    "Only one journal entry is allowed per day.");

            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;
            entry.WordCount = entry.Content
                .Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            await _db.InsertAsync(entry);
        }

        // UPDATE
        public async Task UpdateEntryAsync(JournalEntry entry)
        {
            entry.UpdatedAt = DateTime.Now;
            entry.WordCount = entry.Content
                .Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            await _db.UpdateAsync(entry);
        }

        
        public async Task DeleteEntryAsync(int entryId)
        {
            await _db.DeleteAsync<JournalEntry>(entryId);
        }

        // By date
        public async Task<JournalEntry?> GetEntryByDateAsync(
            DateTime date, int userId)
        {
            return await _db.Table<JournalEntry>()
                .FirstOrDefaultAsync(e =>
                    e.EntryDate == date.Date && e.UserId == userId);
        }

        
        public async Task<List<JournalEntry>> GetAllEntriesAsync(int userId)
        {
            return await _db.Table<JournalEntry>()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EntryDate)
                .ToListAsync();
        }
    }
}
