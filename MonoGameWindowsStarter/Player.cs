using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    public struct Player
    {
        static float FRICTION = (float).4;
        static int ACCELERATION = 1;
        static int SPEEDCAP = 50;

        public Texture2D sprite;
        public Rectangle playerRect;
        public Vector2 playerVelocity;

        public Player(Texture2D sprite, Rectangle size, Vector2 velocity)
        {
            this.sprite = sprite;
            this.playerRect = size;
            this.playerVelocity = velocity;
        }

        public void setSprite(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        public void update(KeyboardState newKeyboardState, KeyboardState oldKeyboardState, GraphicsDeviceManager graphics)
        {

            //Keyboard Input
            if (newKeyboardState.IsKeyDown(Keys.Up))
            {
                playerVelocity.Y -= ACCELERATION;
            }

            if (newKeyboardState.IsKeyDown(Keys.Down))
            {
                playerVelocity.Y += ACCELERATION;
            }
            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                playerVelocity.X -= ACCELERATION;
            }

            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                playerVelocity.X += ACCELERATION;
            }

            

            //Player Restrictions

            if (playerRect.Y < 0)
            {
                playerVelocity.Y = 0;
                playerRect.Y = 0;
            }
            if (playerRect.Y > graphics.PreferredBackBufferHeight - playerRect.Height)
            {
                playerVelocity.Y = 0;
                playerRect.Y = graphics.PreferredBackBufferHeight - playerRect.Height;
            }
            if (playerRect.X < 0)
            {
                playerVelocity.X = 0;
                playerRect.X = 0;
            }
            if (playerRect.X > graphics.PreferredBackBufferWidth - playerRect.Width)
            {
                playerVelocity.X = 0;
                playerRect.X = graphics.PreferredBackBufferWidth - playerRect.Width;
            }

            //Physics
            if (playerVelocity.X > 0)
            {
                playerVelocity.X -= FRICTION;
            }
            if (playerVelocity.Y > 0)
            {
                playerVelocity.Y -= FRICTION;
            }
            if (playerVelocity.X < 0)
            {
                playerVelocity.X += FRICTION;
            }
            if (playerVelocity.Y < 0)
            {
                playerVelocity.Y += FRICTION;
            }

            //Final Update
            if (playerVelocity.Y < SPEEDCAP)
            {
                playerRect.Y += (int)playerVelocity.Y;
            }
            if (playerVelocity.X < SPEEDCAP)
            {
                playerRect.X += (int)playerVelocity.X;
            }
        }
    }
}
