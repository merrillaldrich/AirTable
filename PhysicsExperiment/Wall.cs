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

        public PointF TopLeft
        {
            get
            {
                return new PointF(Extents.Left, Extents.Top);
            }
        }
        public PointF TopRight
        {
            get
            {
                return new PointF(Extents.Right, Extents.Top);
            }
        }
        public PointF BottomLeft
        {
            get
            {
                return new PointF(Extents.Left, Extents.Bottom );
            }
        }
        public PointF BottomRight
        {
            get
            {
                return new PointF(Extents.Right, Extents.Bottom);
            }
        }

        public float distToEdge( PointF p, PointF l1, PointF l2)
        {
            // Repurposed logic from this solution
            // https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment

            var x1 = l1.X;
            var y1 = l1.Y;
            var x2 = l2.X;
            var y2 = l2.Y;

            var A = p.X - x1;
            var B = p.Y - y1;
            var C = x2 - x1;
            var D = y2 - y1;
            var dot = A * C + B * D;
            var len_sq = C * C + D * D;
            var param = -1f;
            if (len_sq != 0) { param = dot / len_sq; }

            float xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }
            var dx = p.X - xx;
            var dy = p.Y - yy;
            return (float)Math.Sqrt(dx * dx + dy * dy);

        }
        public float distTo(PointF p)
        {
            float result;

            var d1 = distToEdge(p, this.TopLeft, this.TopRight);
            var d2 = distToEdge(p, this.TopRight, this.BottomRight);
            result = Math.Min(d1, d2);
            var d3 = distToEdge(p, this.BottomRight, this.BottomLeft);
            result = Math.Min(result, d3);
            var d4 = distToEdge(p, this.BottomLeft, this.TopLeft);
            result = Math.Min(result, d4);
            return result;

        }
    }
}
