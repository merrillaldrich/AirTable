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

        Item B1 = new Item(100,260,15,0.25F,new Velocity(20,15),0.1F);
        Wall W1 = new Wall(new Rectangle(700, 10, 10, 440));
        Wall W2 = new Wall(new Rectangle(10, 10, 700, 10));

        Pen wallPen = new Pen(Color.DarkGray, 1) { Alignment = PenAlignment.Inset };
        Pen B1Pen = new Pen(Color.DarkBlue, 2) { Alignment = PenAlignment.Inset };

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawRectangle(wallPen, W1.Extents);
            e.Graphics.DrawRectangle(wallPen, W2.Extents);
            e.Graphics.DrawCircle(B1Pen, B1.Position.X, B1.Position.Y, B1.Radius);
        }

        private void PhysicsTimer_Tick(object sender, EventArgs e)
        {
            Rectangle oldarea = B1.BoundingBox();
            B1.Move();
            Rectangle newarea = B1.BoundingBox();

            CalculateCollisions();

            DrawingPanel.Invalidate(Rectangle.Union(oldarea,newarea));
        }

        private void CalculateCollisions()
        {
            if(  B1.BoundingBox().IntersectsWith ( W1.BoundingBox()))
            {
                if( B1.Position.X + B1.Radius >= W1.Extents.X)
                {

                    // Compute the new direction our item is heading based on angle of incidence
                    B1.V.Direction = W1.Angle - (B1.V.Direction - W1.Angle);
                    B1.V.Speed = B1.V.Speed * 0.8f;

                }
            }
            if (B1.BoundingBox().IntersectsWith(W2.BoundingBox()))
            {
                if (B1.Position.Y - B1.Radius <= W2.Extents.Y + W2.Extents.Height)
                {

                    // Compute the new direction our item is heading based on angle of incidence
                    B1.V.Direction = W2.Angle - (B1.V.Direction - W2.Angle);
                    B1.V.Speed = B1.V.Speed * 0.8f;

                }
            }

        }
    }
}
