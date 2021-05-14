using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobialacPereiraGarciaOpenWorld.Models
{
    public class Bullets : Sprite
    {
        private Vector2 _origin;
        private Vector2 _direction;
        const float speed = 20f;
        private const float maxDistance = 25f;
        private bool _dead = false;
        public bool Dead => _dead;

        public Bullets(Game1 game, string name, float width = 0, float height = 0, float scale = 0) : base(game, name, width, height, scale, collides: true)
        {

        }

        public void Shoot(Vector2 direction)
        {
            _origin = position;
            _direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            if (collider._inCollision)
            {
                if (collider.collisions.Count != 1 || collider.collisions[0].Tag!= "TratorAzul")
                {
                    _dead = true;
                }
            }
            #region bulletMov
            if ((_origin-position).LengthSquared()>=maxDistance*maxDistance)
            {
                _dead = true;
            }
            else
            {
                SetPosition(position + _direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            #endregion
            base.Update(gameTime);
        }
    }
}
