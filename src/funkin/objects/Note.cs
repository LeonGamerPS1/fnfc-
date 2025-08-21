using System.Numerics;
using framework;
using Microsoft.VisualBasic;
using static funkin.ChartParser;

namespace funkin
{


    class Note : Sprite
    {
        public NoteData noteData;
        public bool hit = false;

        public Sustain sustain;
        public string skin;



        // State images
        public readonly string confirmImg = "assets/confirm.png";

        public Note(NoteData noteData, string skin = "default")
            : base()
        {

            this.noteData = noteData;
            confirmImg = Assets.GetPath("images/notes/" + skin + "/note.png");
            scale.X = 0.7f;
            scale.Y = 0.7f;
            this.skin = skin;
            updateGraphic();
        }

        public void updateGraphic()
        {
            int direction = 0;
            if (noteData != null)
                direction = noteData.Data % 4;
            switch (direction % 4)
            {
                case 0:
                    angle = -90;
                    break;
                case 1:
                    angle = 180;
                    break;
                case 2:
                    angle = 0;
                    break;
                case 3:
                    angle = 90;
                    break;
            }
            loadGraphic(confirmImg);
        }

        public void followStrum(Strum myStrum, float speed = 1)
        {
            float distance = (noteData.Time - Conductor.SongPosition) * (0.45f * speed);
            if (!myStrum.scrollUp)
                distance *= -1;
            x = myStrum.x;
            y = myStrum.y + distance;

            if (sustain != null)
            {
                sustain.x = x;
                sustain.y = y;
                sustain.flip = !myStrum.scrollUp;
                sustain.setLength(noteData.Length * 0.45f * speed);

                if (hit)
                {
                    sustain.x = myStrum.x;
                    sustain.y = myStrum.y;

                    sustain.setLength(((noteData.Length - (Conductor.SongPosition - noteData.Time)) * 0.45f * speed));
                }
            }

        }

        public override void Render()
        {
            if (!hit)
                base.Render();
        }
    }


}
