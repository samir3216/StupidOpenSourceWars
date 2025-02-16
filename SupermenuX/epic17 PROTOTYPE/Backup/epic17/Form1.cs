using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Net;
using System.Net.Sockets;

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

        private UdpClient udpClient;
        private UdpClient udpServer;
        private IPEndPoint remoteEndpoint;
        private IPEndPoint localEndpoint;

        public void Shake() {
            int wnx = this.Location.X;
            int wny = this.Location.Y;

            this.Location = new Point(new Random().Next(wnx - 2, wnx + 1), new Random().Next(wny - 2, wny + 1));
            Thread.Sleep(10);
            this.Location = new Point(wnx, wny);
        }

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;

            xscroll = this.Width;
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
                xscroll = 669;
            }
            else {
                xscroll -= 2;
            }


            // Request the form to repaint
            this.Invalidate();
        }


        private float frequency = 0.1f; // Adjust the frequency of the sine wave
        private float phase = 0; // Current phase of the sine wave

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Perform all drawing here
            Graphics g = e.Graphics;

            // Draw two "DVDs" as rectangles or ellipses

            g.DrawLine(Pens.Purple, y, 140, 200, x);
            g.DrawLine(Pens.Purple, 140, x, x, y);
            g.DrawLine(Pens.Purple, x ,140, 200, y);


            Brush transparentBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));

            // Draw a transparent rectangle
            g.FillRectangle(transparentBrush, new Rectangle(12, 12, 629, 336));
            string gooftext = "Este programa for criado pelo saladino,enquanto isso pesquise e621 e aprecie como a internet é hoje em dia.";
            // Assuming g is your Graphics object and fontFamily is already defined:
            FontFamily fontFamily = new FontFamily("simsun"); // Example font family
            Font font = new Font(fontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel);

            Brush brush = new SolidBrush(Color.FromArgb(255, 0,255,0));

            g.DrawString("ENDEREÇO IP", font, brush, new PointF(36, 87));

            g.DrawString("SUPERMENUX", font, brush, new PointF(27, 36));

            phase += frequency;
            int my = (int)(Math.Sin(phase) * 3);
            if (phase > 100) { phase = 0; }

            g.DrawString(gooftext, font, brush, new PointF(xscroll, 325 + my));

            SizeF textSize = g.MeasureString(gooftext, font);
            txtsize = (int)textSize.Width;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { SoundPlayer player = new SoundPlayer(Properties.Resources.warning); player.Play(); MessageBox.Show("Oops :3 ERRO ERRO SEU TEXTO ESTÁ BRANCO Q NEM VC", "Meu waifu", MessageBoxButtons.OK, MessageBoxIcon.None); return; }

            try
            {
                // Setup UDP
                udpClient = new UdpClient();
                remoteEndpoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), 1337);
                localEndpoint = new IPEndPoint(IPAddress.Any, 1337);
                udpServer = new UdpClient(localEndpoint);

                // Send initial message
                byte[] messageBytes = Encoding.UTF8.GetBytes("y");
                udpClient.Send(messageBytes, messageBytes.Length, remoteEndpoint);

                // Start the timer to check for messages
                udpuppy.Enabled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void udpuppy_Tick(object sender, EventArgs e)
        {
            if (udpServer.Available > 0)  // Check if data is available
            {
                byte[] receivedBytes = udpServer.Receive(ref remoteEndpoint);  // Use remoteEndpoint instead of localEndpoint
                string receivedMessage = Encoding.UTF8.GetString(receivedBytes);
                button1.Text = receivedMessage;


                
                // Handle your received message here
            }
        }


    }
}
