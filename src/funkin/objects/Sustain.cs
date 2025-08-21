using System.Numerics;
using Raylib_cs;
using framework;

namespace funkin
{
    class Sustain : Sprite
    {
        public Sprite tail;
        public float fullLength = 0f;

        public bool flip = false;

        public Sustain(float length = 100f, string bodyPath = "images/notes/default/hold piece.png", string tailPath = "images/notes/default/hold end.png")
            : base(0f, 0f, Assets.GetPath(bodyPath))
        {
            fullLength = length;

            // Stretch body vertically to match sustain length
            scale.Y = 1;
            scale.X = 0.7f;
            alpha = 1f;

            // Tail sprite at the end
            tail = new Sprite(0f, 0f, Assets.GetPath(tailPath));
            tail.origin = new Vector2(tail.graphic.Width / 2f, 0f);
            tail.scale.X = scale.X;
        
            tail.scale.Y = scale.X;
            origin.Y = 0;
            setAntialiasing(true);
            tail.setAntialiasing(true);
        }

        public override void Render2D()
        {
            if (fullLength < 0)
                return;
            tail.alpha = alpha;
            // Draw body
            setLength(fullLength);
            base.Render2D();


            tail.x = x;
            tail.y = y + getHeight();
            if (flip)
            {
                angle = 180;
                tail.y = y - getHeight();
            }
            else
                angle = 0;

             tail.angle = angle;
            tail.color = color;
            tail.Render2D();
        }

        public void setLength(float length)
        {
            fullLength = length;
            frame.Height = length;
           // frame.Y = -length;


            scale.Y = 1;
            scale.X = 0.7f;

            // Tail sprite at the end

            centerOrigin();
            tail.origin.X = tail.graphic.Width / 2f;
            tail.origin.Y = 0;
            tail.scale.X = scale.X;
            tail.scale.Y = scale.X;
            origin.Y = 0;
            setAntialiasing(true);
            tail.setAntialiasing(true);
        }
    }
}
