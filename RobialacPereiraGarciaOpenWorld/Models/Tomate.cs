using Microsoft.Xna.Framework;
using RobialacPereiraGarciaOpenWorld.Models.Colliders;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobialacPereiraGarciaOpenWorld.Models
{
    public class Tomate : Sprite
    {
        public Tomate(Game1 game, string name, float width = 0, float height = 0, float scale = 0, bool collides = false)
                                : base(game, name, width, height, scale, collides)
        {
        }

        public override void Update(GameTime gameTime)
        {

            if (collider._inCollision)
            {
                foreach (Collider c in collider.collisions)
                {
                    if (c.Tag == "TratorAzul")
                    {
                        _game.Scene.Remove(this);
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
