// File: MainMenu.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages the Main Menu screen of the game, including rendering the title, prompt text, and handling the fade effect upon game start.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Proj6
{
    /// <summary>
    /// Represents the main menu screen of the game.
    /// </summary>
    public class MainMenu
    {
        private SpriteFont _font;
        private SpriteFont _largeFont;

        private string _titleText = "SKULL  ISLAND"; // Title text
        private string _promptText = "PRESS ENTER TO PLAY"; // Prompt text

        private Vector2 _titlePosition;
        private Vector2 _titlePosition1Fall;
        private Vector2 _titlePosition2Fall;
        private Vector2 _promptPosition; // Position of the prompt text

        private float _promptBaselineY; // Base Y position for prompt
        private float _oscillationSpeed = 2.0f; // Position of the prompt

        private bool _enterPressed = false; // Flag to check if enter is pressed
        private TimeSpan _enterPressTime; // Time when enter was pressed

        private float _fadeValue = 0f; // Value for fade effect
        private TimeSpan _fadeDuration = TimeSpan.FromSeconds(1); // Duration of fade
        private Texture2D _fadeTexture; // Texture for fade effect

        private Texture2D _backgroundTexture; // Background texture

        /// <summary>
        /// Constructor for the MainMenu screen.
        /// </summary>
        /// <param name="content">ContentManager to load resources.</param>
        /// <param name="graphicsDevice">GraphicsDevice for creating textures.</param>
        public MainMenu(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _fadeTexture = new Texture2D(graphicsDevice, 1, 1);
            _fadeTexture.SetData(new[] { Color.Black });

            _font = content.Load<SpriteFont>("LanaPixel");
            _largeFont = content.Load<SpriteFont>("LanaPixelLarge");

            _backgroundTexture = content.Load<Texture2D>("mainMenuBackground");

            _titlePosition = new Vector2(
                (Proj6.ScreenWidth - _largeFont.MeasureString(_titleText).X) / 2,
                Proj6.ScreenHeight * 1 / 500
            );
            _titlePosition1Fall = _titlePosition;
            _titlePosition2Fall = _titlePosition;

            _promptPosition = new Vector2(
                (Proj6.ScreenWidth - _font.MeasureString(_promptText).X) / 2,
                Proj6.ScreenHeight * 12 / 16
            );
            _promptBaselineY = _promptPosition.Y;
        }

        /// <summary>
        /// Updates the main menu screen, handling input and fade effects.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>True if the game is transitioning from the main menu; otherwise, false.</returns>
        public bool Update(GameTime gameTime)
        {

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
        /// Draws the main menu screen, including the title, prompt, and fade effect.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing sprites.</param>
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
            spriteBatch.Begin();
        }
    }
}
