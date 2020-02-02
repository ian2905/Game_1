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
        static float SPEEDCAP = 10;

        public Texture2D Sprite;
        public Rectangle Rect;
        public Vector2 Velocity;
        public BoundingRectangle HitBox;

        public bool Hit;

        public Enemy(Texture2D sprite, Rectangle size)
        {
            this.Sprite = sprite;
            this.Rect = size;
            this.Velocity = new Vector2(0, 0);
            this.HitBox = new BoundingRectangle(size.X, size.Y, size.Width, size.Height);
            this.Hit = false;
        }
        public void setSprite(Texture2D sprite)
        {
            this.Sprite = sprite;
        }

        public void hit()
        {
            Hit = true;
        }

        public void update(Player player, GraphicsDeviceManager graphics)
        {
            //Player Tracking
            Vector2 playerDistence = trackPlayer(player);
            

            //Enemy Restrictions

            if (Rect.Y < 0 - Rect.Height)
            {
                Rect.Y = 0 - Rect.Height;
            }
            if (Rect.Y > graphics.PreferredBackBufferHeight)
            {
                Rect.Y = graphics.PreferredBackBufferHeight;
            }
            if (Rect.X < 0 - Rect.Width)
            {
                Rect.X = 0 - Rect.Width;
            }
            if (Rect.X > graphics.PreferredBackBufferWidth)
            {
                Rect.X = graphics.PreferredBackBufferWidth;
            }

            //Physics

            //Final Update
            Console.WriteLine(Rect);
            Console.WriteLine((int)(SPEEDCAP * (playerDistence.X / Math.Abs(playerDistence.X + playerDistence.Y))));
            Console.WriteLine((int)(SPEEDCAP * (playerDistence.Y / Math.Abs(playerDistence.X + playerDistence.Y))));
            Rect.X += (int)(SPEEDCAP * (playerDistence.X / Math.Abs(playerDistence.X + playerDistence.Y)));
            Rect.Y += (int)(SPEEDCAP * (playerDistence.Y / Math.Abs(playerDistence.X + playerDistence.Y)));
            Console.WriteLine(Rect);
        }

        private Vector2 trackPlayer(Player player)
        {
            return new Vector2((player.Rect.X + player.Rect.Width/2) - this.Rect.X, (player.Rect.Y + player.Rect.Height / 2) - this.Rect.Y);
        }
    }
}
