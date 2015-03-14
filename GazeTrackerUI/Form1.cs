using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GazeTrackerUI
{
    public partial class Form1 : Form
    {
        private Thread currentTimeThread = null;

        public Form1()
        {
            InitializeComponent();

            currentTimeThread = new Thread(new ThreadStart(CountTime));
            currentTimeThread.IsBackground = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void CountTime()
        {
            while (true)
            {
                Invoke(new UpdateTimeDelegate(updateCurrentTime));
                Thread.Sleep(1000);
            }
        }


        private void updateCurrentTime()
        {
            try
            {
                double dL = GazeTrackingLibrary.Tracker.dL;
                double dR = GazeTrackingLibrary.Tracker.dR;
                if (dL > 0 && dR > 0)
                {
                    lblPupilLeft.Text = "" + dL;
                    lblPupilRight.Text = "" + dR;
                    button1.SetBounds(button1.Location.X, button1.Location.Y
                    , Convert.ToInt32(dL), Convert.ToInt32(dR) );
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.StackTrace);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentTimeThread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
			currentTimeThread.Abort();
			currentTimeThread.Join();

			currentTimeThread = new Thread(new ThreadStart(CountTime));
			currentTimeThread.IsBackground = true;
        }
    }
    public delegate void UpdateTimeDelegate();
}
