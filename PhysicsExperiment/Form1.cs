using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsExperiment
{
    public partial class PhysicsForm : Form
    {
        public PhysicsForm()
        {
            InitializeComponent();
            this.timer.Elapsed += this.Timer_Elapsed;

            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, DrawingPanel, new object[] { true });
            timer.Enabled = true;
        }

        Model m = new Model();
        HiResTimer timer = new HiResTimer(0.5f);

        int frameCounter = 0;
        int paintInterval = 25;

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            m.Paint(e);
        }

        private void Timer_Elapsed(object sender, HiResTimerElapsedEventArgs e)
        {
            if (frameCounter < paintInterval)
            {
                frameCounter += 1;
            }
            else { 
                frameCounter = 0; 
            }
            m.Move();
            m.CalculateCollisions();
            if (frameCounter == 0)
            {
                DrawingPanel.Invalidate();
            }
        }
    }
}
