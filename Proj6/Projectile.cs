// File: Projectile.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages projectiles in the game. It handles the creation, movement,
// and collision state of each projectile.

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Proj6
{
    /// <summary>
    /// Represents a projectile in the game, managing its position, movement, and collision state.
    /// </summary>
    public class Projectile
    {
        private Vector2 _position;
        private int _speed = 1000;
        private Direction _direction;
        private bool _collided = false;

        /// <summary>
        /// A static list that holds all projectiles in the game.
        /// </summary>
        public static List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// Radius of the projectile, used for collision detection.
        /// </summary>
        public int Radius = 18;

        /// <summary>
        /// Constructor for creating a projectile.
        /// </summary>
        /// <param name="newPosition">The starting position of the projectile.</param>
        /// <param name="newDirection">The direction in which the projectile will move.</param>
        public Projectile(Vector2 newPosition, Direction newDirection)
        {
            _position = newPosition;
            _direction = newDirection;
        }

        /// <summary>
        /// Gets the current position of the projectile.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Gets or sets the collision state of the projectile.
        /// </summary>
        public bool Collided
        {
            get { return _collided; }
            set { _collided = value; }
        }

        /// <summary>
        /// Updates the projectile's position based on its direction and speed.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values to calculate movement.</param>
        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (_direction)
            {
                case Direction.Right:
                    _position.X += _speed * deltaTime;
                    break;
                case Direction.Left:
                    _position.X -= _speed * deltaTime;
                    break;
                case Direction.Down:
                    _position.Y += _speed * deltaTime;
                    break;
                case Direction.Up:
                    _position.Y -= _speed * deltaTime;
                    break;
            }
        }
    }
}
