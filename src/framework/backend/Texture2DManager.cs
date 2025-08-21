using Raylib_cs;

namespace framework;

class Texture2DManager
{


    public static void clearCache()
    {
        // unused func
    }

    public static Texture2D GetImage(string ImagePath = "assets/default.png")
    {


        if (File.Exists(ImagePath))
        {
            Texture2D texture = Raylib.LoadTexture(ImagePath);

            return texture;
        }
        return GetImage("assets/fallback.png");

    }



}