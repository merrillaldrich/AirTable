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
                            Console.WriteLine("Smash");

                            // For the simplest, staring case two objects collide
                            // exactly in a line (the x direction) and have the same 
                            // mass. In order to conserve momentum, the momentum of
                            // each of the two colliding objects are exchanged. And because  
                            // mass of the objects in this simple example is the same,
                            // we can just exchange their velocities:

                            float o1Speed = o1.V.Speed;
                            float o1Direction = o1.V.Direction;

                            float o2Speed = o2.V.Speed;
                            float o2Direction = o2.V.Direction;

                            o1.V = new Velocity(o2Speed, o2Direction);
                            o2.V = new Velocity(o1Speed, o1Direction);

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
