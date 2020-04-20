using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsExperiment
{
    class Velocity
    {
        public float Speed { get; set; }
        public float Direction { get; set; }

        public Velocity(float speed, float direction)
        {
            Speed = speed/10;
            Direction = direction;
        }

        public PointF ApplyTo(PointF position)
        {
            float dx = (float)(Speed * (Math.Cos(Math.PI / 180 * Direction)));
            float dy = -(float)(Speed * (Math.Sin(Math.PI / 180 * Direction)));
            return new PointF(position.X + dx, position.Y + dy);
        }

        public void ApplyFriction(float friction)
        {
            if (Speed - friction > 0)
            {
                Speed -= friction;
            }
            else
            {
                Speed = 0;
            }
        }
    }
}
