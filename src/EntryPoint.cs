
using Raylib_cs;
using funkin;
using framework;



class EntryPoint
{
    // STAThread is required if you deploy using NativeAOT on Windows - See https://github.com/raylib-cs/raylib-cs/issues/301
    [STAThread]
    public static void Main()
    {
        Raylib.InitWindow(1280, 720, "Night Engine");
        Raylib.SetTargetFPS(200);
        Raylib.InitAudioDevice();

        StateManager.SwitchState(new PlayState());

        while (!Raylib.WindowShouldClose())
        {
            StateManager.SwitchIfRequested();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            StateManager.Update(Raylib.GetFrameTime());
            StateManager.Render();
            Raylib.DrawFPS(10, 10);
            Raylib.EndDrawing();
        }


        Texture2DManager.clearCache();
        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }
}



