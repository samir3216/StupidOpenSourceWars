using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;
using System.Threading;
using System.Runtime.InteropServices;

namespace megayiffmachine
{
    public partial class Form1 : Form
    {

        bool furryenabled = true;
        bool animeenabled = true;
        int chooserindex = 0;
        int clkroute = 0;

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public Form1()
        {
            InitializeComponent();
            this.MouseMove += MainForm_MouseMove;
        }
        private Point originalPosition;


        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled == false) {
                if (originalPosition != null)
                {
                    // Generate a random offset for X and Y within a range to simulate shaking
                    Random random = new Random();
                    int shakeOffsetX = random.Next(-1, 2);  // Random movement on X-axis (-5 to +5)
                    int shakeOffsetY = random.Next(-1, 2);  // Random movement on Y-axis (-5 to +5)

                    // Apply the random shake offset to the original position (not the current one)
                    this.Location = new Point(originalPosition.X + shakeOffsetX, originalPosition.Y + shakeOffsetY);
                }
            }

        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void furryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            furryenabled = furryToolStripMenuItem.Checked;
        }

        private void animeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            animeenabled = animeToolStripMenuItem.Checked;
        }

        private void randomurlindex()
        {
            if (label1.Text == "MODO FURRY") {

                string url = "https://e621.net/posts/" + new Random().Next(100, 5331712).ToString(); // the last post goes hard tho

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });

            }

            if (label1.Text == "MODO ANIME")
            {
                string url = "https://imhentai.xxx/view/" + new Random().Next(10, 1385994).ToString() + "/1/";

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (furryenabled == false && animeenabled == false) {
                MessageBox.Show("o seu branco ativa ou eu não ligo", "Ultilidade de porno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.BackColor = Color.FromArgb(240, 240, 240);

            if (furryenabled == true && animeenabled == false) {
                clkroute = 15;
                timer1.Interval = 100;
                timer1.Enabled = true;
                timer2.Enabled = false;
            }
            else if (furryenabled == false && animeenabled == true)
            {
                clkroute = 15;
                timer1.Interval = 100;
                timer1.Enabled = true;
                timer2.Enabled = false;
            }
            else if (furryenabled == true && animeenabled == true)
            {
                clkroute = 25;
                timer1.Interval = 100;
                timer1.Enabled = true;
                timer2.Enabled = false;
            } 

            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (clkroute < 0)
            {
                timer1.Enabled = false;

                // Get the device context for the entire screen (null refers to the entire screen)
                IntPtr hDC = GetDC(IntPtr.Zero);

                // Create a Graphics object from the device context
                Graphics g = Graphics.FromHdc(hDC);

                // Fill the entire screen with a black color
                g.FillRectangle(Brushes.Black, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                // Play sound
                SoundPlayer player = new SoundPlayer(Properties.Resources.bell);
                player.Play();

                // Wait a second
                Thread.Sleep(1000);

                // Now, create a new Graphics object to update the screen to white
                g = Graphics.FromHdc(hDC); // Recreate the graphics object

                // Fill the entire screen with a white color
                g.FillRectangle(Brushes.White, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);


                string text = label1.Text;
                Font font = new Font("Trebuchet MS", 48, FontStyle.Regular);
                SizeF textSize = g.MeasureString(text, font);
                float x = (Screen.PrimaryScreen.Bounds.Width - textSize.Width) / 2;
                float y = (Screen.PrimaryScreen.Bounds.Height - textSize.Height) / 2;
                g.DrawString(text, font, Brushes.Black, x, y);


                text = label1.Text;
                font = new Font("wingdings", 15, FontStyle.Regular);
                textSize = g.MeasureString(text, font);
                x = (Screen.PrimaryScreen.Bounds.Width - textSize.Width) / 2;
                y = (Screen.PrimaryScreen.Bounds.Height - textSize.Height) / 2 + 40;
                g.DrawString(text, font, Brushes.Black, x, y);


                // Wait a bit
                Thread.Sleep(500);

                // Release the device context
                ReleaseDC(IntPtr.Zero, hDC);

                // Call any other functions you need (like randomurlindex())
                randomurlindex();
                timer2.Enabled = true;
            }
            else
            {
                clkroute -= 1;
                timer1.Interval += 10;
                if (furryenabled == true)
                {
                    label1.Text = "MODO FURRY";
                    if (animeenabled == true) {
                        if (new Random().Next(0, 3) == 2)
                        {
                            label1.Text = "MODO FURRY";

                        }
                        else {
                            label1.Text = "MODO ANIME";
                        }
                    }
                }

                if (animeenabled == true && furryenabled == false)
                {
                    label1.Text = "MODO ANIME";
                }


                int centerY = Screen.FromControl(this).WorkingArea.Top + (Screen.FromControl(this).WorkingArea.Height - this.Height) / 2;


                this.Location = new Point(this.Location.X, new Random().Next(centerY - 10, centerY + 10));

                SoundPlayer player = new SoundPlayer(Properties.Resources.LINES);
                player.Play();

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Get the position of the mouse pointer
            Point mousePosition = MousePosition;

            Point buttonPosition = button1.PointToScreen(Point.Empty);

            double distance = GetDistance(mousePosition, buttonPosition);

            int dist = (int)Math.Min(255, Math.Max(0, 255 - distance) * 2);

            int greenBlue = Math.Max(0, 240 - dist);

            int fix = Math.Max(240, dist);

            this.BackColor = Color.FromArgb(fix, greenBlue, greenBlue);
            originalPosition = this.Location;
            
        }

        // Method to calculate Euclidean distance
        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private void creditosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ABOUT().Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            furryenabled = furryToolStripMenuItem.Checked;
            animeenabled = animeToolStripMenuItem.Checked;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
