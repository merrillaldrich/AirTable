using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        Item B1 = new Item(100,220,15,0.25F,10,0.1F);
        Pen linePen = new Pen(Color.DarkGray, 1);
        Pen B1Pen = new Pen(Color.DarkBlue, 2) { Alignment = PenAlignment.Inset };

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLine(linePen, 10, 220, DrawingPanel.Width - 10, 220);
            e.Graphics.DrawCircle(B1Pen, B1.XPosition, B1.YPosition, B1.Radius);
        }

        private void PhysicsTimer_Tick(object sender, EventArgs e)
        {
            Rectangle oldarea = B1.BoundingBox();
            B1.Move();
            Rectangle newarea = B1.BoundingBox();
            DrawingPanel.Invalidate(Rectangle.Union(oldarea,newarea));
        }
    }
}
