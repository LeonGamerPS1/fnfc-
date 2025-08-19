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

        public Rectangle frame; // portion of the texture to draw
        public Color color = Color.White;
        public float angle = 0f;

        public bool antialiasing = false; // Flixel-style toggle

        Rectangle source = new Rectangle();
        Rectangle dest = new Rectangle();

        public Sprite(float x = 0f, float y = 0f, string imgPath = "assets/default.png", bool antialias = true) : base()
        {
            setPos(x, y);

            origin = Vector2.Zero;
            offset = Vector2.Zero;
            scale = Vector2.One;

            antialiasing = antialias;
            loadGraphic(imgPath);
        }

        public void loadGraphic(string imgPath)
        {
            graphic = Texture2DManager.GetImage(imgPath);

            // Apply smoothing based on antialiasing flag
            Raylib.SetTextureFilter(graphic, antialiasing ? TextureFilter.Bilinear : TextureFilter.Point);

            // Full texture by default
            frame = new Rectangle(0f, 0f, graphic.Width, graphic.Height);

            centerOrigin();
            offset = Vector2.Zero;
        }

        public override void Render2D()
        {
            // Source rectangle
            source.X = frame.X;
            source.Y = frame.Y;
            source.Width = frame.Width;
            source.Height = frame.Height;

            // Destination rectangle
            dest.X = x - offset.X;
            dest.Y = y - offset.Y;
            dest.Width = frame.Width * scale.X;
            dest.Height = frame.Height * scale.Y;

            // Scaled origin
            Vector2 scaledOrigin = new Vector2(origin.X * scale.X, origin.Y * scale.Y);

            Raylib.DrawTexturePro(graphic, source, dest, scaledOrigin, angle, color);
        }

        public void setFrame(float x, float y, float width, float height)
        {
            frame.X = x;
            frame.Y = y;
            frame.Width = width;
            frame.Height = height;
            centerOrigin();
        }

        public void centerOrigin()
        {
            origin.X = frame.Width / 2f;
            origin.Y = frame.Height / 2f;
        }

        // Allow changing at runtime
        public void setAntialiasing(bool enable)
        {
            antialiasing = enable;
         
                Raylib.SetTextureFilter(graphic, antialiasing ? TextureFilter.Bilinear : TextureFilter.Point);
        }
    }
}
