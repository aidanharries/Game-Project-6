// File: SpriteAnimation.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages sprite animations within the game. It includes functionality for managing sprite frames, 
// updating animations, and rendering sprites. The class extends SpriteManager to handle sprite sheet textures and animation frames.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proj6
{
    /// <summary>
    /// Manages textures and rectangles for individual frames for sprite animations.
    /// </summary>
    public class SpriteManager
    {
        protected Texture2D Texture;
        protected Rectangle[] Rectangles;
        protected int FrameIndex = 0;

        public Vector2 Position = Vector2.Zero;
        public Color Color = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        public float Scale = 1f;
        public SpriteEffects SpriteEffect;

        /// <summary>
        /// Constructor for SpriteManager. Splits a texture into frames for animation.
        /// </summary>
        /// <param name="Texture">Texture to be divided into frames.</param>
        /// <param name="frames">Number of frames in the sprite animation.</param>
        public SpriteManager(Texture2D Texture, int frames)
        {
            this.Texture = Texture;
            int width = Texture.Width / frames;
            Rectangles = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle(i * width, 0, width, Texture.Height);
            }
        }

        /// <summary>
        /// Draws the sprite using the current frame index.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[FrameIndex], Color, Rotation, Origin, Scale, SpriteEffect, 0f);
        }
    }

    /// <summary>
    /// Extends SpriteManager to add animation functionality, managing frame updates and animation looping.
    /// </summary>
    public class SpriteAnimation : SpriteManager
    {
        private float _timeElapsed;
        private float _timeToUpdate;

        public bool IsLooping = true;

        /// <summary>
        /// Sets the frame rate of the animation.
        /// </summary>
        public int FramesPerSecond { set { _timeToUpdate = (1f / value); } }

        /// <summary>
        /// Constructor for SpriteAnimation. Sets up the animation with the specified frames and frame rate.
        /// </summary>
        /// <param name="Texture">Texture to be used for the animation.</param>
        /// <param name="frames">Number of frames in the animation.</param>
        /// <param name="fps">Frame rate of the animation.</param>
        public SpriteAnimation(Texture2D Texture, int frames, int fps) : base(Texture, frames)
        {
            FramesPerSecond = fps;
        }

        /// <summary>
        /// Updates the animation based on the game time.
        /// </summary>
        /// <param name="gameTime">Game time information for timing the frame updates.</param>
        public void Update(GameTime gameTime)
        {
            _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeElapsed > _timeToUpdate)
            {
                _timeElapsed -= _timeToUpdate;

                if (FrameIndex < Rectangles.Length - 1)
                {
                    FrameIndex++;
                }
                else if (IsLooping)
                {
                    FrameIndex = 0;
                }
            }
        }

        /// <summary>
        /// Sets the current frame of the animation.
        /// </summary>
        /// <param name="frame">Frame index to be set.</param>
        public void SetFrame(int frame)
        {
            FrameIndex = frame;
        }
    }
}
