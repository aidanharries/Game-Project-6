// File: GameOver.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class represents the player in the game. It manages the player's movement, 
// animations, hit detection, and health status.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Proj6
{
    /// <summary>
    /// Enumeration for player movement direction.
    /// </summary>
    public enum Direction
    {
        Down,
        Up,
        Left,
        Right
    }

    /// <summary>
    /// Class representing the player. Manages position, speed, direction, movement, and hit detection.
    /// </summary>
    public class Player
    {
        private Vector2 _position = new Vector2(500, 300);
        private int _speed = 400;
        private Direction _direction = Direction.Down;
        private bool _isMoving = false;
        private KeyboardState _keyboardStateOld = Keyboard.GetState();
        private float _hitFlashDuration = 0.2f; // Duration to flash red
        private float _hitFlashTimer = 0;

        /// <summary>
        /// Maximum number of hits the player can take.
        /// </summary>
        public const int MAX_HITS = 10;

        /// <summary>
        /// Current number of hits taken by the player.
        /// </summary>
        public int HitCount = 0;

        /// <summary>
        /// Indicates whether the player is dead.
        /// </summary>
        public bool Dead = false;

        /// <summary>
        /// Current animation of the player.
        /// </summary>
        public SpriteAnimation Animation;

        /// <summary>
        /// Array of animations for each movement direction.
        /// </summary>
        public SpriteAnimation[] Animations = new SpriteAnimation[4];

        /// <summary>
        /// Gets the current position of the player.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Sets the X-coordinate of the player's position.
        /// </summary>
        /// <param name="newX">New X-coordinate.</param>
        public void SetX(float newX)
        {
            _position.X = newX;
        }

        /// <summary>
        /// Sets the Y-coordinate of the player's position.
        /// </summary>
        /// <param name="newY">New Y-coordinate.</param>
        public void SetY(float newY)
        {
            _position.Y = newY;
        }

        /// <summary>
        /// Updates the player's state, including movement and animation.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _isMoving = false;

            if (Dead)
            {
                _isMoving = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Space))
            {
                _isMoving = false;
            }
            else
            {
                // Determine the movement direction
                bool movingRight = keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
                bool movingLeft = keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
                bool movingUp = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
                bool movingDown = keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);

                // Diagonal Movement
                if (movingRight && movingUp || movingRight && movingDown)
                {
                    _direction = Direction.Right;
                    _isMoving = true;
                }
                else if (movingLeft && movingUp || movingLeft && movingDown)
                {
                    _direction = Direction.Left;
                    _isMoving = true;
                }
                // Horizontal Movement
                else if (movingRight)
                {
                    _direction = Direction.Right;
                    _isMoving = true;
                }
                else if (movingLeft)
                {
                    _direction = Direction.Left;
                    _isMoving = true;
                }
                // Vertical Movement
                else if (movingUp)
                {
                    _direction = Direction.Up;
                    _isMoving = true;
                }
                else if (movingDown)
                {
                    _direction = Direction.Down;
                    _isMoving = true;
                }

                // Apply movement
                if (_isMoving)
                {
                    if (movingRight && _position.X < 1275)
                    {
                        _position.X += _speed * deltaTime;
                    }
                    if (movingLeft && _position.X > 225)
                    {
                        _position.X -= _speed * deltaTime;
                    }
                    if (movingUp && _position.Y > 200)
                    {
                        _position.Y -= _speed * deltaTime;
                    }
                    if (movingDown && _position.Y < 1250)
                    {
                        _position.Y += _speed * deltaTime;
                    }
                }

                // Update hit flash timer
                if (_hitFlashTimer > 0)
                {
                    _hitFlashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_hitFlashTimer <= 0)
                    {
                        // Reset color to white after flash duration
                        foreach (var animation in Animations)
                        {
                            animation.Color = Color.White;
                        }
                    }
                }
            }

            // Update animation and other functionalities
            Animation = Animations[(int)_direction];
            Animation.Position = new Vector2(_position.X - 48, _position.Y - 48);

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Animation.SetFrame(0);
            }
            else if (_isMoving)
            {
                Animation.Update(gameTime);
            }
            else
            {
                Animation.SetFrame(1);
            }

            if (keyboardState.IsKeyDown(Keys.Space) && _keyboardStateOld.IsKeyUp(Keys.Space) && !Dead)
            {
                Projectile.Projectiles.Add(new Projectile(_position, _direction));
            }

            _keyboardStateOld = keyboardState;
        }

        /// <summary>
        /// Applies damage to the player and updates the hit count and state.
        /// </summary>
        public void TakeHit()
        {
            if (HitCount < MAX_HITS)
            {
                HitCount++;
                _hitFlashTimer = _hitFlashDuration;
                // Change color to red to indicate hit
                foreach (var animation in Animations)
                {
                    animation.Color = Color.Red;
                }

                if (HitCount >= MAX_HITS)
                {
                    Dead = true;
                }
            }
        }
    }
}
