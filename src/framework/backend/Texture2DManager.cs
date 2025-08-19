using Raylib_cs;

namespace framework;

class Texture2DManager
{
    public static StringMap<Texture2D> cache = new StringMap<Texture2D>();


    public static void clearCache()
    {
        foreach (var textureKey in cache)
        {
            Raylib.UnloadTexture(textureKey.Value);
            cache.Remove(textureKey.Key);
        }
    }

    public static Texture2D GetImage(string ImagePath = "assets/default.png")
    {
        if (cache.Exists(ImagePath))
            return cache.Get(ImagePath);

        if (File.Exists(ImagePath))
        {
            Texture2D texture = Raylib.LoadTexture(ImagePath);
            cache.Set(ImagePath, texture);
            return texture;
        }
        return GetImage("assets/fallback.png");

    }
    


}