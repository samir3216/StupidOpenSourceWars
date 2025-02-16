using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace epic17
{
    public partial class Form1 : Form
    {

        private int y = 50; // Initial position
        private int x = 50; // Another DVD position
        private int ySpeed = 5; // Speed of movement in the y-axis
        private int xSpeed = 5; // Speed of movement in the x-axis
        int xscroll;
        int txtsize;
        string statustext = "Esperando";
        string gooftext;


        bool updatedisplay = true;

        string[] rndx = { "Este programa foi criado pelo saladino, enquanto isso pesquise e621 e aprecie como a internet é hoje em dia.",
                "Ele queima ou não queima?? VISHHHHH DAYM",
                "Aí o que eu disse pra ele eu falei OiIiIiiIi EU Tenho fetiche de gigantismo e falei pra ele a vai tomar no cu fdp",
                "Roblox vai acabar em 2030 ve o que estou falando,aí ninguem vai ter robux roblox roblox"
                , "Meu mano foi num lugar que ele morava antes sabe aí quando a casa dele pegou fogo falaram logo que ele era chris chan"
                , "As disciplinas da humanidades são quando um ser humano começa quer uma atração estranha com tubarões gays e especificamente com olhos FUCK ME.",
                "EU SOU DJ DJ SALIN ESTOU AQUI DENOVO PARA TE FALAR QUE MEU FETICHE É QUE NEM RAP QUE NEM DRAGÕES GRAVIDAS"};




        public void Shake()
        {
            int wnx = this.Location.X;
            int wny = this.Location.Y;

            this.Location = new Point(new Random().Next(wnx - 2, wnx + 2), new Random().Next(wny - 2, wny + 2));
            Thread.Sleep(10);
            this.Location = new Point(wnx, wny);
        }

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
            this.Resize += Form1_Resize;
            xscroll = this.Width;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                updateisplay.Enabled = false;
            }

            if (WindowState == FormWindowState.Normal)
            {
                updateisplay.Enabled = true;
            }
        }


        private void updatedisplay_tick(object sender, EventArgs e)
        {
            // Update the position
            y += ySpeed;
            x += xSpeed;

            // Bounce logic for y-axis
            if (y <= 0 || y >= this.ClientSize.Height - 1) // Assuming 50 is the size of the DVD
            {
                ySpeed = -ySpeed; // Reverse direction
                Shake();
            }

            // Bounce logic for x-axis
            if (x <= 0 || x >= this.ClientSize.Width - 1) // Assuming 50 is the size of the DVD
            {
                xSpeed = -xSpeed; // Reverse direction
                Shake();
            }

            if (xscroll < -txtsize - 15)
            {
                gooftext = rndx[new Random().Next(0, rndx.Length)];
                xscroll = 669;
                
            }
            else
            {
                xscroll -= 2;
            }


            // Request the form to repaint
            this.Invalidate();
        }


        private float frequency = 0.1f; // Adjust the frequency of the sine wave
        private float phase = 0; // Current phase of the sine wave
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (updatedisplay == false)
            {
                pictureBox1.Location = new Point(new Random().Next(222- 2,222 + 2), new Random().Next(158 - 2, 158 + 2));
                return;
            }
            // Perform all drawing here
            Graphics g = e.Graphics;

            // Draw two "DVDs" as rectangles or ellipses

            //cooll
            g.DrawLine(Pens.White, y, 140, 200, x);
            g.DrawLine(Pens.White, 140, x, x, y);
            g.DrawLine(Pens.White, x, 140, 200, y);
            g.DrawLine(Pens.White, y, 140, x, y);
            g.DrawLine(Pens.White, x, 200, y, y);
            g.DrawLine(Pens.White, x, x, 200, x);


            Brush transparentBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));

            // Draw a transparent rectangle
            g.FillRectangle(transparentBrush, new Rectangle(12, 12, 629, 336));
            
            // Assuming g is your Graphics object and fontFamily is already defined:
            FontFamily fontFamily = new FontFamily("simsun"); // Example font family
            Font font = new Font(fontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel);

            Font font2 = new Font(new FontFamily("Arial"), 10, FontStyle.Regular, GraphicsUnit.Pixel);

            Brush brush = new SolidBrush(Color.FromArgb(255, 0, 255, 0));

            Brush whitebrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

            g.DrawString("ENDEREÇO IP", font, brush, new PointF(36, 87));

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 0, 31, 669, 41);

            g.DrawString("SUPERMENUX", font, brush, new PointF(27, 36));

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 27, 151, 317, 180);

            g.DrawString(statustext, font2, brush, new PointF(36, 165));

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 589, 0, 31, 399);

            phase += frequency;
            int my = (int)(Math.Sin(phase) * 3);
            if (phase > 100) { phase = 0; }

            g.DrawString(gooftext, font, whitebrush, new PointF(xscroll, 325 + my));

            SizeF textSize = g.MeasureString(gooftext, font);
            txtsize = (int)textSize.Width;



        }


        private void fireGUI()
        {


            SPLASH splashwnd = new SPLASH();
            splashwnd.Show();

            // Create the NotifyIcon
            NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.Icon = SystemIcons.Application; // Default application icon
                notifyIcon.Text = "uwu"; // Tooltip text
                notifyIcon.Visible = true;

                notifyIcon.BalloonTipTitle = "Waifu";
                notifyIcon.BalloonTipText = "Iniciou,ESC Para Disconnectar :3";
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;

                notifyIcon.ShowBalloonTip(300);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Text = "SUPERMENUX | Modo grafico : GDI+ | HLT";
            progressBar1.Value = 0;
            statustext = "REQUEST";
            if (textBox1.Text == "") { SoundPlayer player = new SoundPlayer(Properties.Resources.warning); player.Play(); MessageBox.Show("Oops :3 ERRO ERRO SEU TEXTO ESTÁ BRANCO Q NEM VC", "Meu waifu", MessageBoxButtons.OK, MessageBoxIcon.None); return; }
            statustext += "\n" + "OK";
            this.WindowState = FormWindowState.Minimized;


            statustext += "\n" + "NOTIF 1";


            // Get the argument from the text box
            string argument = textBox1.Text;

            Process.Start(@"ipreceiver.exe", argument);

            Thread.Sleep(1500);


            fireGUI();
            Process.Start("powerpnt.exe", "/s \"point.pptx");

            progressBar1.Value = 100;
            statustext += "\n" + "PASS";

            this.Text = "SUPERMENUX | Modo grafico : GDI+";

            hudmode(2);
        }

        void hudmode (int mode)
        {
            if (mode == 1)
            {
                this.DoubleBuffered = true;
                pictureBox1.Visible = false;
                textBox1.Visible = true;
                progressBar1.Visible = true;
                button1.Visible = true;
                updatedisplay = true;
                this.BackColor = Color.FromArgb(255, 47, 79, 79);
            }

            if (mode == 2)
            {
                this.DoubleBuffered = false;
                textBox1.Visible = false;
                progressBar1.Visible = false;
                button1.Visible = false;
                pictureBox1.Visible = true;
                this.BackColor = Color.FromArgb(255, 0, 0, 0);
                updatedisplay = false;
                this.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gooftext = rndx[new Random().Next(0,rndx.Length)];
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            progressBar1.Value = 0;
            statustext = "Esperando";
            Thread.Sleep(1000);
            hudmode(1);
        }
    }
}
