using System.Numerics;
using framework;

namespace funkin
{
    enum StrumState
    {
        Static,
        Press,
        Confirm
    }

    class Strum : Sprite
    {
        public int direction = 0;

        public bool scrollUp = true;

        // State images
        public readonly string staticImg = "assets/static.png";
        public readonly string pressImg = "assets/press.png";
        public readonly string confirmImg = "assets/confirm.png";

        public string skin;

        public Strum(int dir = 0, string skin = "default")
            : base()
        {
            this.skin = skin;
            direction = dir;
            staticImg = Assets.GetPath("images/notes/" + skin + "/static.png");
            pressImg = Assets.GetPath("images/notes/" + skin + "/press.png");
            confirmImg = Assets.GetPath("images/notes/" + skin + "/confirm.png");
            scale.X = 0.7f;
            scale.Y = 0.7f;
            SetState(StrumState.Static);
        }

        public void SetState(StrumState state)
        {
            Console.WriteLine(direction);
           
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

            switch (state)
            {
                case StrumState.Static:
                    loadGraphic(staticImg);
                     color.R = 255;
                    color.G = 255;
                    color.B = 255;
                    break;
                case StrumState.Press:
                    loadGraphic(pressImg);
                    break;
                case StrumState.Confirm:
                
                    loadGraphic(confirmImg);
                    break;
            }

        }
    }
}
