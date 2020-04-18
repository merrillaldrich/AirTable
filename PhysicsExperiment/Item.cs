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
        public float Velocity { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public float Friction { get; set; }
        public Item( float x, float y, float radius, float mass, float velocity, float friction )
        {
            X = x;
            Y = y;
            Radius = radius;
            Mass = mass;
            Velocity = velocity;
            Friction = friction;
        }

        public void Move()
        {          
            X += Velocity;

            if (Velocity > 0)
            {
                if (Velocity - Friction > 0)
                {
                    Velocity -= Friction;
                }
                else
                {
                    Velocity = 0;
                }           
            } else if( Velocity < 0 ) {
                if (Velocity + Friction < 0)
                {
                    Velocity += Friction;
                }
                else {
                    Velocity = 0;
                }
            }
            
        }

        public Rectangle BoundingBox()
        {
            int padding = 2;
            return new Rectangle((int)(X - Radius) - padding, (int)(Y - Radius) - padding, (int)(2*Radius) + (2*padding), (int)(2*Radius) + (2*padding));
        }
    }
}
