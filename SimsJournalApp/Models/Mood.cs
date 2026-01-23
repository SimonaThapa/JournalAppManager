using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsJournalApp.Models
{
    public enum MoodCategory
    {
        Positive,
        Neutral,
        Negative
    }

    public class Mood
    {
        public string Name { get; set; }
        public MoodCategory Category { get; set; }

        public Mood(string name, MoodCategory category)
        {
            Name = name;
            Category = category;
        }
    }
}
