using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game0
{
    public class Game1 : Game
    {
        private const int GAME_WIDTH = 320;
        private const int GAME_HEIGHT = 180;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private AnimatedSprite _bgBushSprite;
        private AnimatedSprite _bgTreeSprite;
        private AnimatedSprite _grassSprite;
        private RenderTarget2D _lightTarget;
        private Texture2D _circleTexture;
        private Texture2D _bannerTexture;
        private Random _random;
        private float _cameraY;
        private SpriteFont _gidole;

        private List<Entity> _entities;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 720;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            _lightTarget = new RenderTarget2D(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _random = new Random();

            _entities = new List<Entity>();

            // Add fireflies
            for(int i = 0; i < 10; i++)
            {
                _entities.Add(new Firefly(_random) { Position = new Vector2((float)_random.NextDouble() * 320, (float)_random.NextDouble() * 160) });
            }

            _entities.Add(new Player());
            _entities.Add(new Fire(_random));

            _cameraY = 0.0f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _circleTexture = Content.Load<Texture2D>("Sprites/Circle");
            _bannerTexture = Content.Load<Texture2D>("Sprites/Banner");
            
            _bgBushSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/BGBush"), 320, 180, 0.00f) { Position = new Vector2(0, 0), Paused = true };
            _bgTreeSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/BGTrees"), 320, 180, 0.00f) { Position = new Vector2(0, 0), Paused = true };
            _grassSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/Grass"), 320, 42, 0.00f) { Position = new Vector2(0, 180-21), Paused = true };

            foreach (Entity e in _entities) e.LoadContent(Content);

            _gidole = Content.Load<SpriteFont>("Gidole");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            foreach (Entity e in _entities) e.Update(gameTime);

            _cameraY = (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds / 2.0f) * 4f);

            _bgBushSprite.Position = new Vector2(0, _cameraY / 4);
            _bgTreeSprite.Position = new Vector2(0, _cameraY / 1.25f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var mat = Matrix.CreateTranslation(new Vector3(0, -_cameraY, 0)) * Matrix.CreateScale(_graphics.PreferredBackBufferWidth / (float)GAME_WIDTH);

            #region Light Sources

            // Render light sources to render target
            GraphicsDevice.SetRenderTarget(_lightTarget);
            GraphicsDevice.Clear(Color.FromNonPremultiplied(0, 0, 0, 175));

            // BlendState come's from Jjagg
            // https://community.monogame.net/t/how-to-make-lightsources-torch-fire-campfire-etc-in-dark-area-2d-pixel-game/8058/17
            var blendState = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One
            };

            _spriteBatch.Begin(SpriteSortMode.Deferred, blendState, SamplerState.PointClamp, null, null, null, mat);
            foreach (Entity e in _entities) e.DrawLight(gameTime, _spriteBatch, _circleTexture);
            _spriteBatch.End();

            #endregion

            #region Main content

            // Render regular content
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.FromNonPremultiplied(5, 3, 9, 255));

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, mat);

            _bgTreeSprite.Draw(gameTime, _spriteBatch);
            _bgBushSprite.Draw(gameTime, _spriteBatch);

            foreach (Entity e in _entities) e.Draw(gameTime, _spriteBatch);
            
            _grassSprite.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            #endregion

            #region GUI Layer

            // Render "Gui" Layer

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            _spriteBatch.Draw(_lightTarget, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(_bannerTexture, new Vector2(_graphics.PreferredBackBufferWidth / 2.0f, _graphics.PreferredBackBufferHeight / 4.0f), null, Color.White, 0.0f, new Vector2(_bannerTexture.Width / 2.0f, _bannerTexture.Height / 2.0f), _graphics.PreferredBackBufferWidth / (float)GAME_WIDTH, SpriteEffects.None, 0.0f);
            
            _spriteBatch.End();

            // Render "Gui" text
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, mat);

            string guiStr = "Press Esc to Quit";
            Vector2 guiTextOrigin = _gidole.MeasureString(guiStr) / 2.0f;
            guiTextOrigin.Y = 0;

            float guiStrXOffset = MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * 10;
            float guiStrYOffset = MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds + 2.5f) * 4;

            guiStrXOffset = 0.0f;

            _spriteBatch.DrawString(_gidole, guiStr, new Vector2(320 / 2 + guiStrXOffset, 55 + _cameraY + guiStrYOffset), Color.Gold, 0.0f, guiTextOrigin, 0.5f, SpriteEffects.None, 0.0f);

            _spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
    }
}
