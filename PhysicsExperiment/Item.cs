using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsExperiment
{
    class Item
    {
        public float Mass { get; set; }
        public Velocity V { get; set; }
        public PointF Position { get; set; }
        public float Radius { get; set; }
        public float Friction { get; set; }
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
    }
}
