﻿using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PhysicsExperiment
{
    class Item : PhysicsObject 
    {
        public float Mass { get; set; }
        public Velocity V { get; set; }
        public PointF Position { get; set; }
        public float Radius { get; set; }
        public float Friction { get; set; }

        public List<PhysicsObject> ProcessedCollisions = new List<PhysicsObject>();
        public Item( float x, float y, float radius, float mass, Velocity velocity, float friction )
        {
            Position = new PointF(x, y);
            Radius = radius;
            Mass = mass;
            V = velocity;
            Friction = friction;
        }


        public void Move()
        {
            // Move the item by one increment ("Apply" the velocity once)
            Position = V.ApplyTo(Position);

            // Adjust the velocity to remove speed due to friction
            V.ApplyFriction(Friction);     
        }

        public Rectangle BoundingBox()
        {
            int padding = 2;
            return new Rectangle((int)(Position.X - Radius) - padding, (int)(Position.Y - Radius) - padding, (int)(2*Radius) + (2*padding), (int)(2*Radius) + (2*padding));
        }
        public float DistTo(PointF p)
        {
            // Distance from some point p to the edge of this item

            var xlen = p.X - this.Position.X;
            var ylen = p.Y - this.Position.Y;

            var dxsquared = Math.Pow(xlen, 2);
            var dysquared = Math.Pow(ylen, 2);
            var sum = dxsquared + dysquared;
            var dist = (float)Math.Sqrt(sum);
            return dist - this.Radius;
        }

        public override void Paint(PaintEventArgs e)
        {
            Pen B1Pen = new Pen(Color.DarkBlue, 2) { Alignment = PenAlignment.Inset };
            e.Graphics.DrawCircle(B1Pen, Position.X, Position.Y, Radius);
        }
    }
}
