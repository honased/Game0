using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game0
{
    /// <summary>
    /// A class representing a Player
    /// </summary>
    public class Player : Entity
    {
        private AnimatedSprite _sprite;

        private double _pauseTimer;

        /// <summary>
        /// Create a new FireParticle entity
        /// </summary>
        public Player()
        {
            _pauseTimer = 0.0;
        }

        /// <summary>
        /// Load the content for the player entity
        /// </summary>
        /// <param name="content">The content manager</param>
        public override void LoadContent(ContentManager content)
        {
            _sprite = new AnimatedSprite(content.Load<Texture2D>("Sprites/Player"), 18, 16, 0.21f) { Position = new Vector2(134, 145) };
        }

        /// <summary>
        /// Update the Player entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            if(_sprite.Paused)
            {
                _pauseTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_pauseTimer >= 2.5)
                {
                    _pauseTimer -= 2.5;
                    _sprite.Paused = false;
                    _sprite.CurrentFrame = 1;
                }
            }
            else
            {
                if(_sprite.CurrentFrame == 0)
                {
                    _sprite.Paused = true;
                }
            }    

            _sprite.Update(gameTime);
        }

        /// <summary>
        /// Draw the Player entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch);
        }
    }
}
