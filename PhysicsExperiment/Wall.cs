using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsExperiment
{
    class Wall : PhysicsObject
    {
        //public Rectangle Extents { get; set; }

        public PointF[] Shape = new PointF[4];

        public Wall(PointF start, PointF end)
        {
            // A wall is constructed from a base line that is one of the long sides
            // then expanded by computing a parallel line to that and storing
            // the four resulting points that form a rectangle
            // Technique from https://stackoverflow.com/questions/2825412/draw-a-parallel-line

            var x1 = start.X;
            var y1 = start.Y;
            var x2 = end.X;
            var y2 = end.Y;

            var L = (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            var offsetPixels = 10.0f;

            // This is the second line
            var x1p = x1 + offsetPixels * (y2 - y1) / L;
            var y1p = y1 + offsetPixels * (x1 - x2) / L;
            var x2p = x2 + offsetPixels * (y2 - y1) / L;
            var y2p = y2 + offsetPixels * (x1 - x2) / L;

            Shape[0] = start;
            Shape[1] = end;
            Shape[2] = new PointF(x2p, y2p);
            Shape[3] = new PointF(x1p, y1p);
        }

        public Rectangle BoundingBox()
        {
            float padding = 1;
            float minX = 1000000;
            float maxX = 0;
            float minY = 1000000;
            float maxY = 0;

            foreach (PointF p in Shape)
            {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }
            return new Rectangle(
                (int)(minX - padding),
                (int)(minY - padding),
                (int)((maxX - minX) + 2 * padding),
                (int)((maxY - minY) + 2 * padding)
                );
        }

        public override void Paint(PaintEventArgs e)
        {
            Pen wallPen = new Pen(Color.DarkGray, 2) { Alignment = PenAlignment.Center };
            e.Graphics.DrawPolygon(wallPen, Shape);
        }

        public float Angle
        {
            get
            {
                // Return the angle of the base line of the wall
                // atan(opposite/adjacent)
                float angle;
                float dy = Shape[1].Y - Shape[0].Y;
                float dx = Shape[1].X - Shape[0].X;
                if (dy == 0) { angle = 0; }
                else if (dx == 0) { angle = 90; }
                else { angle = (float)(Math.Atan(-dy / dx) * 180 / Math.PI); }
                return angle;
            }
        }

        public float DistToEdge(PointF p, PointF l1, PointF l2)
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
        public float DistTo(PointF p)
        {
            // For each edge of the wall shape compute the perpendicular distance,
            // then return the minimum (the closest distance between some
            // part of the shape and the point)

            float result = 100000000f;
            float currentDist;
            int j;

            for (int i = 0; i < 4; i++)
            {
                j = i < 3 ? i + 1 : 0;
                currentDist = DistToEdge(p, Shape[i], Shape[j]);
                result = Math.Min(currentDist, result);
            }
            return result;
        }
    }
}
