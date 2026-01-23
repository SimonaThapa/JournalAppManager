using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsJournalApp.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public int? UserId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public bool IsPrebuilt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
