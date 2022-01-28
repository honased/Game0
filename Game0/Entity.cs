using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game0
{
    /// <summary>
    /// An abstract class defining an Entity
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Load the content for the entity
        /// </summary>
        /// <param name="content">The content manager</param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw the entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Draw the light source for the entity
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <param name="spriteBatch">A sprite batcher</param>
        /// <param name="lightTexture">A light texture</param>
        public virtual void DrawLight(GameTime gameTime, SpriteBatch spriteBatch, Texture2D lightTexture)
        {
            // Do nothing in general
        }
    }
}
