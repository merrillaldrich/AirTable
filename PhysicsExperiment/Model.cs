using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsExperiment
{
    class Model
    {
        public List<Item> MoveableObjects = new List<Item>();
        public List<Wall> FixedObjects = new List<Wall>();

        public Model()
        {
            Item B1 = new Item(
                500,   // x position
                260,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    50, // starting speed in approximately pixels/second
                    25  // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B1);

            Wall W1 = new Wall(new PointF(700, 10), new PointF(700, 440));
            FixedObjects.Add(W1);

            Wall W2 = new Wall(new PointF(10, 10), new PointF(710, 10));
            FixedObjects.Add(W2);

            Wall W3 = new Wall(new PointF(100, 30), new PointF(400, 250));
            FixedObjects.Add(W3);
        }
        public void Move()
        {
            foreach (Item i in MoveableObjects)
            {
                i.Move();
            }
        }
        public void CalculateCollisions()
        {
            // For every item process possible collisions
            // Note in this version there is just one item moving
            // In future versions we will have to add a process to 
            // detect moving items colliding with one another

            foreach (Item i in MoveableObjects)
            {
                foreach (Wall w in FixedObjects)
                {
                    if (w.distTo(i.Position) < i.Radius)
                    {
                        // Solving this problem: if objects collide at a slow speed with a high 
                        // frame rate then the collision logic gets processed more than once,
                        // which has the effect of making the item get "stuck," as
                        // the velocity vector is changed over and over before the item 
                        // moves far enough away not to be detected as in collision any longer

                        // Save the collision to a list of processed collisions for the object
                        // which we will use to indicate the effect of this collision
                        // has already been applied

                        if (!i.ProcessedCollisions.Contains(w))
                        {
                            // Compute the new direction our item is heading based on angle of incidence
                            i.V.Direction = w.Angle - (i.V.Direction - w.Angle);
                            i.V.Speed = i.V.Speed * 0.8f;

                            // Record that this change is applied
                            i.ProcessedCollisions.Add(w);
                        }
                    }
                    else
                    {
                        // The two objects are out of range, so if we recently processed a collision
                        // between then remove this wall from the processed collisions list
                        if (i.ProcessedCollisions.Contains(w))
                        {
                            i.ProcessedCollisions.Remove(w);
                        }
                    }
                }
            }
        }
        public void Paint(PaintEventArgs e)
        {
            foreach (Item i in MoveableObjects)
            {
                i.Paint(e);
            }
            foreach (Wall w in FixedObjects)
            {
                w.Paint(e);
            }
        }
    }
}
