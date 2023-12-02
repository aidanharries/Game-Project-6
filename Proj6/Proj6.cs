// File: Proj6.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages the game state, rendering, and updating of different game screens such as the main menu,
// how-to-play, gameplay, and game over screens. It also handles screen resolution settings and background music.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Proj6
{
    /// <summary>
    /// Main class for the game, handling game state management, screen rendering, and transitions.
    /// </summary>
    public class Proj6 : Game
    {
        private const int SCREEN_WIDTH = 1280;
        private const int SCREEN_HEIGHT = 720;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private MainMenu _mainMenu;
        private HowToPlay _howToPlay;
        private Gameplay _gameplay;
        private GameOver _gameOver;

        private GameState _currentState;

        private Song _backgroundMusic;

        /// <summary>
        /// Provides public access to the screen width.
        /// </summary>
        public static int ScreenWidth => SCREEN_WIDTH;

        /// <summary>
        /// Provides public access to the screen height.
        /// </summary>
        public static int ScreenHeight => SCREEN_HEIGHT;

        /// <summary>
        /// Constructor for Proj6. Initializes graphics settings and sets the initial game state.
        /// </summary>
        public Proj6()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;

            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            _currentState = GameState.MainMenu;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Loads game content such as sprites and music, and initializes game screens.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mainMenu = new MainMenu(Content, GraphicsDevice);
            _howToPlay = new HowToPlay(Content, GraphicsDevice);
            _gameplay = new Gameplay(Content, this);
            _gameOver = new GameOver(Content, GraphicsDevice);

            _backgroundMusic = Content.Load<Song>("backgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(_backgroundMusic);
        }

        /// <summary>
        /// Updates the game state and manages transitions between different game screens.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // State management logic
            switch (_currentState)
            {
                case GameState.MainMenu:
                    if (_mainMenu.Update(gameTime))
                    {
                        _currentState = GameState.HowToPlay;
                    }
                    break;

                case GameState.HowToPlay:
                    if (_howToPlay.Update(gameTime))
                    {
                        _currentState = GameState.Gameplay;
                    }
                    break;

                case GameState.Gameplay:
                    _gameplay.Update(gameTime);
                    if (_gameplay.Player.Dead)
                    {
                        _currentState = GameState.GameOver;
                    }
                    break;
                case GameState.GameOver:
                    if (_gameOver.Update(gameTime))
                    {
                        _gameplay.ResetGame(); // Reset the game
                        _currentState = GameState.Gameplay;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the current game state to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Render current state
            switch (_currentState)
            {
                case GameState.MainMenu:
                    _mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.HowToPlay:
                    _howToPlay.Draw(_spriteBatch);
                    break;

                case GameState.Gameplay:
                    _gameplay.Draw(gameTime, _spriteBatch);
                    break;

                case GameState.GameOver:
                    _gameOver.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}