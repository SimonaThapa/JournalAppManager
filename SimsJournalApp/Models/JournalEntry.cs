using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsJournalApp.Models
{
    public class JournalEntry
    {
        public int EntryId { get; set; }
        public int UserId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsMarkdown { get; set; }
        public string PrimaryMood { get; set; } = string.Empty;
        public string? SecondaryMood1 { get; set; }
        public string? SecondaryMood2 { get; set; }
        public int? CategoryId { get; set; }
        public int WordCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public List<Tag> Tags { get; set; } = new();
        public Category? Category { get; set; }
    }

}

