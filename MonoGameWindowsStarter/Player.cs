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

        public Texture2D Sprite;
        public Rectangle Rect;
        public Vector2 Velocity;
        public BoundingRectangle HitBox;
        public bool Hit;
        public double shotTime;

        public Player(Texture2D sprite, Rectangle size, Vector2 velocity)
        {
            this.Sprite = sprite;
            this.Rect = size;
            this.Velocity = velocity;
            this.HitBox = new BoundingRectangle(size.X, size.Y, size.Width, size.Height);
            this.Hit = false;
            this.shotTime = 0;
        }

        public void setSprite(Texture2D sprite)
        {
            this.Sprite = sprite;
        }

        public void hit()
        {
            Hit = true;
        }

        public void update(KeyboardState newKeyboardState, KeyboardState oldKeyboardState, GraphicsDeviceManager graphics)
        {

            //Keyboard Input
            if (newKeyboardState.IsKeyDown(Keys.W))
            {
                Velocity.Y -= ACCELERATION;
            }

            if (newKeyboardState.IsKeyDown(Keys.S))
            {
                Velocity.Y += ACCELERATION;
            }
            if (newKeyboardState.IsKeyDown(Keys.A))
            {
                Velocity.X -= ACCELERATION;
            }

            if (newKeyboardState.IsKeyDown(Keys.D))
            {
                Velocity.X += ACCELERATION;
            }

            

            //Player Restrictions

            if (Rect.Y < 0)
            {
                Velocity.Y = 0;
                Rect.Y = 0;
            }
            if (Rect.Y > graphics.PreferredBackBufferHeight - Rect.Height)
            {
                Velocity.Y = 0;
                Rect.Y = graphics.PreferredBackBufferHeight - Rect.Height;
            }
            if (Rect.X < 0)
            {
                Velocity.X = 0;
                Rect.X = 0;
            }
            if (Rect.X > graphics.PreferredBackBufferWidth - Rect.Width)
            {
                Velocity.X = 0;
                Rect.X = graphics.PreferredBackBufferWidth - Rect.Width;
            }

            //Physics
            if (Velocity.X > 0)
            {
                Velocity.X -= FRICTION;
            }
            if (Velocity.Y > 0)
            {
                Velocity.Y -= FRICTION;
            }
            if (Velocity.X < 0)
            {
                Velocity.X += FRICTION;
            }
            if (Velocity.Y < 0)
            {
                Velocity.Y += FRICTION;
            }

            //Final Update
            if (Velocity.Y < SPEEDCAP)
            {
                Rect.Y += (int)Velocity.Y;
            }
            if (Velocity.X < SPEEDCAP)
            {
                Rect.X += (int)Velocity.X;
            }
        }
    }
}
