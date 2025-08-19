using framework;
using Raylib_cs;
namespace funkin;

class PlayState : State
{
        Sprite sprite = new Sprite(200, 200,"assets/images/sustains.png");
    public override void Create()
    {

        Add(sprite);
    }

    float sustainHeight = 0f;
    public override void Update(float elapsed)
    {
        base.Update(elapsed);
        sustainHeight += 0.2f;
        sprite.setFrame(sprite.graphic.Width / 8, 0f, sprite.graphic.Width, sustainHeight);
    }
}