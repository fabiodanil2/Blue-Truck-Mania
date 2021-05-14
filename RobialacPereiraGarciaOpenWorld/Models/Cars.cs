using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RobialacPereiraGarciaOpenWorld.Models.Colliders;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobialacPereiraGarciaOpenWorld.Models
{
    class Cars : Sprite
    {
        // variables 
        public Vector2 _position; // actual position
        Vector2 origin; // initial position
        Vector2 acceleration; // acceleration of all the cars
        int life=10;
        private Vector2 direction = Vector2.UnitY;
        private SoundEffect tomatoSplash;


        public Cars(Game1 game, string name, float width = 0, float height = 0, float scale = 0, bool collides = false) : base(game, name, width, height, scale, collides)
        {
            _position = origin;
            tomatoSplash = game.Content.Load<SoundEffect>("Splash ");
        }


        public override void Update(GameTime gameTime)
        {
            //remove o carro
            if (life <= 0f)
            {
                _game.Scene.Remove(this);
                _game._player.points += 200;
                
            }
            // constant acceleration of all the cars
            acceleration.X = 0.3f;

            position.X += acceleration.X;

            SetPosition(position);

            if (collider._inCollision)
            {
                bool fenceCollision = false; // verdadeiro se colide com a vedação

                foreach (Collider c in collider.collisions)
                {
                    // colisão com a vedação
                    if (c.Tag == "CercaGrande" || c.Tag == "cerca")
                    {
                        fenceCollision = true;
                    }

                    if (c.Tag == "tomato") // colisão com o tomate
                    {
                        life -= 5;
                        //muda a cor do carro
                        _color = new Color(1f,
                           MathHelper.Lerp(1, 0, (10 - life) / 10f),
                           MathHelper.Lerp(1, 0, (10 - life) / 10f));
                        _game.splashes.Add(new Splash(_game, "tomate2", height: 2).SetPosition(position) as Splash);
                        tomatoSplash.Play();
                    }
                }

                    

                if (fenceCollision == true)
                {
                    // inverte o sentido
                    acceleration.X = acceleration.X * -1;

                    SetPosition(new Vector2(-position.X, position.Y));

                }
                
                // colisão passa a false, o que prepara o carro para a próxima colisão
                fenceCollision = false;

            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }
    }
}
