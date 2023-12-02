// File: GameOver.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages the game over screen, including rendering text and handling fade animations.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Proj6
{
    /// <summary>
    /// Class for handling the game over screen.
    /// </summary>
    public class GameOver
    {
        // Variables for fade animation
        private float _fadeAlpha = 1.0f;
        private float _fadeSpeed = 0.02f;

        // Fonts for displaying text
        private SpriteFont _font;
        private SpriteFont _largeFont;

        // Text for instructions and titles
        private string _titleText = "GAME OVER";
        private string _promptText = "PRESS ENTER TO PLAY AGAIN";

        private Vector2 _titlePosition;
        private Vector2 _titlePosition1Fall;
        private Vector2 _titlePosition2Fall;

        private Vector2 _promptPosition;
        private float _promptBaselineY;
        private float _oscillationSpeed = 2.0f;

        // Variables to handle enter key press
        private bool _enterPressed = false;
        private TimeSpan _enterPressTime;

        // Fade effect variables
        private float _fadeValue = 0f;
        private TimeSpan _fadeDuration = TimeSpan.FromSeconds(1);
        private Texture2D _fadeTexture;

        private Texture2D _backgroundTexture;
        private Texture2D _fade;

        /// <summary>
        /// Constructor for the GameOver screen.
        /// </summary>
        /// <param name="content">Content manager for loading resources.</param>
        /// <param name="graphicsDevice">Graphics device for rendering.</param>
        public GameOver(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _fadeTexture = new Texture2D(graphicsDevice, 1, 1);
            _fadeTexture.SetData(new[] { Color.Black });

            _font = content.Load<SpriteFont>("LanaPixel");
            _largeFont = content.Load<SpriteFont>("LanaPixelLarge");

            _fade = content.Load<Texture2D>("fade");
            _backgroundTexture = content.Load<Texture2D>("howToPlayBackground");

            _titlePosition = new Vector2(
                (Proj6.ScreenWidth - _largeFont.MeasureString(_titleText).X) / 2,
                Proj6.ScreenHeight * 1 / 500
            );
            _titlePosition1Fall = _titlePosition;
            _titlePosition2Fall = _titlePosition;

            _promptPosition = new Vector2(
                (Proj6.ScreenWidth - _font.MeasureString(_promptText).X) / 2,
                Proj6.ScreenHeight / 2
            );
            _promptBaselineY = _promptPosition.Y;
        }

        /// <summary>
        /// Updates the state of the game over screen, handling animations and input.
        /// </summary>
        /// <param name="gameTime">Snapshot of game timing.</param>
        /// <returns>True if the screen should transition, otherwise false.</returns>
        public bool Update(GameTime gameTime)
        {
            // Fade effect logic
            if (_fadeAlpha > 0)
            {
                _fadeAlpha -= _fadeSpeed;
                _fadeAlpha = MathHelper.Clamp(_fadeAlpha, 0f, 1f);
            }

            // Update falling title positions
            float fallSpeed = 0.75f;
            float fallLimit = 10.0f;

            if (_titlePosition1Fall.Y - _titlePosition.Y < fallLimit)
            {
                _titlePosition1Fall.Y += fallSpeed;
            }
            if (_titlePosition2Fall.Y - _titlePosition.Y < fallLimit * 2)
            {
                _titlePosition2Fall.Y += fallSpeed;
            }

            if (!_enterPressed)
            {
                _promptPosition.Y = _promptBaselineY + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * _oscillationSpeed) * 20.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_enterPressed)
            {
                _enterPressed = true;
                _enterPressTime = gameTime.TotalGameTime;
            }

            if (_enterPressed)
            {
                float fadeProgress = (float)(gameTime.TotalGameTime - _enterPressTime).TotalSeconds / (float)_fadeDuration.TotalSeconds;
                _fadeValue = MathHelper.Clamp(fadeProgress, 0f, 1f);

                if (_fadeValue >= 1f)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Draws the game over screen to the given sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw to.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Proj6.ScreenWidth, Proj6.ScreenHeight), Color.White);

            Color black2 = new Color(100, 100, 100);
            Color black1 = new Color(75, 75, 75);
            Color black = new Color(50, 50, 50);

            Color promptColor = _enterPressed ? Color.White : black;
            spriteBatch.DrawString(_font, _promptText, _promptPosition, promptColor);

            // Draw titles 
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition2Fall, black2);
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition1Fall, black1);
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition, black);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if (_enterPressed)
            {
                byte alphaValue = (byte)(_fadeValue * 255);
                Color fadeColor = new Color((byte)255, (byte)255, (byte)255, alphaValue);
                spriteBatch.Draw(_fadeTexture, new Rectangle(0, 0, Proj6.ScreenWidth, Proj6.ScreenHeight), fadeColor);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if (_fadeAlpha > 0)
            {
                Color fadeColor = new Color(0, 0, 0, _fadeAlpha);
                spriteBatch.Draw(_fade, new Rectangle(0, 0, Proj6.ScreenWidth, Proj6.ScreenHeight), fadeColor);
            }

            spriteBatch.End();
            spriteBatch.Begin();
        }

    }
}
