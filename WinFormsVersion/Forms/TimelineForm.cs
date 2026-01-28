

using SimsAppJournal.Models;
using SimsAppJournal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimsAppJournal.Forms
{
    public class TimelineForm : Form
    {
        private Panel mainArea;
        private JournalService journalService;
        private List<JournalEntry> allEntries;

        // Pagination
        private int currentPage = 1;
        private int pageSize = 5;

        // Controls
        private TextBox txtSearch;
        private ComboBox cmbMoodFilter;
        private ComboBox cmbCategoryFilter;
        private FlowLayoutPanel entriesPanel;
        private Button btnPrev, btnNext;
        private Label lblPage;

        public TimelineForm()
        {
            journalService = new JournalService();
            allEntries = journalService.GetAllEntries();

            Text = "📝 Timeline";
            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;

            InitializeUI();
            LoadFilters();
            DisplayEntries();
        }

        private void InitializeUI()
        {
            // search + filters
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            txtSearch = new TextBox
            {
                PlaceholderText = "Search by title or content...",
                Width = 250,
                Location = new Point(10, 15)
            };
            txtSearch.TextChanged += (s, e) => { currentPage = 1; DisplayEntries(); };

            cmbMoodFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(280, 15),
                Width = 120
            };
            cmbMoodFilter.SelectedIndexChanged += (s, e) => { currentPage = 1; DisplayEntries(); };

            cmbCategoryFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(410, 15),
                Width = 120
            };
            cmbCategoryFilter.SelectedIndexChanged += (s, e) => { currentPage = 1; DisplayEntries(); };

            topPanel.Controls.Add(txtSearch);
            topPanel.Controls.Add(cmbMoodFilter);
            topPanel.Controls.Add(cmbCategoryFilter);

            // Main panel entries
            entriesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10)
            };

            // Bottom panel pagination
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            btnPrev = new Button
            {
                Text = "< Previous",
                Location = new Point(10, 10),
                Enabled = false
            };
            btnPrev.Click += (s, e) => { currentPage--; DisplayEntries(); };

            btnNext = new Button
            {
                Text = "Next >",
                Location = new Point(120, 10),
                Enabled = false
            };
            btnNext.Click += (s, e) => { currentPage++; DisplayEntries(); };

            lblPage = new Label
            {
                AutoSize = true,
                Location = new Point(250, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            bottomPanel.Controls.Add(btnPrev);
            bottomPanel.Controls.Add(btnNext);
            bottomPanel.Controls.Add(lblPage);

            // Main container
            mainArea = new Panel { Dock = DockStyle.Fill };
            mainArea.Controls.Add(entriesPanel);
            mainArea.Controls.Add(bottomPanel);

            Controls.Add(mainArea);
            Controls.Add(topPanel);
        }

        private void LoadFilters()
        {
            // Mood filter
            var moods = allEntries.Select(e => e.PrimaryMood).Where(m => !string.IsNullOrEmpty(m)).Distinct().ToList();
            cmbMoodFilter.Items.Clear();
            cmbMoodFilter.Items.Add("All");
            cmbMoodFilter.Items.AddRange(moods.ToArray());
            cmbMoodFilter.SelectedIndex = 0;

            // Category filter
            var categories = allEntries.Select(e => e.Category).Where(c => !string.IsNullOrEmpty(c)).Distinct().ToList();
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add("All");
            cmbCategoryFilter.Items.AddRange(categories.ToArray());
            cmbCategoryFilter.SelectedIndex = 0;
        }

        private void DisplayEntries()
        {
            entriesPanel.Controls.Clear();

            // Apply search and filters
            var filtered = allEntries.AsEnumerable();

            // Search
            string searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(e =>
                    (!string.IsNullOrEmpty(e.Title) && e.Title.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(e.Content) && e.Content.ToLower().Contains(searchText))
                );
            }

            // Mood filter
            if (cmbMoodFilter.SelectedItem != null && cmbMoodFilter.SelectedItem.ToString() != "All")
            {
                string selectedMood = cmbMoodFilter.SelectedItem.ToString();
                filtered = filtered.Where(e => e.PrimaryMood == selectedMood);
            }

            // Category filter
            if (cmbCategoryFilter.SelectedItem != null && cmbCategoryFilter.SelectedItem.ToString() != "All")
            {
                string selectedCategory = cmbCategoryFilter.SelectedItem.ToString();
                filtered = filtered.Where(e => e.Category == selectedCategory);
            }

            var filteredList = filtered.OrderByDescending(e => e.CreatedAt).ToList();

            // Pagination
            int totalPages = (int)Math.Ceiling(filteredList.Count / (double)pageSize);
            currentPage = Math.Min(Math.Max(1, currentPage), Math.Max(1, totalPages));

            var pageEntries = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            // Display
            foreach (var entry in pageEntries)
            {
                entriesPanel.Controls.Add(CreateEntryPanel(entry));
            }

            // Update pagination buttons
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            lblPage.Text = $"Page {currentPage} of {Math.Max(1, totalPages)}";
        }

        private Panel CreateEntryPanel(JournalEntry entry)
        {
            Panel panel = new Panel
            {
                Width = entriesPanel.Width - 40,
                Height = 120,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = entry.Title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 5),
                AutoSize = false,
                Width = panel.Width - 20,
                Height = 25
            };

            Label lblDate = new Label
            {
                Text = entry.CreatedAt.ToString("yyyy-MM-dd") + " | Mood: " + entry.PrimaryMood,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Location = new Point(10, 30),
                AutoSize = false,
                Width = panel.Width - 20,
                Height = 20
            };

            Label lblContent = new Label
            {
                Text = entry.Content.Length > 150 ? entry.Content.Substring(0, 150) + "..." : entry.Content,
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 55),
                AutoSize = false,
                Width = panel.Width - 20,
                Height = 50
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDate);
            panel.Controls.Add(lblContent);

            panel.Click += (s, e) =>
            {
                // Open editor on click
                EditorForm editor = new EditorForm(entry);
                editor.ShowDialog();
                allEntries = journalService.GetAllEntries();
                LoadFilters();
                DisplayEntries();
            };

            return panel;
        }
    }
}

