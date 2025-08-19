using System;
using System.Collections.Generic;
using Raylib_cs;

namespace framework
{


    /// <summary>
    /// A Group is just a TypedGroup of Basics.
    /// </summary>
    public class Group : TypedGroup<Basic> { }

    /// <summary>
    /// A generic group for managing Basic objects.
    /// </summary>
    /// <typeparam name="T">Type of Basic</typeparam>
    public class TypedGroup<T> : Basic where T : Basic
    {
        /// <summary>
        /// Members of this group.
        /// </summary>
        public List<T> Members { get; private set; } = new List<T>();

        /// <summary>
        /// Number of members in the group.
        /// </summary>
        public int Length => Members.Count;

        /// <summary>
        /// Applies a function to each alive member.
        /// </summary>
        public void ForEachAlive(Action<T> func)
        {
            foreach (var obj in Members)
            {
                if (obj != null && obj.Alive)
                    func(obj);
            }
        }

        /// <summary>
        /// Applies a function to each dead member.
        /// </summary>
        public void ForEachDead(Action<T> func)
        {
            foreach (var obj in Members)
            {
                if (obj != null && !obj.Alive)
                    func(obj);
            }
        }

        /// <summary>
        /// Applies a function to each member.
        /// </summary>
        public void ForEach(Action<T> func)
        {
            foreach (var obj in Members)
            {
                if (obj != null)
                    func(obj);
            }
        }

        /// <summary>
        /// Adds a new member if not already present.
        /// </summary>
        public T Add(T b)
        {
            if (b != null && !Members.Contains(b))
                Members.Add(b);

            return b;
        }

        /// <summary>
        /// Removes a member and destroys it.
        /// </summary>
        public T Remove(T b)
        {
            if (b != null && Members.Contains(b))
            {
                b.Destroy();
                Members.Remove(b);
            }
            return b;
        }

        /// <summary>
        /// Calls render on all alive and visible members.
        /// </summary>
        public override void Render()
        {
            base.Render();

            ForEach(obj =>
            {
                if (obj.Visible && obj.Alive)
                {
                    // Share cameras with parent group
                    obj.Cameras2D = this.Cameras2D;
                    obj.Cameras3D = this.Cameras3D;

                    for (int i = 0; i < obj.Cameras2D.Count; i++)
                    {
                        var cam2d = obj.Cameras2D[i];
                        //cam2d.Zoom = 1.0f; // your desired zoom
                        Raylib.BeginMode2D(cam2d);
                        obj.Render2D();
                        Raylib.EndMode2D();

                        // Write the modified camera back to the list if needed
                        obj.Cameras2D[i] = cam2d;
                    }


                }
            });
        }

        /// <summary>
        /// Updates all alive and existing members.
        /// </summary>
        public override void Update(float elapsed)
        {
            base.Update(elapsed);

            ForEach(obj =>
            {
                if (obj.Alive && obj.Exists)
                    obj.Update(elapsed);
            });
        }
    }
}
