using SimsAppJournal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimsAppJournal.Services
{
    public static class Analytics
    {
        // Mood Distribution
        public static Dictionary<string, int> MoodDistribution(List<JournalEntry> entries)
        {
            return entries
                .GroupBy(e => e.PrimaryMood)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Most Used Tags 
        public static Dictionary<string, int> MostUsedTags(List<JournalEntry> entries)
        {
            var tags = new Dictionary<string, int>();
            foreach (var e in entries)
            {
                if (string.IsNullOrWhiteSpace(e.Tags)) continue;

                var splitTags = e.Tags.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(t => t.Trim());

                foreach (var tag in splitTags)
                {
                    if (tags.ContainsKey(tag))
                        tags[tag]++;
                    else
                        tags[tag] = 1;
                }
            }
            return tags;
        }

        // Category Breakdown 
        public static Dictionary<string, int> CategoryBreakdown(List<JournalEntry> entries)
        {
            return entries
                .Where(e => !string.IsNullOrWhiteSpace(e.Category))
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        //  Word Count Trends 
        public static Dictionary<DateTime, int> WordCountTrends(List<JournalEntry> entries)
        {
            return entries
                .GroupBy(e => e.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => string.IsNullOrWhiteSpace(e.Content) ? 0 : e.Content.Split(' ').Length)
                );
        }

        // Streak Info 
        public static (int CurrentStreak, int LongestStreak, List<DateTime> MissedDays) GetStreakInfo(List<JournalEntry> entries)
        {
            var dates = entries.Select(e => e.CreatedAt.Date).Distinct().OrderBy(d => d).ToList();
            int currentStreak = 0, longestStreak = 0;
            List<DateTime> missedDays = new List<DateTime>();

            if (!dates.Any()) return (0, 0, missedDays);

            DateTime prev = dates.First();
            longestStreak = currentStreak = 1;

            for (int i = 1; i < dates.Count; i++)
            {
                if ((dates[i] - dates[i - 1]).Days == 1)
                {
                    currentStreak++;
                }
                else
                {
                    for (int d = 1; d < (dates[i] - dates[i - 1]).Days; d++)
                        missedDays.Add(dates[i - 1].AddDays(d));

                    if (currentStreak > longestStreak)
                        longestStreak = currentStreak;

                    currentStreak = 1;
                }
            }

            if (currentStreak > longestStreak)
                longestStreak = currentStreak;

            if ((DateTime.Today - dates.Last()).Days > 1)
                missedDays.AddRange(Enumerable.Range(1, (DateTime.Today - dates.Last()).Days - 1)
                                              .Select(d => dates.Last().AddDays(d)));

            return (currentStreak, longestStreak, missedDays);
        }
    }
}

