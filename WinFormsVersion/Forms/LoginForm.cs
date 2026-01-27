using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsAppJournal.Forms
{
    public partial class LoginForm : Form
    {
        private string enteredPin = "";
        private readonly Services.AuthorizationService authService = new Services.AuthorizationService();

        public LoginForm()
        {
            InitializeComponent();
            InitializeLoginUI();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void InitializeLoginUI()
        {
            this.Text = "SimsAppJournal - Login";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 480
            };
            this.Controls.Add(splitContainer);

            // Left panel
            splitContainer.Panel1.BackgroundImage = Image.FromFile("images/login-bg.jpg");
            splitContainer.Panel1.BackgroundImageLayout = ImageLayout.Stretch;
            var lblQuote = new Label
            {
                Text = "\"Capture your thoughts, one entry at a time.\"",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(20, 350)
            };
            splitContainer.Panel1.Controls.Add(lblQuote);

            // Right panel
            splitContainer.Panel2.BackColor = Color.FromArgb(30, 41, 59);
            var lblPinDots = new Label
            {
                Text = "○ ○ ○ ○",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(120, 50)
            };
            splitContainer.Panel2.Controls.Add(lblPinDots);

            int btnSize = 60, startX = 80, startY = 120, padding = 10;
            int number = 1;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var btn = new Button
                    {
                        Text = number.ToString(),
                        Size = new Size(btnSize, btnSize),
                        Location = new Point(startX + col * (btnSize + padding), startY + row * (btnSize + padding)),
                        BackColor = Color.FromArgb(99, 102, 241),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    int captured = number;
                    btn.Click += (s, e) => EnterDigit(captured, lblPinDots);
                    splitContainer.Panel2.Controls.Add(btn);
                    number++;
                }
            }

            // Zero button
            var zeroBtn = new Button
            {
                Text = "0",
                Size = new Size(btnSize, btnSize),
                Location = new Point(startX + btnSize + padding, startY + 3 * (btnSize + padding)),
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            zeroBtn.Click += (s, e) => EnterDigit(0, lblPinDots);
            splitContainer.Panel2.Controls.Add(zeroBtn);
        }

        private void EnterDigit(int digit, Label lblDots)
        {
            if (enteredPin.Length < 4)
            {
                enteredPin += digit;
                lblDots.Text = new string('●', enteredPin.Length).PadRight(4, '○');

                if (enteredPin.Length == 4)
                {
                    if (authService.ValidatePin(enteredPin))
                    {
                        this.Hide();
                        var dashboard = new DashboardForm();
                        dashboard.Show();
                    }
                    else
                    {
                        ShakeForm();
                        enteredPin = "";
                        lblDots.Text = "○ ○ ○ ○";
                    }
                }
            }
        }

        private void ShakeForm()
        {
            var original = this.Location;
            var rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                this.Location = new Point(original.X + rnd.Next(-10, 10), original.Y);
                System.Threading.Thread.Sleep(10);
            }
            this.Location = original;
        }
    }
}
