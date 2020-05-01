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
                280,   // x position
                200,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    75, // starting speed in approximately pixels/second
                    0  // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B1);

            Item B2 = new Item(
                400,   // x position
                215,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    0, // starting speed in approximately pixels/second
                    0 // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B2);

            Item B3 = new Item(
                400,   // x position
                300,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    200, // starting speed in approximately pixels/second
                    90 // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B3);

            Item B4 = new Item(
                300,   // x position
                100,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    50, // starting speed in approximately pixels/second
                    45 // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B4);

            Item B5 = new Item(
                320,   // x position
                100,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    75, // starting speed in approximately pixels/second
                    20 // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B5);

            Item B6 = new Item(
                340,   // x position
                110,   // y position
                15,    // radius
                0.25F, // mass
                new Velocity(
                    50, // starting speed in approximately pixels/second
                    210 // starting direction (degrees from +x counterclockwise)
                    ),
                0.002F   // friction
                );
            MoveableObjects.Add(B6);

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

            foreach (Item i in MoveableObjects)
            {
                foreach (Wall w in FixedObjects)
                {
                    if (w.DistTo(i.Position) < i.Radius)
                    {

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
                            // A collision is detected (for the first time)
                            // Console.WriteLine("Smash");

                            // For the next most complex case, 2d collision,
                            // we have to account for the velocity as separate x
                            // and y vectors, and to rotate our frame of reference
                            // to align with the line between the centers of the two
                            // balls, the direction of the collision instead of the 
                            // orientation of the original frame

                            // In order to conserve momentum, the momenta of
                            // each of the two colliding objects are exchanged. And because  
                            // mass of the objects in this simple example is the same,
                            // we can just exchange their velocities, and the masses cancel:

                            double dx = o2.Position.X - o1.Position.X;
                            double dy = -(o2.Position.Y - o1.Position.Y);
                            float cn = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);

                            //if (cn < 0) cn += 180;

                            // Console.WriteLine(String.Format("Angle of collision: {0}", cn));

                            // "Rotate" the vectors for the two objects into a frame parallel
                            // with the line of collision, and calculate the X and Y speeds
                            // of each relative to that frame

                            var o1LocalDir = o1.V.Direction - cn;

                            var o1RefXSpeed = Math.Cos(o1LocalDir * Math.PI / 180) * o1.V.Speed;
                            var o1RefYSpeed = Math.Sin(o1LocalDir * Math.PI / 180) * o1.V.Speed;

                            var o2LocalDir = o2.V.Direction - cn;

                            var o2RefXSpeed = Math.Cos(o2LocalDir * Math.PI / 180) * o2.V.Speed;
                            var o2RefYSpeed = Math.Sin(o2LocalDir * Math.PI / 180) * o2.V.Speed;

                            // Exchange the x speeds only, leaving the Y speeds

                            var temp = o2RefXSpeed;
                            o2RefXSpeed = o1RefXSpeed;
                            o1RefXSpeed = temp;

                            // Calculate new vectors for each object after the exchanged speeds

                            var o1NewSpeed = (float)(Math.Sqrt((o1RefXSpeed * o1RefXSpeed) + (o1RefYSpeed * o1RefYSpeed)));
                            var o1NewDirection = (float)(Math.Atan2(o1RefYSpeed, o1RefXSpeed) * 180 / Math.PI);

                            var o2NewSpeed = (float)(Math.Sqrt((o2RefXSpeed * o2RefXSpeed) + (o2RefYSpeed * o2RefYSpeed)));
                            var o2NewDirection = (float)(Math.Atan2(o2RefYSpeed, o2RefXSpeed) * 180 / Math.PI);

                            // "Turn" the vectors back to the original frame

                            var o1GlobalDir = o1NewDirection + cn;
                            var o2GlobalDir = o2NewDirection + cn;

                            o1.V = new Velocity(o1NewSpeed, o1GlobalDir);
                            o2.V = new Velocity(o2NewSpeed, o2GlobalDir);

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
        public void Paint(PaintEventArgs e)
        {
            foreach (PhysicsObject i in MoveableObjects)
            {
                i.Paint(e);
            }
            foreach (PhysicsObject w in FixedObjects)
            {
                w.Paint(e);
            }
        }
    }
}
