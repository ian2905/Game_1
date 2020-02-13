using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWindowsStarter
{
    public enum Face
    {
        Down,
        Right,
        Left,
        Up
    }
    public class Player
    {
        const float FRICTION = (float).5;
        const float ACCELERATION = (float).05;
        const int FRAME_WIDTH = 49;
        const int FRAME_HEIGHT = 63;
        const int ANIMATION_FRAME_RATE = 124;
        const int SHOT_LOOK_TIME = 1240;
        const int SPEEDCAP = 40;

        public int[,] frameVectorX = { {0, 49, 101, 147}, {4, 49, 101, 147}, {4, 49, 101, 147}, {0, 49, 101, 147} };
        public int[,] frameVectorY = { {3, 0, 3, 0}, {63, 63, 63, 63 }, {130, 130, 130, 130 }, {201, 197, 201, 197} };
        public int[,] frameVectorW = { { 46, 50, 46, 50 }, { 38, 39, 38, 39 }, { 38, 39, 38, 39 }, { 45, 49, 45, 49 } };
        public int[,] frameVectorH = { { 56, 62, 56, 62 }, { 64, 63, 64, 63 }, { 64, 63, 64, 63 }, { 58, 61, 58, 61 } };


        public Game game;

        public double shotTime;
        public Texture2D sprite;
        Dictionary<string, SoundEffect> soundEffects;
        public Vector2 velocity;
        public BoundingRectangle hitBox;
        public bool hit;
        private Face face;
        
        public TimeSpan animationTimer;
        public TimeSpan shotTimer;
        public int frame;

        public Player(Game game)
        {
            this.game = game;
            velocity = new Vector2(0, 0);
            hitBox = new BoundingRectangle(((game.GraphicsDevice.Viewport.Height / 2) - (FRAME_HEIGHT / 2)),
                                                ((game.GraphicsDevice.Viewport.Width / 2) - (FRAME_WIDTH / 2)),
                                                FRAME_WIDTH,
                                                FRAME_HEIGHT);
            shotTime = 0;
            soundEffects = new Dictionary<string, SoundEffect>();
            hit = false;
            face = Face.Down;
            animationTimer = new TimeSpan();
            shotTimer = new TimeSpan();

        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("playerSprite");
            soundEffects.Add("PlayerHit", content.Load<SoundEffect>("playerHit"));
            soundEffects.Add("PlayerShoot", content.Load<SoundEffect>("playerShoot"));
        }

        public void Hit()
        {
            hit = true;
            soundEffects["PlayerHit"].CreateInstance().Play();
        }

        public void Shoot(Face direction, double time)
        {
            shotTime = time;
            shotTimer = new TimeSpan(0, 0, 0, 0, SHOT_LOOK_TIME);
            face = direction;
            soundEffects["PlayerShoot"].CreateInstance().Play();
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();


            Console.WriteLine(gameTime.TotalGameTime.Milliseconds);
            //Keyboard Input
            if (velocity.Y > -1*SPEEDCAP) 
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    velocity.Y -= ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (shotTimer.Milliseconds <= 0)
                    {
                        face = Face.Up;
                        shotTimer = new TimeSpan();
                    }
                    else
                    {
                        shotTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }
            }
            if (velocity.Y < SPEEDCAP)
            {
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    velocity.Y += ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (shotTimer.Milliseconds <= 0)
                    {
                        face = Face.Down;
                        shotTimer = new TimeSpan();
                    }
                    else
                    {
                        shotTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }
            }
            if (velocity.X > -1*SPEEDCAP)
            {
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    velocity.X -= ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (shotTimer.Milliseconds <= 0)
                    {
                        face = Face.Left;
                        shotTimer = new TimeSpan();
                    }
                    else
                    {
                        shotTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }
            }
            if (velocity.X < SPEEDCAP)
            {
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    velocity.X += ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (shotTimer.Milliseconds <= 0)
                    {
                        face = Face.Right;
                        shotTimer = new TimeSpan();
                    }
                    else
                    {
                        shotTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }
                }
            }
            

            //Player Restrictions

            if (hitBox.Y < 0)
            {
                velocity.Y = 0;
                hitBox.Y = 0;
            }
            if (hitBox.Y > game.GraphicsDevice.Viewport.Height - hitBox.Height)
            {
                velocity.Y = 0;
                hitBox.Y = game.GraphicsDevice.Viewport.Height - hitBox.Height;
            }
            if (hitBox.X < 0)
            {
                velocity.X = 0;
                hitBox.X = 0;
            }
            if (hitBox.X > game.GraphicsDevice.Viewport.Width - hitBox.Width)
            {
                velocity.X = 0;
                hitBox.X = game.GraphicsDevice.Viewport.Width - hitBox.Width;
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

            while (animationTimer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                Console.WriteLine(frame);
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                animationTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }
            frame %= 4;
            if (Math.Abs(velocity.X) > 1 || Math.Abs(velocity.Y) > 1)
            {
                animationTimer += gameTime.ElapsedGameTime;
            }



            //Final Update
            hitBox.Y += (int)velocity.Y;
            hitBox.X += (int)velocity.X;
            hitBox.Width = frameVectorW[(int)face, frame];
            hitBox.Height = frameVectorH[(int)face, frame];

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (!hit)
            {
                
                spriteBatch.Draw(sprite, new Vector2(hitBox.X, hitBox.Y), new Rectangle(
                                                        frameVectorX[(int)face, frame],
                                                        frameVectorY[(int)face, frame],
                                                        frameVectorW[(int)face, frame],
                                                        frameVectorH[(int)face, frame]), Color.White);
            }
            else
            {
                spriteBatch.Draw(sprite, new Vector2(hitBox.X, hitBox.Y), new Rectangle(
                                                        frameVectorX[(int)face, frame],
                                                        frameVectorY[(int)face, frame],
                                                        frameVectorW[(int)face, frame],
                                                        frameVectorH[(int)face, frame]), Color.DarkGreen);
                spriteBatch.DrawString(
                        spriteFont,
                        "GAME OVER",
                        new Vector2((game.GraphicsDevice.Viewport.Width/2) - 100,
                                    game.GraphicsDevice.Viewport.Height/ 3),
                        Color.White
                );
            }
        }
    }
}
