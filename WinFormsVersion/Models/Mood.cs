namespace SimsAppJournal.Models
{
    public enum MoodCategory
    {
        Positive,
        Neutral,
        Negative
    }

    public class Mood
    {
        public string Name { get; set; } //getter and setter methods
        public MoodCategory Category { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

