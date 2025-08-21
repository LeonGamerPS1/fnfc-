using System.Numerics;
using framework;
using Microsoft.VisualBasic;
namespace funkin
{
    class Strumline : Group
    {
        public TypedGroup<Note> notes;

        public bool cpu = true;
        public TypedGroup<Strum> strums;

        public TypedGroup<Sustain> sustains;

        public List<Note> unspawnNotes = [];

        public float speed = 1f;
        public Strumline(Vector2 position, bool scrollUp = true, string skin = "default") : base()
        {


            sustains = new TypedGroup<Sustain>();
            Add(sustains);

            strums = new TypedGroup<Strum>();
            Add(strums);

            notes = new TypedGroup<Note>();
            Add(notes);



            generateStrums(4, position, scrollUp, skin);
        }

        public void generateStrums(int v, Vector2 position, bool scrollUp, string skin)
        {
            for (int i = 0; i < v; i++)
            {
                Strum strum = new Strum(i, skin);
                strum.scrollUp = scrollUp;
                strum.x = position.X + (float)(160 * 0.7 * i) + 50;
                strum.y = position.Y + 50;
                strums.Add(strum);
            }
        }
        public override void Update(float elapsed)
        {
            if (unspawnNotes.Count > 0 && unspawnNotes[0] != null)
            {
                Note note = unspawnNotes[0];
                if (note.noteData.Time <= Conductor.SongPosition + (1500 / speed))
                {
                    notes.Add(note);
                    unspawnNotes.Remove(note);

                    if (note.noteData.Length > 0)
                    {
                        Sustain sustain = new Sustain(20, "images/notes/" + note.skin + "/hold piece.png", "images/notes/" + note.skin + "/hold end.png");
                        sustains.Add(sustain);
                        note.sustain = sustain;
                        sustain.x = -1000;
                    }
                }
            }
            notes.ForEachAlive(note =>
           {
               Strum strum = strums.Members[note.noteData.Data % 4];

               note.followStrum(strum, speed);

               if (note.noteData.Time <= Conductor.SongPosition && !note.hit)
               {
                   note.hit = true;
                   strum.SetState(StrumState.Confirm);

               }

               if (note.hit && note.noteData.Time + note.noteData.Length <= Conductor.SongPosition)
               {
                   notesToDelete.Add(note);
                   strum.SetState(StrumState.Static);
               }
           });

            base.Update(elapsed);

            while (notesToDelete.Count > 0)
            {
                var note = notesToDelete[0];
                note.Kill();
                notes.Remove(note);
                note.Destroy();
                notesToDelete.Remove(note);

                if (note.sustain != null)
                {
                    note.sustain.Kill();
                    sustains.Remove(note.sustain);
                    note.sustain.Destroy();
                }
            }

        }

        List<Note> notesToDelete = [];
    }



}