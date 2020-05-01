﻿using System;
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

        Item B1 = new Item(100,220,15,0.25F,new Velocity(15,0),0.1F);
        Wall W1 = new Wall(new Rectangle(700, 10, 10, 440));

        Pen linePen = new Pen(Color.DarkGray, 1);
        Pen wallPen = new Pen(Color.DarkGray, 2) { Alignment = PenAlignment.Center };
        Pen B1Pen = new Pen(Color.DarkBlue, 2) { Alignment = PenAlignment.Inset };

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLine(linePen, 10, 220, DrawingPanel.Width - 10, 220);
            e.Graphics.DrawRectangle(wallPen, W1.Extents);
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
                    // Objects collided - change velocity/direction
                    // For the 1 D version, just change the sign of velocity
                    // and substract some to mimic friction

                    // For the 2D version, change the direction property of the 
                    // velocity to 180 degrees different, and reduce the speed
                    // property to mimic friction

                    B1.V.Direction += 180;
                    B1.V.Speed = B1.V.Speed * 0.8f;
                }
            }
        }
    }
}
