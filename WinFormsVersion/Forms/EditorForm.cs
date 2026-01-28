using System;
using System.Drawing;
using System.Windows.Forms;
using SimsAppJournal.Models;
using SimsAppJournal.Services;

namespace SimsAppJournal.Forms
{
    public partial class EditorForm : Form
    {
        private readonly JournalService journalService;
        private readonly JournalEntry editingEntry;

        TextBox txtTitle;
        RichTextBox rtbContent;
        ComboBox cmbPrimaryMood, cmbSecondaryMood1, cmbSecondaryMood2;
        ComboBox cmbCategory, cmbTags;
        Button btnSave, btnDelete;

        public EditorForm(JournalEntry entry = null)
        {
            journalService = new JournalService();
            editingEntry = entry;

            BuildUI();

            if (editingEntry != null)
                LoadEntry();
        }

        private void BuildUI()
        {
            this.Text = editingEntry == null ? "New Journal Entry" : "Edit Journal Entry";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;

            // Title 
            txtTitle = new TextBox
            {
                PlaceholderText = "Title",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40
            };

            //  Mood Panel 
            FlowLayoutPanel moodPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(10)
            };

            cmbPrimaryMood = BuildMoodBox("Primary Mood");
            cmbSecondaryMood1 = BuildMoodBox("Secondary Mood 1");
            cmbSecondaryMood2 = BuildMoodBox("Secondary Mood 2");

            moodPanel.Controls.AddRange(new Control[]
            {
                cmbPrimaryMood,
                cmbSecondaryMood1,
                cmbSecondaryMood2
            });

            // Category & Tags Panel 
            FlowLayoutPanel tagPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(10)
            };

            // Category ComboBox
            cmbCategory = new ComboBox
            {
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new string[] { "Positive", "Neutral", "Negative" });

            // Tags ComboBox
            cmbTags = new ComboBox
            {
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            cmbTags.Items.AddRange(new string[]
            {
                "Work", "Career", "Studies", "Family", "Friends", "Relationships",
                "Health", "Fitness", "Personal Growth", "Self-care", "Hobbies", "Travel",
                "Nature", "Finance", "Spirituality", "Birthday", "Holiday", "Vacation",
                "Celebration", "Exercise", "Reading", "Writing", "Cooking", "Meditation",
                "Yoga", "Music", "Shopping", "Parenting", "Projects", "Planning", "Reflection"
            });

            tagPanel.Controls.AddRange(new Control[] { cmbCategory, cmbTags });

            //Formatting Toolbar 
            ToolStrip formattingToolbar = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden
            };

            // Bold
            ToolStripButton btnBold = new ToolStripButton("B")
            {
                CheckOnClick = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnBold.Click += (s, e) =>
            {
                if (rtbContent.SelectionFont != null)
                {
                    FontStyle style = rtbContent.SelectionFont.Style;
                    style ^= FontStyle.Bold;
                    rtbContent.SelectionFont = new Font(rtbContent.SelectionFont, style);
                }
            };

            // Italic
            ToolStripButton btnItalic = new ToolStripButton("I")
            {
                CheckOnClick = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            btnItalic.Click += (s, e) =>
            {
                if (rtbContent.SelectionFont != null)
                {
                    FontStyle style = rtbContent.SelectionFont.Style;
                    style ^= FontStyle.Italic;
                    rtbContent.SelectionFont = new Font(rtbContent.SelectionFont, style);
                }
            };

            // Underline
            ToolStripButton btnUnderline = new ToolStripButton("U")
            {
                CheckOnClick = true,
                Font = new Font("Segoe UI", 9, FontStyle.Underline)
            };
            btnUnderline.Click += (s, e) =>
            {
                if (rtbContent.SelectionFont != null)
                {
                    FontStyle style = rtbContent.SelectionFont.Style;
                    style ^= FontStyle.Underline;
                    rtbContent.SelectionFont = new Font(rtbContent.SelectionFont, style);
                }
            };

            // Bullet list
            ToolStripButton btnBullet = new ToolStripButton("•")
            {
                CheckOnClick = true
            };
            btnBullet.Click += (s, e) =>
            {
                rtbContent.SelectionBullet = !rtbContent.SelectionBullet;
            };

            // Insert Link
            ToolStripButton btnLink = new ToolStripButton("Link");
            btnLink.Click += (s, e) =>
            {
                string url = Microsoft.VisualBasic.Interaction.InputBox("Enter URL:", "Insert Link", "https://");
                if (!string.IsNullOrWhiteSpace(url))
                {
                    rtbContent.SelectedText = url;
                    rtbContent.SelectionColor = Color.Blue;
                    rtbContent.SelectionFont = new Font(rtbContent.SelectionFont ?? rtbContent.Font, FontStyle.Underline);
                }
            };

            formattingToolbar.Items.AddRange(new ToolStripItem[]
            {
                btnBold, btnItalic, btnUnderline, btnBullet, btnLink
            });

            // Content 
            rtbContent = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                AcceptsTab = true
            };

            // Buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };

            btnSave = new Button
            {
                Text = "💾 Save",
                Width = 120,
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += SaveEntry;

            btnDelete = new Button
            {
                Text = "🗑 Delete",
                Width = 120,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = editingEntry != null
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += DeleteEntry;

            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnDelete);

            //  Layout 
            this.Controls.Add(rtbContent);
            this.Controls.Add(formattingToolbar);
            this.Controls.Add(tagPanel);
            this.Controls.Add(moodPanel);
            this.Controls.Add(txtTitle);
            this.Controls.Add(buttonPanel);
        }

        private ComboBox BuildMoodBox(string placeholder)
        {
            ComboBox cmb = new ComboBox
            {
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmb.Items.AddRange(new string[]
            {
                "",
                "Happy","Excited","Relaxed","Grateful","Confident",
                "Calm","Thoughtful","Curious","Nostalgic","Bored",
                "Sad","Angry","Stressed","Lonely","Anxious"
            });

            return cmb;
        }

        private void LoadEntry()
        {
            txtTitle.Text = editingEntry.Title;

            if (!string.IsNullOrEmpty(editingEntry.Content))
                rtbContent.Rtf = editingEntry.Content;

            cmbPrimaryMood.Text = editingEntry.PrimaryMood;
            cmbSecondaryMood1.Text = editingEntry.SecondaryMood1;
            cmbSecondaryMood2.Text = editingEntry.SecondaryMood2;
            cmbCategory.Text = editingEntry.Category;
            cmbTags.Text = editingEntry.Tags;
        }

        private void SaveEntry(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(cmbPrimaryMood.Text))
            {
                MessageBox.Show("Title and Primary Mood are required.");
                return;
            }

            JournalEntry entry = editingEntry ?? new JournalEntry();
            entry.Title = txtTitle.Text;

           
            entry.Content = rtbContent.Rtf;

            entry.PrimaryMood = cmbPrimaryMood.Text;
            entry.SecondaryMood1 = cmbSecondaryMood1.Text;
            entry.SecondaryMood2 = cmbSecondaryMood2.Text;
            entry.Category = cmbCategory.Text;
            entry.Tags = cmbTags.Text;
            entry.UpdatedAt = DateTime.Now;

            if (editingEntry == null)
                journalService.AddEntry(entry);
            else
                journalService.UpdateEntry(entry);

            MessageBox.Show("Entry saved ✔");
            this.Close();
        }

        private void DeleteEntry(object sender, EventArgs e)
        {
            if (editingEntry == null) return;

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this entry?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                journalService.DeleteEntry(editingEntry.Id);
                MessageBox.Show("Entry deleted ✔");
                this.Close();
            }
        }
    }
}



