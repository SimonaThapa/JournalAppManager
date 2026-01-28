using SimsAppJournal.Models;
using SimsAppJournal.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimsAppJournal.Forms
{
    public class CalendarForm : Form
    {
        private MonthCalendar calendar;
        private JournalService journalService;

        public CalendarForm()
        {
            journalService = new JournalService();

            Text = "📅 Calendar";
            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;

            InitializeUI();
            HighlightDays();
        }

        private void InitializeUI()
        {
            calendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                Location = new Point(50, 50),
                Font = new Font("Segoe UI", 14),
                CalendarDimensions = new Size(3, 2), 
            };

            calendar.DateSelected += Calendar_DateSelected;
            Controls.Add(calendar);
        }

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start.Date;

            // Check if an entry exists for this date
            var entry = journalService.GetAllEntries().FirstOrDefault(j => j.CreatedAt.Date == selectedDate);

            var editor = new EditorForm(entry);
            editor.ShowDialog();

            HighlightDays(); 
        }

        private void HighlightDays()
        {
            var entries = journalService.GetAllEntries();

            calendar.RemoveAllBoldedDates();

            foreach (var entry in entries)
            {
                calendar.AddBoldedDate(entry.CreatedAt.Date);
            }

            calendar.UpdateBoldedDates();
        }
    }
}


