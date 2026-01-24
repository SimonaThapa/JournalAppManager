using MudBlazor;
using SimsJournalApp.Data;
using SimsJournalApp.Models;


namespace JournalApp.Services
{
    public class JournalService
    {
        private readonly AppDbContext _context;

        public JournalService(AppDbContext context)
        {
            _context = context;
        }

        // Get all journal entries
        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            return await _context.JournalEntries
                                 .Include(e => e.Tags) // include tags if you have them
                                 .OrderBy(e => e.EntryDate)
                                 .ToListAsync();
        }

        // Calculate streaks
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

        // Optional: search and filter (used in JournalList)
        public async Task<List<JournalEntry>> SearchAndFilterEntriesAsync(string search, string mood, DateRange? dateRange)
        {
            var query = _context.JournalEntries.Include(e => e.Tags).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => e.Content.Contains(search));

            if (!string.IsNullOrWhiteSpace(mood))
                query = query.Where(e => e.PrimaryMood == mood);

            if (dateRange != null)
                query = query.Where(e => e.EntryDate >= dateRange.Value.Start && e.EntryDate <= dateRange.Value.End);

            return await query.OrderByDescending(e => e.EntryDate).ToListAsync();
        }
    }
}

