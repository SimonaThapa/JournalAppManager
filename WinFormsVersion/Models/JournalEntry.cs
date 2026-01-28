public class JournalEntry
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } // Markdown/Rich text
    public string PrimaryMood { get; set; } // Required
    public string SecondaryMood1 { get; set; } // Optional
    public string SecondaryMood2 { get; set; } // Optional
    public string Category { get; set; } // Optional
    public string Tags { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
