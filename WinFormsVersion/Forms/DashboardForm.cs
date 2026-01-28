using SimsAppJournal.Services;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SimsAppJournal.Forms
{
    public class DashboardForm : Form
    {
        private Panel mainArea;
        private readonly JournalService journalService;

        public DashboardForm()
        {
            journalService = new JournalService();
            // TITLE
            this.Text = "Sims Journal App";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;

            // Sidebar for navigation
            Panel sidebar = BuildSidebar();

            //  Main panel 
            mainArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

           


            // Add controls to the Form
            this.Controls.Add(mainArea);
            this.Controls.Add(sidebar);




            // Show Dashboard page by default
            ShowDashboard();
        }

        // Sidebar
        private Panel BuildSidebar()
        {
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            // Navigation buttons
            sidebar.Controls.Add(NavButton("📊 Dashboard", ShowDashboard));
            sidebar.Controls.Add(NavButton("📝 Timeline", ShowTimeline));
            sidebar.Controls.Add(NavButton("📅 Calendar", ShowCalendar));
            sidebar.Controls.Add(NavButton("⚙️ Settings", ShowSettings));
            sidebar.Controls.Add(NavButton("📄 Export PDF", ExportPdf));
            sidebar.Controls.Add(NavButton("➕ Add Journal", OpenAddJournal));

            return sidebar;
        }

        private void OpenAddJournal()
        {
            EditorForm addForm = new EditorForm();
            addForm.ShowDialog();
            ShowDashboard(); //show dashboard
        }
        private void ExportPdf()
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PDF File|*.pdf";
                    sfd.FileName = $"MyJournal_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"; // unique filename

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    // Call your PDF export service
                    PdfExportService pdfService = new PdfExportService();
                    pdfService.ExportEntries(DateTime.MinValue, DateTime.MaxValue, sfd.FileName);

                    MessageBox.Show(
                        $"PDF exported successfully!\n\nSaved at:\n{sfd.FileName}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(
                    $"Cannot write PDF because it is already open or in use.\nClose the file and try again.\n\nDetails: {ioEx.Message}",
                    "File Access Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error exporting PDF: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private Button NavButton(string text, Action onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 41, 59),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Font = new Font("Segoe UI", 11, FontStyle.Regular)
            };
            btn.FlatAppearance.BorderSize = 0;

            if (onClick != null)
                btn.Click += (s, e) => onClick.Invoke();

            return btn;
        }

        // Page Methods
        private void ShowDashboard()
        {
            mainArea.Controls.Clear();
            Label lbl = new Label
            {
                Text = "Welcome to your Dashboard!",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            mainArea.Controls.Add(lbl);
        }

        private void ShowTimeline()
        {
            mainArea.Controls.Clear();

            // Create TimelineForm and embed it inside mainArea
            TimelineForm timeline = new TimelineForm
            {
                TopLevel = false, 
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None
            };
            mainArea.Controls.Add(timeline);
            timeline.Show();
        }


        private void ShowCalendar()
        {
            mainArea.Controls.Clear();
            mainArea.AutoScroll = true;

            // Month Calendar 
            MonthCalendar calendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                Font = new Font("Segoe UI", 14),
                Dock = DockStyle.Fill,  // Fills the main area
                CalendarDimensions = new Size(3, 2) // 3 months wide, 2 months tall
            };
            calendar.TitleBackColor = Color.DarkSlateBlue;   // Month header background
            calendar.TitleForeColor = Color.White;          // Month header text
            //calendar.MonthBackground = Color.WhiteSmoke;    // Calendar background
            calendar.ForeColor = Color.Black;               // Regular day text
            calendar.TrailingForeColor = Color.Gray;        // Previous/next month days


            // Bold dates with journal entries
            foreach (var entry in journalService.GetAllEntries())
                calendar.AddBoldedDate(entry.CreatedAt.Date);
            calendar.UpdateBoldedDates();

            // Open editor on date select
            calendar.DateSelected += (s, e) =>
            {
                var entry = journalService.GetAllEntries()
                                          .FirstOrDefault(j => j.CreatedAt.Date == e.Start.Date);
                EditorForm editor = new EditorForm(entry);
                editor.ShowDialog();

                // Refresh bolded dates after editing
                calendar.RemoveAllBoldedDates();
                foreach (var en in journalService.GetAllEntries())
                    calendar.AddBoldedDate(en.CreatedAt.Date);
                calendar.UpdateBoldedDates();
            };


            mainArea.Controls.Add(calendar);
        }



        private void ShowSettings()
        {
            mainArea.Controls.Clear();
            mainArea.AutoScroll = true;

            //  Settings Panel
            Panel settingsPanel = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(20)
            };

            // Title
            Label title = new Label
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50
            };
            settingsPanel.Controls.Add(title);

            //  Theme Selection
            Label themeLabel = new Label
            {
                Text = "Select Theme:",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(0, 10, 0, 0)
            };
            settingsPanel.Controls.Add(themeLabel);

            ComboBox themeCombo = new ComboBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            themeCombo.Items.AddRange(new string[] { "Light", "Dark", "Blue", "Green" });
            themeCombo.SelectedIndex = 0; // Default
            settingsPanel.Controls.Add(themeCombo);

            Button applyThemeBtn = new Button
            {
                Text = "Apply Theme",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 40,
                Margin = new Padding(0, 10, 0, 0)
            };
            applyThemeBtn.Click += (s, e) =>
            {
                string selectedTheme = themeCombo.SelectedItem.ToString();
                ApplyTheme(selectedTheme);
                MessageBox.Show($"Theme changed to {selectedTheme}.", "Theme Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            settingsPanel.Controls.Add(applyThemeBtn);

            //  Change PIN 
            Label pinLabel = new Label
            {
                Text = "Change PIN:",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(0, 20, 0, 0)
            };
            settingsPanel.Controls.Add(pinLabel);

            TextBox pinBox = new TextBox
            {
                PasswordChar = '*',
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 30
            };
            settingsPanel.Controls.Add(pinBox);

            Button changePinBtn = new Button
            {
                Text = "Update PIN",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 40,
                Margin = new Padding(0, 10, 0, 0)
            };
            changePinBtn.Click += (s, e) =>
            {
                string newPin = pinBox.Text.Trim();
                if (newPin.Length < 4)
                {
                    MessageBox.Show("PIN must be at least 4 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    SettingsService.UpdatePin(newPin);
                    MessageBox.Show("PIN updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pinBox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update PIN: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            settingsPanel.Controls.Add(changePinBtn);

            mainArea.Controls.Add(settingsPanel);
        }

        // Apply Theme Method 
        private void ApplyTheme(string theme)
        {
            Color sidebarColor, headerColor, mainColor;

            switch (theme)
            {
                case "Dark":
                    sidebarColor = Color.FromArgb(20, 20, 20);
                    headerColor = Color.FromArgb(40, 40, 40);
                    mainColor = Color.FromArgb(30, 30, 30);
                    break;
                case "Blue":
                    sidebarColor = Color.FromArgb(30, 60, 120);
                    headerColor = Color.FromArgb(50, 100, 180);
                    mainColor = Color.FromArgb(200, 230, 255);
                    break;
                case "Green":
                    sidebarColor = Color.FromArgb(30, 90, 30);
                    headerColor = Color.FromArgb(50, 130, 50);
                    mainColor = Color.FromArgb(220, 255, 220);
                    break;
                default: // Light
                    sidebarColor = Color.FromArgb(30, 41, 59);
                    headerColor = Color.Black;
                    mainColor = Color.White;
                    break;
            }

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel p)
                {
                    if (p.Dock == DockStyle.Left) p.BackColor = sidebarColor;
                    else if (p.Dock == DockStyle.Top) p.BackColor = headerColor;
                    else if (p.Dock == DockStyle.Fill) p.BackColor = mainColor;
                }
            }
        }


        private void ExportPdfButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF File|*.pdf";
                sfd.FileName = "MyJournal.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        PdfExportService pdfService = new PdfExportService();
                        pdfService.ExportEntries(DateTime.MinValue, DateTime.MaxValue, sfd.FileName);
                        MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error exporting PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}

namespace iText.Kernel
{
    [Serializable]
    class PdfException : Exception
    {
        public PdfException()
        {
        }

        public PdfException(string? message) : base(message)
        {
        }

        public PdfException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}