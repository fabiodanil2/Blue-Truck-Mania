using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RobialacPereiraGarciaOpenWorld.Models;
using RobialacPereiraGarciaOpenWorld.Models.CameraModel;
using RobialacPereiraGarciaOpenWorld.Models.Colliders;
using RobialacPereiraGarciaOpenWorld.Models.KeyboardModel;
using RobialacPereiraGarciaOpenWorld.Models.Scenes;
using RobialacPereiraGarciaOpenWorld.Models.SpriteModel;
using System;
using System.Collections.Generic;

namespace RobialacPereiraGarciaOpenWorld
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteManager spriteManager;
        private Camera camera;
        public CollisionManager cManager;
        public BackGround _background;

        private Scene _scene;
        public Scene Scene => _scene;
        public Player _player;
        public List<Splash> splashes;
        public Song backGroundMusic;


        #region Propriedades
        public SpriteManager SpriteManager
        {
            get
            {
                return spriteManager;
            }
        }

        public GraphicsDeviceManager Graphics
        {
            get
            {
                return graphics;
            }
        }

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        public object Splash { get; internal set; }
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 900;
            graphics.ApplyChanges();

            splashes = new List<Splash>();
            Components.Add(new KeyboardManager(this));
            spriteManager = new SpriteManager(this);
            camera = new Camera(this, worldWidth: 50f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            cManager = new CollisionManager();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteManager.AddSpriteSheet("sprites");

            _scene = new Scene(this, "MainScene");
            _player = _scene.Player;

            _background = new BackGround(this, "Grass", "Asphalt", new Vector2(5));
            backGroundMusic = Content.Load<Song>("Song");
            MediaPlayer.Play(backGroundMusic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _scene.Update(gameTime);
            _player.Update(gameTime);
            cManager.Update(gameTime);
            _player.LateUpdate(gameTime);

            foreach (Splash s in splashes.ToArray())
            {
                s.Update(gameTime);
                if (s.Done)
                {
                    splashes.Remove(s);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _background.Draw(gameTime);
            _scene.Draw(gameTime);

            spriteBatch.Begin();
            _player.Draw(spriteBatch);
            foreach (Splash s in splashes)
            {
                s.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
