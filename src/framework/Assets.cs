using System.IO;

namespace framework
{
    public static class Assets
    {
        /// <summary>
        /// If not "null", paths start with mods/modname/... 
        /// Otherwise, they start with res/...
        /// </summary>
        public static string? CurrentMod { get; set; } = null;

        /// <summary>
        /// Resolve path depending on whether mod exists.
        /// </summary>
        public static string GetPath(string path)
        {
            if (CurrentMod != null)
            {
                string modPath = Path.Combine("mods", CurrentMod, path);

                if (File.Exists(modPath))
                    return modPath;
            }
            return Path.Combine("res", path);
        }

        public static string Font(string s)
        {
            return GetPath(Path.Combine("fonts", s));
        }

        public static string Image(string s)
        {
            return GetPath(Path.Combine("images", $"{s}.png"));
        }

        public static string Sound(string s)
        {
            return GetPath(Path.Combine("sounds", $"{s}.ogg"));
        }

        public static string Sparrow(string s)
        {
            return GetPath(Path.Combine("images", $"{s}.xml"));
        }

        public static string Music(string s)
        {
            return GetPath(Path.Combine("music", $"{s}.ogg"));
        }

        public static string Txt(string s)
        {
            return GetPath(Path.Combine("data", $"{s}.txt"));
        }

        public static bool Exists(string file)
        {
            return File.Exists(GetPath(file));
        }

        public static string GetText(string path)
        {
            return File.ReadAllText(GetPath(path));
        }
    }
}
