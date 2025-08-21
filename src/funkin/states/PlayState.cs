
using System.Numerics;
using framework;
using Microsoft.VisualBasic;
using Raylib_cs;
using static funkin.ChartParser;
namespace funkin;

class PlayState : State
{
    public static ChartFile song;
    public static string Difficulty = "Easy";

    public Strumline? opponentStrumline;

    public Strumline? playerStrumline;

    public Group ui = new Group();

    public Camera2D camHUD = new Camera2D();
    private float gameCamZoom = 1f;
    private float gameHudZoom = 1f;

    public Audio inst;

    public bool upScroll = true;


    public override void Create()
    {
        song = ChartParser.ParseFromFile(Assets.GetPath("songs/stress/stress-chart.json"));
        camHUD.Zoom = 1f;
        ui.Cameras2D = [camHUD];
        opponentStrumline = new Strumline(new Vector2(50, upScroll ? 50 : 720 - 150), upScroll);
        ui.Add(opponentStrumline);

        playerStrumline = new Strumline(new Vector2(1280 / 2 + (100), upScroll ? 50 : 720 - 150), upScroll);
        ui.Add(playerStrumline);

        inst = new Audio();
        inst.LoadEmbedded(Assets.GetPath("songs/stress/Inst.ogg"));




        Conductor.SetBPM(100);
        Conductor.UpdatePosition(-(3000.4232434343f));
        Conductor.OnMeasure += measureHit;
        generateNotes();

        startCountdown();
        Console.WriteLine(ChartHelpers.GetSongName(Assets.GetPath("songs/stress/stress-chart.json"), song));
    }

    private void startCountdown()
    {
        startedCountdown = true;

    }

    public bool startedSong = false;
    public bool startedCountdown = false;
    private void generateNotes()
    {

        var noteArray = song.Notes.Hard;

        foreach (var noteData in noteArray)
        {
            Strumline? strumline = noteData.Data > 3 ? opponentStrumline : playerStrumline;
            strumline.speed = song.ScrollSpeed.Hard;
            Note note = new Note(noteData, strumline.strums.Members[noteData.Data % 4].skin);
            strumline.unspawnNotes.Add(note);


        }
    }

    public float defaultGameZoom = 1f;
    public override void Update(float elapsed)
    {
        gameCamZoom = Lerp(defaultGameZoom, gameCamZoom, (float)Math.Exp(-elapsed * 8f));
        gameHudZoom = Lerp(Raylib.GetRenderWidth() / 1280f, gameHudZoom, (float)Math.Exp(-elapsed * 8f));

        ui.Update(elapsed);
        base.Update(elapsed);


        var cam = Cameras2D[0];

        cam.Zoom = gameCamZoom;
        Cameras2D[0] = cam;
        if (startedCountdown && !startedSong)
        {
            Conductor.UpdatePosition(Conductor.SongPosition + (elapsed) * 1000);
            if (Conductor.SongPosition > 0)
                startSong();
        }
        if (inst.Playing)
            Conductor.UpdatePosition(inst.Time * 1000);

    }

    public void startSong()
    {
        startedSong = true;
        inst.Play();
        Add(inst);
    }
    public void measureHit(float measure)
    {
        gameHudZoom += 0.015f;
        gameCamZoom += 0.05f;
        Console.WriteLine("poo");
    }
    public override void Render()
    {
        camHUD.Zoom = gameHudZoom;
        ui.Cameras2D = [camHUD];

        base.Render();
        Raylib.BeginMode2D(camHUD);
        ui.Render();
        Raylib.EndMode2D();
    }


    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

}