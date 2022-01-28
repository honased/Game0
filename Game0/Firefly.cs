using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game0
{
    /// <summary>
    /// A class representing a Firefly
    /// </summary>
    public class Firefly : Entity
    {
        private Texture2D _texture;

        private Vector2 _lightPosition;

        private float _lightScale;

        private static float currentOffset = 0.0f;

        private float offset = 0;

        private Random _random;

        private double _lightTimer;

        /// <summary>
        /// The position of the Firefly
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Create a new Firefly entity
        /// </summary>
        /// <param name="random">A random number generator</param>
        public Firefly(Random random)
        {
            _random = random;
            offset = currentOffset;
            currentOffset += 0.6f;
            _lightScale = 0.2f;
        }

        /// <summary>
        /// Load the content for the Firefly entity
        /// </summary>
        /// <param name="content">The content manager</param>
        public override void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Firefly");
        }

        /// <summary>
        /// Update the Firefly entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            _lightPosition = Position + new Vector2(MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds + offset) * 3, MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds + offset) * 2.5f);

            _lightTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(_lightTimer > .016f)
            {
                _lightTimer -= .016f;
                _lightScale = 0.2f + ((float)_random.NextDouble()) * 0.1f;
            }
        }

        /// <summary>
        /// Draw the Firefly entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _lightPosition, Color.White);
        }

        /// <summary>
        /// Draw the light source for the Firefly entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        /// <param name="lightTexture">A light texture</param>
        public override void DrawLight(GameTime gameTime, SpriteBatch spriteBatch, Texture2D lightTexture)
        {
            spriteBatch.Draw(lightTexture, _lightPosition + new Vector2(1, 1), null, Color.White, 0.0f, new Vector2(16, 16), _lightScale, SpriteEffects.None, 0f);
        }
    }
}
