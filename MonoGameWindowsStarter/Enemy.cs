using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    public class Enemy
    {
        static float SPEEDCAP = 2;
        static int SIZE = 32;

        public Game game;
        public Texture2D sprite;
        public Vector2 velocity;
        public BoundingRectangle hitBox;

        public bool hit;

        public Enemy(Game game, Texture2D sprite, Vector2 spawn)
        {
            this.game = game;
            this.sprite = sprite;
            this.velocity = new Vector2(0, 0);
            this.hitBox = new BoundingRectangle(spawn.X, spawn.Y, SIZE, SIZE);
            this.hit = false;
        }

        public void Hit()
        {
            hit = true;
        }

        private Vector2 TrackPlayer(Player player)
        {
            //Console.WriteLine(player.hitBox.X);

            return new Vector2((player.hitBox.X + player.hitBox.Width / 2) - (hitBox.X + hitBox.Width / 2), (player.hitBox.Y + player.hitBox.Height / 2) - (hitBox.Y + hitBox.Height / 2));
        }

        public void Update(Player player)
        {
            //Player Tracking
            Vector2 playerDistence = TrackPlayer(player);
            

            //Enemy Restrictions

            if (hitBox.Y < 0 - hitBox.Height)
            {
                hitBox.Y = 0 - hitBox.Height;
            }
            if (hitBox.Y > game.GraphicsDevice.Viewport.Height)
            {
                hitBox.Y = game.GraphicsDevice.Viewport.Height;
            }
            if (hitBox.X < 0 - hitBox.Width)
            {
                hitBox.X = 0 - hitBox.Width;
            }
            if (hitBox.X > game.GraphicsDevice.Viewport.Width)
            {
                hitBox.X = game.GraphicsDevice.Viewport.Width;
            }

            //Physics

            //Final Update
            Console.WriteLine(playerDistence);
            hitBox.X += SPEEDCAP * (playerDistence.X / (Math.Abs(playerDistence.X) + Math.Abs(playerDistence.Y)));
            hitBox.Y += SPEEDCAP * (playerDistence.Y / (Math.Abs(playerDistence.X) + Math.Abs(playerDistence.Y)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, hitBox, Color.Red);
        }
    }
}
