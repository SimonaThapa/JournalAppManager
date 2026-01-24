using SimsJournalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsJournalApp.Data
{
    internal class AppDbContext
    {
        public ISet<JournalEntry> JournalEntries => Set <JournalEntry>();

        private ISet<T> Set<T>()
        {
            throw new NotImplementedException();
        }
    }
}
