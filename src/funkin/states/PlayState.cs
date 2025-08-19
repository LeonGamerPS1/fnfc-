using framework;
using Raylib_cs;
namespace funkin;

class PlayState : State
{
        Sprite sprite = new Sprite(200, 200,"assets/images/sustains.png");
    public override void Create()
    {
        sprite.scale.X = 0.7f;
        sprite.scale.Y = 0.7f;
        Add(sprite);
    }

    float sustainHeight = 0f;
    public override void Update(float elapsed)
    {
        base.Update(elapsed);
        sprite.angle += 0.1f;
     
 
    }
}