
using System.Numerics;
using Raylib_cs;

namespace framework
{
    public static class StateManager
    {
        public static State Current { get; private set; } = null;
        public static State Requested { get; private set; } = null;

        public static List<Camera2D> DefaultCameras2D { get; private set; } = new List<Camera2D>();
        public static List<Camera3D> DefaultCameras3D { get; private set; } = new List<Camera3D>();

        /// <summary>
        /// Call this once per frame, after update/draw.
        /// Checks for pending state change and applies it.
        /// </summary>
        public static void SwitchIfRequested()
        {
            if (Requested != null)
            {
                if (Current != null)
                {
                    Current.Destroy();
                }

                ResetDefaultCameras();

                Current = Requested;
                Requested = null;
                Current.Cameras2D = new List<Camera2D>(DefaultCameras2D);
                Current.Cameras3D = new List<Camera3D>(DefaultCameras3D);
                // Copy default cameras into state


                Current.Create();
            }
        }

        /// <summary>
        /// Request a state switch. Will be processed after the current frame.
        /// </summary>
        public static void SwitchState(State next)
        {
            Requested = next;
        }

        public static void Update(float elapsed)
        {
            Current?.Update(elapsed);
        }

        public static void Render()
        {
            Current?.Render();
        }

        public static void ResetDefaultCameras()
        {
            foreach (var camera in DefaultCameras2D)
            {
                Camera2D camera2D = camera;
                DefaultCameras2D.Remove(camera2D);

            }
            // 2D Camera setup
            Camera2D cam = new Camera2D();

            // Set the camera target to world origin
            cam.Target = new Vector2(0, 0);

            // Set the camera offset to the center of the screen
            cam.Offset = new Vector2(0f, 0f);

            // Set zoom and rotation
            cam.Zoom = 1.0f;
            cam.Rotation = 0.0f;

            DefaultCameras2D.Add(cam);


        }
    }
}
