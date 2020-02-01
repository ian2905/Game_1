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
    public struct Enemy
    {
        static float ACCELERATION = 1;
        static int SPEEDCAP = 10;

        public Texture2D sprite;
        public Rectangle rect;
        public Vector2 velocity;
        public BoundingRectangle hitBox;

        public Enemy(Texture2D sprite, Rectangle size, Vector2 velocity)
        {
            this.sprite = sprite;
            this.rect = size;
            this.velocity = velocity;
            this.hitBox = new BoundingRectangle(size.X, size.Y, size.Width, size.Height);
        }

        public void update(Player player, GraphicsDeviceManager graphics)
        {
            //Player Tracking
            Vector2 playerDistence = trackPlayer(player);
            this.velocity.Y += ACCELERATION * (playerDistence.Y / (playerDistence.X + playerDistence.Y));
            this.velocity.X += ACCELERATION * (playerDistence.X / (playerDistence.X + playerDistence.Y));

            //Enemy Restrictions

            if (rect.Y < 0 - rect.Height)
            {
                rect.Y = 0 - rect.Height;
            }
            if (rect.Y > graphics.PreferredBackBufferHeight)
            {
                rect.Y = graphics.PreferredBackBufferHeight;
            }
            if (rect.X < 0 - rect.Width)
            {
                rect.X = 0 - rect.Width;
            }
            if (rect.X > graphics.PreferredBackBufferWidth)
            {
                rect.X = graphics.PreferredBackBufferWidth;
            }

            //Physics

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

        private Vector2 trackPlayer(Player player)
        {
            return new Vector2((player.rect.X + player.rect.Width/2) - this.rect.X, (player.rect.Y + player.rect.Height / 2) - this.rect.Y);
        }
    }
}
