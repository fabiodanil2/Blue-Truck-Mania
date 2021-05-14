using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RobialacPereiraGarciaOpenWorld.Models.Scenes
{
    public class Scene
    {
        private string _name;
        private List<Sprite> _sprites;
        private Game1 _game;
        const float Deg2Rag = (float)Math.PI / 180f;
        private SpriteBatch _spriteBatch;
        public Player Player { get; }

        public Scene(Game1 game, string sceneFile)
        {
            _game = game;
            _sprites = new List<Sprite>();
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);

            JObject json = JObject.Parse(File.ReadAllText($"Content/RobialacPereiraGarciaOpenWorld/scenes/{sceneFile}.dt"));
            _name = json["sceneName"].Value<string>();
            Console.WriteLine(_name);

            foreach (JToken image in json["composite"]["sImages"])
            {
                string imageName = image["imageName"].Value<string>();
                float x = image["x"]?.Value<float>() ?? 0f;
                float y = image["y"]?.Value<float>() ?? 0f;
                float rot = Deg2Rag * (image["rotation"]?.Value<float>() ?? 0f);
                float scale = image["scaleX"]?.Value<float>() ?? 1f;

                if (image["itemIdentifier"]?.Value<string>() == "Player")
                {
                    Player = new Player(_game, imageName, scale: scale);
                    Player.SetPosition(new Vector2(x, y));
                    Player.SetRotation(-rot);
                }
                else if (image["itemIdentifier"]?.Value<string>() == "tomate")
                {
                    bool collides = (image["tags"]?.Count(el => el.Value<string>() == "tomate") ?? 0) > 0;
                    Tomate tomate = new Tomate(_game, imageName, scale: scale, collides: true);
                    tomate.SetPosition(new Vector2(x, y));
                    tomate.SetRotation(-rot);
                    _sprites.Add(tomate);
                }
                else if (image["itemIdentifier"]?.Value<string>() == "coin")
                {
                    bool collides = (image["tags"]?.Count(el => el.Value<string>() == "coin") ?? 0) > 0;
                    Coin coin = new Coin(_game, imageName, scale: scale, collides: true);
                    coin.SetPosition(new Vector2(x, y));
                    coin.SetRotation(-rot);
                    _sprites.Add(coin);
                }
                else if (image["itemIdentifier"]?.Value<string>() == "tomate2")
                {
                    bool collides = (image["tags"]?.Count(el => el.Value<string>() == "tomate2") ?? 0) > 0;
                    Splash splash = new Splash(_game, imageName, scale: scale, collides: collides);
                    splash.SetPosition(new Vector2(x, y));
                    splash.SetRotation(-rot);
                }
                else if (image["itemIdentifier"]?.Value<string>() == "carro")
                {
                    bool collides = (image["tags"]?.Count(el => el.Value<string>() == "collider") ?? 0) > 0;

                    Cars sprite = new Cars(_game, imageName, scale: scale, collides: collides);
                    sprite.SetPosition(new Vector2(x, y));
                    sprite.SetRotation(-rot);

                    _sprites.Add(sprite);
                }
                else
                {
                    bool collides = (image["tags"]?.Count(el => el.Value<string>() == "collider") ?? 0) > 0;

                    Sprite sprite = new Sprite(_game, imageName, scale: scale, collides: collides);
                    sprite.SetPosition(new Vector2(x, y));
                    sprite.SetRotation(-rot);

                    _sprites.Add(sprite);
                    _sprites.Add(sprite);
                }

            }
        }

        public void Remove(Sprite sprite)
        {
            if (sprite.collider != null)
            {
                _game.cManager.Remove(sprite.collider);
            }
            _sprites.Remove(sprite);
        }

        public void Update(GameTime gameTime)
        {
            foreach(Sprite sprite in _sprites.ToArray())
            {
                sprite.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }
    }
}
