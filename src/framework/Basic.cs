using System;
using System.Collections.Generic;
using Raylib_cs;

namespace framework
{
    /// <summary>
    /// Base class for all simple objects with lifecycle flags and a unique ID.
    /// </summary>
    public class Basic
    {
        /// <summary>
        /// Global counter for all Basic instances.
        /// </summary>
        public static int Enumerator = 0;

        public float x = 0f;
        public float y = 0f;
        /// <summary>
        /// Unique ID for this instance.
        /// </summary>
        public int Id = Enumerator++;

        /// <summary>
        /// Whether the object is active (e.g., for updates).
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Whether the object exists.
        /// </summary>
        public bool Exists { get; set; } = true;

        /// <summary>
        /// Whether the object is alive.
        /// </summary>
        public bool Alive { get; set; } = true;

        /// <summary>
        /// Whether the object is visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Cameras used for 2D rendering.
        /// </summary>
        public List<Camera2D> Cameras2D { get; set; } = null;


        /// <summary>
        /// Cameras used for 3D rendering.
        /// </summary>
        public List<Camera3D> Cameras3D { get; set; } = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Basic()
        {
            
        }

        /// <summary>
        /// Marks the object as dead and non-existent.
        /// </summary>
        public void Kill()
        {
            Alive = false;
            Exists = false;
        }

        /// <summary>
        /// Revives the object, making it alive and existing.
        /// </summary>
        public void Revive()
        {
            Alive = true;
            Exists = true;
        }

        /// <summary>
        /// Called every update cycle.
        /// </summary>
        /// <param name="elapsed">Time since last update in seconds.</param>
        public virtual void Update(float elapsed) { }

        /// <summary>
        /// Called every render cycle.
        /// </summary>
        public virtual void Render()
        {
            // 3D cameras first
            foreach (var cam in Cameras3D)
            {
               
                Render3D(); // your custom drawing
              
            }

            // 2D cameras second
            foreach (var cam in Cameras2D)
            {
      
                Render2D(); // your custom drawing
               
            }

            // Optional: fallback render
            if (Cameras2D.Count == 0 && Cameras3D.Count == 0)
            {
                Render2D(); // Default rendering if no camera is used
            }
        }

        /// <summary>
        /// Override for custom 2D rendering.
        /// </summary>
        public virtual void Render2D() { }

        /// <summary>
        /// Override for custom 3D rendering.
        /// </summary>
        public virtual void Render3D() { }

        /// <summary>
        /// Cleans up the object.
        /// </summary>
        public virtual void Destroy()
        {
            Exists = false;
            Alive = false;
            Active = false;
        }

        /// <summary>
        /// Resets the object to a default state.
        /// </summary>
        public virtual void Reset()
        {
            Revive();
            Active = false;
        }

        /// <summary>
        /// Returns a string version of the object.
        /// </summary>
        public override string ToString()
        {
            return $"Basic(Id={Id}, Alive={Alive}, Exists={Exists}, Active={Active})";
        }

        public void setPos(float x = 0f, float y = 0f)
        {
            this.x = x;
            this.y = y;
        } 
    }
}
