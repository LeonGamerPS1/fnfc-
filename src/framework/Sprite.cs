using System.Numerics;
using Raylib_cs;

namespace framework
{
    class Sprite : Basic
    {
        public Texture2D graphic;
        public Vector2 origin;
        public Vector2 scale;
        public Vector2 offset;

        public Rectangle frame; // the portion of the texture to draw

        public Color color = Color.White;
        public float angle = 0f;



        public Sprite(float x = 0f, float y = 0f, string imgPath = "assets/default.png") : base()
        {
            setPos(x, y);
            loadGraphic(imgPath);

            origin = Vector2.Zero;
            offset = Vector2.Zero;
            scale = Vector2.One;

            // default frame is full texture
            frame = new Rectangle(0f, 0f, graphic.Width, graphic.Height);
        }

        public void loadGraphic(string imgPath)
        {
            graphic = Texture2DManager.GetImage(imgPath);

            // default origin is center of the frame
            origin.X = graphic.Width / 2;
            origin.Y = graphic.Height / 2;

            // set default frame to full texture
            frame = new Rectangle(0f, 0f, graphic.Width, graphic.Height);
        }

        Rectangle source = new Rectangle();
        Rectangle dest = new Rectangle();
        public override void Render2D()
        {

            // Use the frame for source rectangle

            source.X = frame.X;
            source.Y = frame.Y;
            source.Width = frame.Width;
            source.Height = frame.Height;


            // Destination rectangle scales frame size
            dest.X = x - offset.X;
            dest.Y = y - offset.Y;
            dest.Width = frame.Width * scale.X;

            dest.Height = frame.Height * scale.Y;


            Raylib.DrawTexturePro(graphic, source, dest, origin, angle, color);
        }

        // Optional helper to set frame by pixel coordinates
        public void setFrame(float x, float y, float width, float height)
        {
            frame.X = x;
            frame.Y = y;
            frame.Width = width;
            frame.Height = height;

        }
    }
}
