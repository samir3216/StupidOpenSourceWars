using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
namespace megayiffmachine
{
    public partial class ABOUT : Form
    {
        public ABOUT()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        private void ABOUT_Load(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Properties.Resources.loop);
            player.PlayLooping();
        }



        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Properties.Resources.loop);
            player.Stop();
            player = new SoundPlayer(Properties.Resources.s15);
            player.Play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Location = new Point(new Random().Next(this.Location.X - 1, this.Location.X + 1), this.Location.Y);
        }
    }
}
