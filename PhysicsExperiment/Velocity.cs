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

        private readonly int FrameRate = 100;

        public Velocity(float speed, float direction)
        {
            // Normalize the direction to be a positive number of
            // degrees from the x axis counter-clockwise, < 360
            // and the speed always to be positive

            if (speed < 0)
            {
                direction -= 180;
            }
            Speed = Math.Abs(speed);

            direction = direction % 360;
            if (direction < 0)
            {
                direction += 360;
            }

            Direction = direction;
        }

        public PointF ApplyTo(PointF position)
        {
            float dx = (float)((Speed/FrameRate) * (Math.Cos(Math.PI / 180 * Direction)));
            float dy = -(float)((Speed/FrameRate) * (Math.Sin(Math.PI / 180 * Direction)));
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
