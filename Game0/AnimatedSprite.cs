using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game0
{
    public class AnimatedSprite
    {
        private Texture2D _texture;
        private float _frameTime;
        private int _frameWidth, _frameHeight;
        private float _timer;

        public int CurrentFrame { get; set; }

        public Vector2 Position { get; set; }

        public float Scale { get; set; } = 1.0f;

        public int FrameCount { get; private set; }

        public bool Paused { get; set; } = false;

        public AnimatedSprite(Texture2D tex, int frameWidth, int frameHeight, float frameTime)
        {
            _texture = tex;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameTime = frameTime;

            FrameCount = _texture.Width / frameWidth;

            _timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if(!Paused)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > _frameTime)
                {
                    _timer -= _frameTime;
                    CurrentFrame = (CurrentFrame + 1) % FrameCount;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, new Rectangle(CurrentFrame * _frameWidth, 0, _frameWidth, _frameHeight), Color.White, 0.0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0.0f);
        }
    }
}
