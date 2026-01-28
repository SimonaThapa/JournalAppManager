using System;
using System.Collections.Generic;
using System.Linq;
using SimsAppJournal.Models;

namespace SimsAppJournal.Models
{
    public static class Analytics
    {
        // Count of entries per mood
        public static Dictionary<string, int> MoodDistribution(List<JournalEntry> entries)
        {
            return entries
                .GroupBy(e => e.PrimaryMood)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Most frequent mood
        public static string MostFrequentMood(List<JournalEntry> entries)
        {
            return entries
                .GroupBy(e => e.PrimaryMood)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "None";
        }

        // Daily streak calculation
        public static int CurrentStreak(List<JournalEntry> entries)
        {
            var dates = entries.Select(e => e.CreatedAt.Date).Distinct().OrderByDescending(d => d).ToList();
            int streak = 0;
            DateTime today = DateTime.Today;

            foreach (var d in dates)
            {
                if (d == today.AddDays(-streak))
                    streak++;
                else
                    break;
            }

            return streak;
        }

        // Longest streak
        public static int LongestStreak(List<JournalEntry> entries)
        {
            var dates = entries.Select(e => e.CreatedAt.Date).Distinct().OrderBy(d => d).ToList();
            int longest = 0, current = 1;

            for (int i = 1; i < dates.Count; i++)
            {
                if ((dates[i] - dates[i - 1]).Days == 1)
                    current++;
                else
                    current = 1;

                if (current > longest)
                    longest = current;
            }

            return longest;
        }

        // Word count trends
        public static Dictionary<DateTime, int> WordCountTrends(List<JournalEntry> entries)
        {
            return entries.ToDictionary(e => e.CreatedAt.Date,
                                        e => string.IsNullOrWhiteSpace(e.Content)
                                             ? 0
                                             : e.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);
        }

        // Most used tags
        public static Dictionary<string, int> MostUsedTags(List<JournalEntry> entries)
        {
            var allTags = entries.SelectMany(e => (e.Tags ?? "")
                                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                .Select(t => t.Trim()));
            return allTags.GroupBy(t => t)
                          .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
