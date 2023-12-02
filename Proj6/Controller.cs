// File: Controller.cs
// Author: Aidan Harries
// Date: 12/1/2023
// Description: This class manages the spawning and timing of enemies in the game. It uses a timer
// to regularly spawn enemies from different directions based on a random selection.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proj6
{
    /// <summary>
    /// Manages enemy spawning and timing within the game.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Current countdown timer to spawn the next enemy.
        /// </summary>
        public static double Timer = 3D;

        /// <summary>
        /// Maximum time interval for the enemy spawn timer.
        /// </summary>
        public static double MaxTime = 3D;
        
        private static Random _random = new Random();

        /// <summary>
        /// Updates the enemy spawning mechanism based on the elapsed game time.
        /// Spawns enemies at random locations around the screen edges.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        /// <param name="spriteSheet">The texture sheet used for enemy sprites.</param>
        public static void Update(GameTime gameTime, Texture2D spriteSheet)
        {
            Timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (Timer <= 0)
            {
                int side = _random.Next(4);

                switch (side)
                {
                    case 0:
                        Enemy.Enemies.Add(new Enemy(new Vector2(-500, _random.Next(-500, 2000)), spriteSheet));
                        break;
                    case 1:
                        Enemy.Enemies.Add(new Enemy(new Vector2(2000, _random.Next(-500, 2000)), spriteSheet));
                        break;
                    case 2:
                        Enemy.Enemies.Add(new Enemy(new Vector2(_random.Next(-500, 2000), -500), spriteSheet));
                        break;
                    case 3:
                        Enemy.Enemies.Add(new Enemy(new Vector2(_random.Next(-500, 2000), 2000), spriteSheet));
                        break;
                }

                Timer = MaxTime;

                if (MaxTime > 0.5)
                {
                    MaxTime -= 0.02D;
                }
            }
        }
    }
}
