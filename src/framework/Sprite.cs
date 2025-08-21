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
        public Shader shader;      // Optional shader

        Rectangle source = new Rectangle();
        Rectangle dest = new Rectangle();
        public float alpha = 1;

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
            if (Raylib.IsTextureValid(graphic))
                Raylib.UnloadTexture(graphic);
            graphic = Texture2DManager.GetImage(imgPath);

            // Apply smoothing based on antialiasing flag
            Raylib.SetTextureFilter(graphic, antialiasing ? TextureFilter.Bilinear : TextureFilter.Point);

            // Full texture by default
            frame = new Rectangle(0f, 0f, graphic.Width, graphic.Height);

            centerOrigin();
           
        }

        public override void Render2D()
        {
            // Source rectangle
           color.A = (byte)(255 * alpha);

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

            if (Raylib.IsShaderValid(shader))
                Raylib.BeginShaderMode(shader);


            Raylib.DrawTexturePro(graphic, source, dest, scaledOrigin, angle, color);
            if (Raylib.IsShaderValid(shader))
                Raylib.EndShaderMode();

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

        public bool IsOnScreen(Camera2D cam)
        {
            float camLeft = cam.Target.X - (Raylib.GetScreenWidth() * 0.5f) / cam.Zoom;
            float camTop = cam.Target.Y - (Raylib.GetScreenHeight() * 0.5f) / cam.Zoom;
            float camRight = camLeft + (Raylib.GetScreenWidth() / cam.Zoom);
            float camBottom = camTop + (Raylib.GetScreenHeight() / cam.Zoom);

            float destX = x - offset.X;
            float destY = y - offset.Y;
            float destW = frame.Width * scale.X;
            float destH = frame.Height * scale.Y;

            float scaledOriginX = origin.X * scale.X;
            float scaledOriginY = origin.Y * scale.Y;

            float spriteLeft = destX - scaledOriginX;
            float spriteTop = destY - scaledOriginY;
            float spriteRight = spriteLeft + destW;
            float spriteBottom = spriteTop + destH;

            return !(spriteRight < camLeft ||
                     spriteBottom < camTop ||
                     spriteLeft > camRight ||
                     spriteTop > camBottom);
        }

        public virtual float getWidth() => frame.Width * scale.X;
        public virtual float getHeight() => frame.Height * scale.Y;

        public void setAntialiasing(bool enable)
        {
            antialiasing = enable;
            Raylib.SetTextureFilter(graphic, antialiasing ? TextureFilter.Bilinear : TextureFilter.Point);
        }

        /// <summary>
        /// Assign a shader to the sprite. Pass null to remove it.
        /// </summary>
        public void setShader(Shader s)
        {
            shader = s;
        }

        public override void Destroy()
        {
            if (Raylib.IsTextureValid(graphic))
                Raylib.UnloadTexture(graphic);
        }
    }
}
