using Raylib_cs;

namespace framework
{
    class Audio : Basic
    {
        private Music music;

        public bool Persist { get; set; }

        public bool Playing => Raylib.IsMusicStreamPlaying(music);

        // Current playback time in seconds
        public float Time => Raylib.GetMusicTimePlayed(music);

        private float pitch = 1.0f;
        public float Pitch
        {
            get => pitch;
            set
            {
                pitch = value;
                Raylib.SetMusicPitch(music, pitch);
            }
        }

        private float volume = 1.0f;
        public float Volume
        {
            get => volume;
            set
            {
                volume = value;
                Raylib.SetMusicVolume(music, volume);
            }
        }

        private float pan = 0.5f;
        public float Pan
        {
            get => pan;
            set
            {
                pan = value;
                Raylib.SetMusicPan(music, pan);
            }
        }

        public Audio LoadEmbedded(string embeddedSound)
        {
            Raylib.UnloadMusicStream(music);
            music = Raylib.LoadMusicStream(embeddedSound);
            return this;
        }

        public void Play() => Raylib.PlayMusicStream(music);

        public void Stop() => Raylib.StopMusicStream(music);

        public void Resume() => Raylib.ResumeMusicStream(music);

        public void Pause() => Raylib.PauseMusicStream(music);

        public override void Destroy()
        {
            Raylib.UnloadMusicStream(music);
            base.Destroy();
        }

        public override void Update(float elapsed)
        {
            base.Update(elapsed);
            Raylib.UpdateMusicStream(music);
        }
    }
}
