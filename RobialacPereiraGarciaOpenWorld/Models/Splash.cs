using Microsoft.Xna.Framework;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;

namespace RobialacPereiraGarciaOpenWorld
{
    public class Splash : Sprite
    {
        private float time = 0f;
        public bool Done { get; private set; } = false;

        public Splash(Game1 game,string name, float width = 0, float height = 0, float scale = 0, bool collides = false) : base(game, name, width, height, scale, collides)
        {
            ForceOrigin(bounds.Size.ToVector2() / 8f);
        }

        public override void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 1)
            {
                time = 0f;
                Done = true;
            }
            

            base.Update(gameTime);
        }


    }
}