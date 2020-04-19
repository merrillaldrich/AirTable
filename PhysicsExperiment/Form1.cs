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
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, DrawingPanel, new object[] { true });
            PhysicsTimer.Enabled = true;
        }

        Model m = new Model();


        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            m.Paint(e);
        }

        private void PhysicsTimer_Tick(object sender, EventArgs e)
        {
            m.Move();
            m.CalculateCollisions();
            DrawingPanel.Invalidate();
        }
    }
}
