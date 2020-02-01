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
        public Rectangle rect;
        public Vector2 velocity;
        public BoundingRectangle hitBox;

        public Player(Texture2D sprite, Rectangle size, Vector2 velocity)
        {
            this.sprite = sprite;
            this.rect = size;
            this.velocity = velocity;
            this.hitBox = new BoundingRectangle(size.X, size.Y, size.Width, size.Height);
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
                velocity.Y -= ACCELERATION;
            }

            if (newKeyboardState.IsKeyDown(Keys.Down))
            {
                velocity.Y += ACCELERATION;
            }
            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                velocity.X -= ACCELERATION;
            }

            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                velocity.X += ACCELERATION;
            }

            

            //Player Restrictions

            if (rect.Y < 0)
            {
                velocity.Y = 0;
                rect.Y = 0;
            }
            if (rect.Y > graphics.PreferredBackBufferHeight - rect.Height)
            {
                velocity.Y = 0;
                rect.Y = graphics.PreferredBackBufferHeight - rect.Height;
            }
            if (rect.X < 0)
            {
                velocity.X = 0;
                rect.X = 0;
            }
            if (rect.X > graphics.PreferredBackBufferWidth - rect.Width)
            {
                velocity.X = 0;
                rect.X = graphics.PreferredBackBufferWidth - rect.Width;
            }

            //Physics
            if (velocity.X > 0)
            {
                velocity.X -= FRICTION;
            }
            if (velocity.Y > 0)
            {
                velocity.Y -= FRICTION;
            }
            if (velocity.X < 0)
            {
                velocity.X += FRICTION;
            }
            if (velocity.Y < 0)
            {
                velocity.Y += FRICTION;
            }

            //Final Update
            if (velocity.Y < SPEEDCAP)
            {
                rect.Y += (int)velocity.Y;
            }
            if (velocity.X < SPEEDCAP)
            {
                rect.X += (int)velocity.X;
            }
        }
    }
}
