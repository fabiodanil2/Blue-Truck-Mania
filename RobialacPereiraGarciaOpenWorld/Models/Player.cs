using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RobialacPereiraGarciaOpenWorld.Models.CameraModel;
using RobialacPereiraGarciaOpenWorld.Models.Colliders;
using RobialacPereiraGarciaOpenWorld.Models.KeyboardModel;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace RobialacPereiraGarciaOpenWorld.Models
{
    public class Player : Sprite
    {
        // variaveis
        private float accelaration = 0f;
        private Vector2 direction = Vector2.UnitY;

        private float oldRotation;
        private Vector2 oldPosition;
        private Vector2 Origin;

        private float ZOOM = 1.0f;
        private int LastMouseWheel;


        private List<Sprite> tomatos;
        private int numberOfTomatoes = 2;
        public int lives = 3;
        public int points = 0;
        int highScore = 0;

        string High_Score;
        string A;
        SpriteFont arial;
        Song SongE;
        SoundEffect CoinS;
        SoundEffect Dead;

        // referência ao game
        Game1 game;

        // variaveis

        public Player(Game1 game, string spriteName, float scale = 1f) : base(game, spriteName, scale: scale, collides: true)
        {
            tomatos = new List<Sprite>();
            Origin = new Vector2(0, 0);
            this.game = game;
            arial = game.Content.Load<SpriteFont>("Arial");
            SongE = game.Content.Load<Song>("End");
            CoinS = game.Content.Load<SoundEffect>("CoinS");
            Dead = game.Content.Load<SoundEffect>("ErrorS");
        }

        public override void LateUpdate(GameTime gameTime)
        {
            if (collider._inCollision)
            {
                bool extraCollision = false;// verdadeiro se colide com algo que nao é um tomate
                foreach (Collider c in collider.collisions)
                {
                    if (c.Tag != "tomato" && c.Tag != "kindpng1378742" && c.Tag != "cerca" && c.Tag != "tomate" && c.Tag != "CercaGrande" && c.Tag != "cowwalksingle" &&
                        c.Tag != "coweatsingle" &&
                        c.Tag != "sheepwalksingle" &&
                        c.Tag != "pigwalksingle" &&
                        c.Tag != "chickeneatsingle" &&
                        c.Tag != "sheepeatsingle")
                    {
                        extraCollision = true;
                    }
                    if (
                        c.Tag == "CercaGrande" ||
                        c.Tag == "cowwalksingle" ||
                        c.Tag == "coweatsingle" ||
                        c.Tag == "sheepwalksingle" ||
                        c.Tag == "pigwalksingle" ||
                        c.Tag == "chickeneatsingle" ||
                        c.Tag == "sheepeatsingle")
                    {
                        SetPosition(oldPosition);
                        SetRotation(oldRotation);
                    }
                    if (c.Tag == "tomate")
                    {
                        numberOfTomatoes++;
                    }
                    if (c.Tag == "kindpng1378742")
                    {
                        points += (int)208.9f;
                        CoinS.Play();
                    }

                }

                if (extraCollision)
                {
                    SetPosition(Origin);
                    lives--;
                    Dead.Play();
                    if(lives<=0)
                        MediaPlayer.Play(SongE);

                }
            }
        }

        public override void Update(GameTime gameTime)
        {

            foreach (Bullets tomato in tomatos.ToArray())
            {
                tomato.Update(gameTime);
                if (tomato.Dead)
                {
                    _game.cManager.Remove(tomato.collider);
                    tomatos.Remove(tomato);
                }
            }

            oldPosition = position;
            oldRotation = rotation;

            #region Aceleracao

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (accelaration >= -0.0001f && KeyboardManager.IsKeyDown(Keys.W))
            {
                // Acelerar
                accelaration += 4f * deltaTime;
            }

            if (accelaration <= +0.001f && KeyboardManager.IsKeyDown(Keys.S))
            {
                // andar para trás
                accelaration -= 2f * deltaTime;
            }

            if (KeyboardManager.IsKeyDown(Keys.Space))
            {
                // travar
                accelaration *= 0.15f * deltaTime;
            }

            accelaration *= 0.9f;
            SetPosition(position + accelaration * direction);

            #endregion

            #region Lançar Tomates
            if (KeyboardManager.IsKeyGoingDown(Keys.Space))
            {
                if (numberOfTomatoes > 0)
                {

                    Vector2 pos = position + direction * size.Y / 1.5f;
                    Bullets tomato = new Bullets(_game, "tomato", width: 2f);
                    tomato.SetPosition(pos);
                    tomato.Shoot(direction);
                    tomatos.Add(tomato);
                    // numero de tomates disponiveis diminui
                    numberOfTomatoes--;

                }
            }
            #endregion

            #region Rotacao

            if (Math.Abs(accelaration) > 0.001f)
            {
                if (KeyboardManager.IsKeyDown(Keys.A)) rotation -= ((float)Math.PI / 2f) * deltaTime;
                if (KeyboardManager.IsKeyDown(Keys.D)) rotation += ((float)Math.PI / 2f) * deltaTime;
                direction = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));
                SetRotation(rotation);
            }

            #endregion

            //ZOOM
            int currentMouseWheel = Mouse.GetState().ScrollWheelValue;
            float delta = -(currentMouseWheel - LastMouseWheel) / 1200f;
            Camera.Zoom(1.0f + delta);
            LastMouseWheel = currentMouseWheel;

            #region HighScore

            // Cria um ficheiro onde guarda o numero total de pontos do jogo passado se este for maior que o valor que la ta
            //if (points > highScore)
            //{
            //    StreamWriter sw = new StreamWriter("HighScore.txt");
            //    sw.WriteLine(points);
            //    sw.Close();

            //}

            //StreamReader sr = new StreamReader("HighScore.txt");
            //A = sr.ReadLine();
            //sr.Close();
            //highScore = Int32.Parse(A);
            #endregion

            // condição de fim do jogo
            if (lives <= 0)
            {
                foreach (GameComponent comp in game.Components)
                {
                    comp.Enabled = false;
                }

                lives = 0;
            }


            // Check Camera Position
            Camera.LookAt(position);
            // Cria um ficheiro onde guarda o numero total de pontos do jogo passado se este for maior que o valor que la ta
            if (points > highScore)
            {
                StreamWriter sw = new StreamWriter("Content" + "/HighScore.txt");
                sw.WriteLine(points);
                sw.Close();

            }

            // HighScore
            StreamReader sr = new StreamReader("Content" + "/HighScore.txt");
            A = sr.ReadLine();
            sr.Close();
            highScore = Int32.Parse(A);


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite t in tomatos)
            {
                t.Draw(spriteBatch);
            }

            //pontuação
            string Points = string.Format("Score: {0} Points", points);
            Vector2 dim = arial.MeasureString(Points);
            spriteBatch.DrawString(arial, Points, new Vector2((dim.X) / (float)30, 50), Color.White);

            //Vida
            string Vidas = string.Format("Lives: {0}", lives);
            dim = arial.MeasureString(Vidas);
            spriteBatch.DrawString(arial, Vidas, new Vector2((dim.X) / (float)30, 75), Color.White);

            //Tomatos
            string Tomatos = string.Format("Tomatos: {0} Tomatos", numberOfTomatoes);
            dim = arial.MeasureString(Tomatos);
            spriteBatch.DrawString(arial, Tomatos, new Vector2((dim.X)+30, 75), Color.White);

            //pontuacao maxima
            High_Score = string.Format("HighScore: {0} Points", highScore);
            dim = arial.MeasureString(High_Score);
            spriteBatch.DrawString(arial, High_Score, new Vector2((dim.X), 50), Color.White);


            // fim do jogo
            if (lives <= 0)
            {   
                string message = "GAME OVER!";
                game.GraphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(arial, message, new Vector2(dim.X + 260, 180), Color.CadetBlue);                   
            }


            base.Draw(spriteBatch);
        }


    }
}