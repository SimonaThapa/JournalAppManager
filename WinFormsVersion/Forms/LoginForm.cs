using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimsAppJournal.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            // Form Setup 
            this.Text = "Sims Journal - Login";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;

            // Left Panel 
            Panel left = new Panel
            {
                Dock = DockStyle.Left,
                Width = screenWidth / 2
            };
            //picture box
            PictureBox pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            string imagePath = Path.Combine(Application.StartupPath, "Assets", "C:\\Users\\DELL\\source\\repos\\SimsJournalApp\\WinFormsVersion\\Assets\\Untitled design.png");

            if (File.Exists(imagePath))
                pic.Image = Image.FromFile(imagePath);
            else
                pic.BackColor = Color.Gray;

            left.Controls.Add(pic);

            // Overlay text (slanted/italic)
            Label quote = new Label
            {
                Text = "“Reflect, record, and grow with every entry.”",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 26, FontStyle.Bold | FontStyle.Italic),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(120, 0, 0, 0) 
            };
            quote.Parent = pic; // picture 
            quote.BringToFront();

            // Right Pane
            Panel right = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White 
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(50)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            right.Controls.Add(layout);

            // PIN TextBox (white for contrast)
            TextBox pin = new TextBox
            {
                PasswordChar = '●',
                Width = 250,
                Height = 40,
                Font = new Font("Segoe UI", 14),
                TextAlign = HorizontalAlignment.Center,
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            // Login Button
            Button login = new Button
            {
                Text = "Login",
                Width = 250,
                Height = 50,
                BackColor = Color.FromArgb(99, 102, 241), // Indigo button
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None
            };
            login.FlatAppearance.BorderSize = 0;

            layout.Controls.Add(pin, 0, 1);
            layout.Controls.Add(login, 0, 2);

            // Login Button Click 
            login.Click += (s, e) =>
            {
                if (pin.Text == "1234") // simple PIN for testing
                {
                    this.DialogResult = DialogResult.OK; 
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid PIN", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Add Panels 
            this.Controls.Add(right); // fill remaining space
            this.Controls.Add(left);  // left panel docked left
        }
    }
}







