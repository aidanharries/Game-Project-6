// File: Enemy.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class represents an enemy in the game. It manages the enemy's position, movement, 
// animation, and state (alive or dead). The enemies are updated based on the player's position and the game state.

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proj6
{
    /// <summary>
    /// Represents an enemy in the game, with properties for animation, position, and state.
    /// </summary>
    public class Enemy
    {
        /// <summary>
        /// Static list of all enemies in the game.
        /// </summary>
        public static List<Enemy> Enemies = new List<Enemy>();

        /// <summary>
        /// The animation for the enemy sprite.
        /// </summary>
        public SpriteAnimation Animation;

        /// <summary>
        /// Radius for enemy collision detection.
        /// </summary>
        public int Radius = 30;

        private Vector2 _position = new Vector2(0, 0);
        private int _speed = 150;
        private bool _dead = false;

        /// <summary>
        /// Constructor for the Enemy class. Initializes the enemy's position and animation.
        /// </summary>
        /// <param name="newPosition">The starting position of the enemy.</param>
        /// <param name="spriteSheet">The sprite sheet used for the enemy's animation.</param>
        public Enemy(Vector2 newPosition, Texture2D spriteSheet)
        {
            _position = newPosition;
            Animation = new SpriteAnimation(spriteSheet, 10, 6);
        }

        /// <summary>
        /// Gets the current position of the enemy.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Gets or sets the dead state of the enemy.
        /// </summary>
        public bool Dead
        {
            get { return _dead; }
            set { _dead = value; }
        }

        /// <summary>
        /// Updates the enemy's position and animation based on the player's position and game state.
        /// </summary>
        /// <param name="gameTime">Game time information.</param>
        /// <param name="playerPosition">The current position of the player.</param>
        /// <param name="isPlayerDead">The dead state of the player.</param>
        public void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            Animation.Position = new Vector2(_position.X - 48, _position.Y - 66);
            Animation.Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isPlayerDead)
            {
                Vector2 moveDirection = playerPosition - _position;
                moveDirection.Normalize();
                _position += moveDirection * _speed * deltaTime;
            }
        }
    }
}
