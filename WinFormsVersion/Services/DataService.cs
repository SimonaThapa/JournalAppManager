using SimsAppJournal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimsAppJournal.Services
{
    public partial class DataService : Form
    {
        public DataService()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        internal List<JournalEntry> GetAllEntries()
        {
            throw new NotImplementedException();
        }

        internal JournalEntry GetEntryByDate(DateTime selectionStart)
        {
            throw new NotImplementedException();
        }

        internal void WipeAllEntries()
        {
            throw new NotImplementedException();
        }

        internal void SaveEntry(JournalEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
