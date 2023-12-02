// File: Gameplay.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages the gameplay logic, including updating and drawing the game state,
// handling player actions, enemy movements, collisions, and managing the game camera.

using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Proj6
{
    /// <summary>
    /// Manages the gameplay mechanics, updating and rendering game elements like the player, enemies, and projectiles.
    /// </summary>
    public class Gameplay
    {
        // Variables for fading effect
        private float _fadeAlpha = 1.0f;
        private float _fadeSpeed = 0.02f;

        private Camera _camera;

        private Texture2D _playerDown;
        private Texture2D _playerUp;
        private Texture2D _playerRight;
        private Texture2D _playerLeft;

        private Texture2D _fade;
        private Texture2D _background;
        private Texture2D _ball;
        private Texture2D _skull;

        private Texture2D _healthUISpritesheet;
        private Rectangle _labelRectangle;
        private Rectangle _fullHeartRectangle;
        private Rectangle _emptyHeartRectangle;

        /// <summary>
        /// The player character in the game.
        /// </summary>
        public Player Player;

        /// <summary>
        /// Constructor for the Gameplay class. Loads necessary content and initializes player and camera.
        /// </summary>
        /// <param name="content">ContentManager to load game assets.</param>
        /// <param name="game">Reference to the main game class for graphics device access.</param>
        public Gameplay(ContentManager content, Proj6 game)
        {
            _camera = new Camera(game.GraphicsDevice);
            Player = new Player();

            _playerDown = content.Load<Texture2D>("Player/playerDown");
            _playerRight = content.Load<Texture2D>("Player/playerRight");
            _playerLeft = content.Load<Texture2D>("Player/playerLeft");
            _playerUp = content.Load<Texture2D>("Player/playerUp");

            _fade = content.Load<Texture2D>("fade");
            _background = content.Load<Texture2D>("background");
            _ball = content.Load<Texture2D>("ball");
            _skull = content.Load<Texture2D>("skull");

            Player.Animations[0] = new SpriteAnimation(_playerDown, 4, 8);
            Player.Animations[1] = new SpriteAnimation(_playerUp, 4, 8);
            Player.Animations[2] = new SpriteAnimation(_playerLeft, 4, 8);
            Player.Animations[3] = new SpriteAnimation(_playerRight, 4, 8);

            Player.Animation = Player.Animations[0];

            _healthUISpritesheet = content.Load<Texture2D>("healthUI");
            _labelRectangle = new Rectangle(0, 0, 64, 52);
            _fullHeartRectangle = new Rectangle(64, 0, 64, 52);
            _emptyHeartRectangle = new Rectangle(128, 0, 64, 52);
        }

        /// <summary>
        /// Updates game elements such as player, enemies, and projectiles. Manages game logic.
        /// </summary>
        /// <param name="gameTime">Snapshot of game timing values.</param>
        public void Update(GameTime gameTime)
        {
            // Fade effect logic
            if (_fadeAlpha > 0)
            {
                _fadeAlpha -= _fadeSpeed;
                _fadeAlpha = MathHelper.Clamp(_fadeAlpha, 0f, 1f);
            }

            Player.Update(gameTime);
            if (!Player.Dead)
            {
                Controller.Update(gameTime, _skull);
            }

            _camera.Position = Player.Position;
            _camera.Update(gameTime);

            foreach (Projectile projectile in Projectile.Projectiles)
            {
                projectile.Update(gameTime);
            }

            foreach (Enemy enemy in Enemy.Enemies)
            {
                enemy.Update(gameTime, Player.Position, Player.Dead);
                int sum = 32 + enemy.Radius;
                if (Vector2.Distance(Player.Position, enemy.Position) < sum && !Player.Dead)
                {
                    Player.TakeHit();
                    enemy.Dead = true;
                }
            }

            foreach (Projectile projectile in Projectile.Projectiles)
            {
                foreach (Enemy enemy in Enemy.Enemies)
                {
                    int sum = projectile.Radius + enemy.Radius;
                    if (Vector2.Distance(projectile.Position, enemy.Position) < sum)
                    {
                        projectile.Collided = true;
                        enemy.Dead = true;
                    }
                }
            }

            Projectile.Projectiles.RemoveAll(p => p.Collided);
            Enemy.Enemies.RemoveAll(e => e.Dead);
        }

        /// <summary>
        /// Renders game elements to the screen, including the player, enemies, and UI components.
        /// </summary>
        /// <param name="gameTime">Snapshot of game timing values.</param>
        /// <param name="spriteBatch">SpriteBatch for drawing textures on the screen.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(_camera);

            spriteBatch.Draw(_background, new Vector2(-500, -500), Color.White);
            foreach (Enemy enemy in Enemy.Enemies)
            {
                enemy.Animation.Draw(spriteBatch);
            }
            foreach (Projectile projectile in Projectile.Projectiles)
            {
                spriteBatch.Draw(_ball, new Vector2(projectile.Position.X - 48, projectile.Position.Y - 48), Color.White);
            }
            if (!Player.Dead)
            {
                Player.Animation.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.Begin();

            float healthUIYPosition = Proj6.ScreenHeight - _labelRectangle.Height - 10;
            spriteBatch.Draw(_healthUISpritesheet, new Vector2(10, healthUIYPosition), _labelRectangle, Color.White);

            Vector2 heartPosition = new Vector2(10 + _labelRectangle.Width, healthUIYPosition);
            for (int i = 0; i < Player.MAX_HITS; i++)
            {
                Rectangle heartRect = (Player.MAX_HITS - Player.HitCount > i) ? _fullHeartRectangle : _emptyHeartRectangle;
                spriteBatch.Draw(_healthUISpritesheet, heartPosition, heartRect, Color.White);
                heartPosition.X += _fullHeartRectangle.Width - 15;
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

        /// <summary>
        /// Resets the game to its initial state, including player position and game entities.
        /// </summary>
        public void ResetGame()
        {
            // Reset player state
            Player = new Player();

            // Reset animations
            Player.Animations[0] = new SpriteAnimation(_playerDown, 4, 8);
            Player.Animations[1] = new SpriteAnimation(_playerUp, 4, 8);
            Player.Animations[2] = new SpriteAnimation(_playerLeft, 4, 8);
            Player.Animations[3] = new SpriteAnimation(_playerRight, 4, 8);
            Player.Animation = Player.Animations[0];

            // Reset hit count
            Player.HitCount = 0;

            // Reset camera position
            _camera.Position = Vector2.Zero;
            _camera.Update(new GameTime());

            // Clear and reset projectiles and enemies
            Projectile.Projectiles.Clear();
            Enemy.Enemies.Clear();

            // Reset fade effect
            _fadeAlpha = 1.0f;
        }
    }
}
