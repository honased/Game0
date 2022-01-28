using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game0
{
    /// <summary>
    /// A class representing a FireParticle
    /// </summary>
    public class FireParticle : Entity
    {
        private Texture2D _texture;
        private Vector2 _lightPosition;
        private float _lightScale;
        private static float currentOffset = 0.0f;
        private float offset = 0;
        private Random _random;
        private double _lightTimer;
        private double _lifeTime;
        private double _initialLife;
        private float _speedMod;

        /// <summary>
        /// The position of the FireParticle.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Is the particle dead or not.
        /// </summary>
        public bool Dead { get; private set; } = false;

        /// <summary>
        /// Create a new FireParticle entity
        /// </summary>
        /// <param name="random">A random number generator</param>
        public FireParticle(Random random)
        {
            _random = random;
            offset = currentOffset;
            currentOffset += 0.6f;
            _lightScale = 0.2f;
            _lifeTime = 1.0f + random.NextDouble() * 5.0f;
            _initialLife = _lifeTime;
            _speedMod = (float)(_random.NextDouble() + 1) * 2.0f;
        }

        /// <summary>
        /// Load the content for the FireParticle entity
        /// </summary>
        /// <param name="content">The content manager</param>
        public override void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Firefly");
        }

        /// <summary>
        /// Update the FireParticle entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            Position -= new Vector2(0.0f, 8 * (float)gameTime.ElapsedGameTime.TotalSeconds * _speedMod);
            _lightPosition = Position + new Vector2(MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds*4.5f + offset) * 5, 0);

            _lightTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(_lightTimer > .016f)
            {
                _lightTimer -= .016f;
                _lightScale = 0.2f + ((float)_random.NextDouble()) * 0.1f;
            }

            _lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if(_lifeTime <= 0)
            {
                Dead = true;
            }
        }

        /// <summary>
        /// Draw the FireParticle entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _lightPosition, null, Color.FromNonPremultiplied(255, 165, 0, (int)((_lifeTime / _initialLife)*255)), 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw the light source for the FireParticle entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        /// <param name="lightTexture">A light texture</param>
        public override void DrawLight(GameTime gameTime, SpriteBatch spriteBatch, Texture2D lightTexture)
        {
            spriteBatch.Draw(lightTexture, _lightPosition + new Vector2(1, 1), null, Color.FromNonPremultiplied(255, 255, 255, (int)((_lifeTime / _initialLife) * 255)), 0.0f, new Vector2(16, 16), _lightScale, SpriteEffects.None, 0f);
        }
    }
}
