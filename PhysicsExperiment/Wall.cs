using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsExperiment
{
    class Wall
    {
        public Rectangle Extents { get; set; }

        public Wall(Rectangle extents)
        {
            Extents = new Rectangle(extents.Location, extents.Size);
        }

        public Rectangle BoundingBox()
        {
            return new Rectangle(Extents.Location, Extents.Size);
        }

        public float Angle
        {
            get
            {
                float angle = Extents.Width > Extents.Height ? 0 : 90;
                return angle;
            }
        }
    }
}
