using MudBlazor;
using SimsJournalApp.Data;
using SimsJournalApp.Models;

namespace JournalApp.Services
{
    public class JournalService
    {
        private readonly AppDbContext _context;

        public JournalEntry(AppDbContext context)
        {
            _context = context;
        }

        // Existing CRUD methods
        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            return await _context.JournalEntries.OrderBy(e => e.EntryDate).ToListAsync();
        }

        // <--- Place the streak calculation here
        public List<DateTime> CalculateStreaks(List<JournalEntry> entries, out int currentStreak, out int longestStreak)
        {
            var ordered = entries.OrderBy(e => e.EntryDate).ToList();
            List<DateTime> missed = new();
            currentStreak = 0;
            longestStreak = 0;
            DateTime? lastDate = null;

            foreach (var e in ordered)
            {
                if (lastDate != null)
                {
                    int gap = (e.EntryDate - lastDate.Value).Days;
                    if (gap > 1)
                    {
                        for (int i = 1; i < gap; i++)
                            missed.Add(lastDate.Value.AddDays(i));
                        currentStreak = 0;
                    }
                    else currentStreak++;
                }
                else currentStreak = 1;

                longestStreak = Math.Max(longestStreak, currentStreak);
                lastDate = e.EntryDate;
            }
            return missed;
        }
    }
}
