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
                200,   // x position
                200,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    0, // starting speed in approximately pixels/second
                    0  // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B1);

            Item B2 = new Item(
                500,   // x position
                200,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    50, // starting speed in approximately pixels/second
                    180  // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B2);

            Wall W1 = new Wall(new PointF(10, 10), new PointF(710, 10));
            FixedObjects.Add(W1);

            Wall W2 = new Wall(new PointF(700, 10), new PointF(700, 440));
            FixedObjects.Add(W2);

            Wall W3 = new Wall(new PointF(710, 440), new PointF(10, 440));
            FixedObjects.Add(W3);

            Wall W4 = new Wall(new PointF(20, 440), new PointF(20, 10));
            FixedObjects.Add(W4);

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
                    if (w.DistTo(i.Position) < i.Radius)
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
                // Todo: for every OTHER moving item
                Item o1;
                Item o2;
                for (int j = 0; j < MoveableObjects.Count - 1; j++)
                {
                    o1 = MoveableObjects[j];
                    for (int k = j + 1; k < MoveableObjects.Count; k++)
                    {
                        o2 = MoveableObjects[k];

                        // See if the objects are in collision
                        if (o1.DistTo(o2.Position) < o2.Radius)
                        {
                            if (!o1.ProcessedCollisions.Contains(o2))
                            {
                                // Compute the new direction our item is heading

                                // Calculate the angle of the line between the center
                                // of o1 and the center of o2. This is the line perpendicular
                                // to the tangent of the surface of the ball(s) right at the point
                                // where they meet. It's the only direction that forces can be
                                // transmitted between the balls. Any change in the velocity
                                // of each ball can only be along this direction, so it helps 
                                // to calculate the velocity change with that angle as our
                                // frame of reference. See
                                // https://en.wikipedia.org/wiki/Elastic_collision
                                // section "Two Dimensional"

                                // Collision "normal" angle cn in degrees

                                double dx = o2.Position.X - o1.Position.X;
                                double dy = o2.Position.Y - o1.Position.Y;
                                float cn = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);

                                Console.WriteLine("Smash");
                                Console.WriteLine(String.Format("Object 1 Speed: {0} Direction {1}", o1.V.Speed, o1.V.Direction));
                                Console.WriteLine(String.Format("Object 2 Speed: {0} Direction {1}", o2.V.Speed, o2.V.Direction));
                                Console.WriteLine(String.Format("Collision angle {0}", cn));

                                // Express the velocity of o1 in x and y directions with that 
                                // angle as our local "x" axis and the line along the tangent of the
                                // ball surface as the "y" axis. Only x can change; y remains
                                // constant.

                                // Temp velocity angle tva
                                float tva = o1.V.Direction - cn;

                                // Break the velocity into "local" x and y components

                                // local speeds object 1
                                float o1xs;
                                float o1ys;
                                if (o1.V.Speed == 0)
                                {
                                    o1xs = 0;
                                    o1ys = 0;
                                }
                                else
                                {
                                    o1xs = o1.V.Speed * (float)(Math.Cos(tva * Math.PI / 180));
                                    o1ys = o1.V.Speed * (float)(Math.Sin(tva * Math.PI / 180));
                                }
                                // local speeds object 2
                                float o2xs;
                                float o2ys;
                                if (o2.V.Speed == 0)
                                {
                                    o2xs = 0;
                                    o2ys = 0;
                                }
                                else
                                {
                                    o2xs = o2.V.Speed * (float)(Math.Cos(tva * Math.PI / 180));
                                    o2ys = o2.V.Speed * (float)(Math.Sin(tva * Math.PI / 180));
                                }

                                // For the simple case we have balls of the same mass, so in an elastic
                                // collision where momentum is preserved, they just exchange speeds
                                // (the masses being equal causes the mass component of the equation
                                // to cancel) -- BUT only in the Y direction of our local axes

                                // newDirection for o1 = solve triangle for angle, using local x from *o2* 
                                // and local y from o1

                                float newDir = (float)(Math.Atan2(o1ys, o2xs) * 180 / Math.PI);

                                // New speed = solve triangle for the hypoteneuse, same inputs
                                // sqrt ( object1 y squared + object2 x squared )

                                float newSpeed = (float)(Math.Sqrt((o1ys * o1ys) + (o2xs * o2xs)));

                                // "Turn" this back to the original frame of reference, the original
                                // object orientations

                                newDir = newDir + cn;

                                o1.V = new Velocity(newSpeed * 100, newDir);
                                Console.WriteLine(String.Format("Speed: {0} Direction {1} ", o1.V.Speed, o1.V.Direction));

                                // Repeat this computation for o2, exchanging o1's x speed

                                newDir = (float)(Math.Atan2(o2ys, o1xs) * 180 / Math.PI);
                                newSpeed = (float)(Math.Sqrt((o2ys * o2ys) + (o1xs * o1xs)));

                                newDir = newDir + cn;

                                o2.V = new Velocity(newSpeed * 100, newDir);
                                Console.WriteLine(String.Format("Speed: {0} Direction {1} ", o2.V.Speed, o2.V.Direction));

                                // Record that this change is applied
                                o1.ProcessedCollisions.Add(o2);
                            }
                        }
                        else
                        {
                            // The two objects are out of range, so if we recently processed a collision
                            // between then remove this wall from the processed collisions list
                            if (o1.ProcessedCollisions.Contains(o2))
                            {
                                o1.ProcessedCollisions.Remove(o2);
                            }
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
