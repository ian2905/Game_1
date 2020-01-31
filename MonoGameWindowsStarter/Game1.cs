﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        Texture2D ball;
        Vector2 ballPosition = Vector2.Zero;
        Vector2 ballVelocity;
        Texture2D paddle;
        Rectangle paddleRect;
        int paddleSpeed = 0;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            ballVelocity = new Vector2(
                (float)random.NextDouble(),
                (float)random.NextDouble()
            );

            paddleRect.X = 0;
            paddleRect.Y = 0;
            paddleRect.Width = 50;
            paddleRect.Height = 250;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = Content.Load<Texture2D>("ball");
            paddle = Content.Load<Texture2D>("OnePixel");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();


            // Input
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            paddleRect.Y += paddleSpeed;

            if(newKeyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up))
            {
                paddleSpeed -= 1;
            }

            if (newKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down))
            {
                paddleSpeed += 1;
            }

            if (paddleRect.Y < 0)
            {
                paddleRect.Y = 0;
            }

            if (paddleRect.Y > GraphicsDevice.Viewport.Height - paddleRect.Height)
            {
                paddleRect.Y = GraphicsDevice.Viewport.Height - paddleRect.Height;
            }


            // TODO: Add your update logic here
            ballPosition += (float)gameTime.ElapsedGameTime.TotalMilliseconds * ballVelocity;

            //Check for wall collisions
            if(ballPosition.Y < 0)
            {
                ballVelocity.Y *= -1;
                float delta = 0 - ballPosition.Y;
                ballPosition.Y += 2 * delta;
            }

            if (ballPosition.Y > graphics.PreferredBackBufferHeight - 100)
            {
                ballVelocity.Y *= -1;
                float delta = graphics.PreferredBackBufferHeight - 100 - ballPosition.Y;
                ballPosition.Y += 2 * delta;
            }

            if (ballPosition.X < 0)
            {
                ballVelocity.X *= -1;
                float delta = 0 - ballPosition.X;
                ballPosition.X += 2 * delta;
            }

            if (ballPosition.X > graphics.PreferredBackBufferWidth - 100)
            {
                ballVelocity.X *= -1;
                float delta = graphics.PreferredBackBufferWidth - 100 - ballPosition.X;
                ballPosition.X += 2 * delta;
            }

            /*
            if(paddleRect.Y <= 0)
            {
                paddleSpeed = 0;
                paddleRect.Y = 1;
            }
            if (paddleRect.Y >= graphics.PreferredBackBufferHeight - paddleRect.Height)
            {
                paddleSpeed = 0;
                paddleRect.Y = graphics.PreferredBackBufferHeight - paddleRect.Height - 1;
            }
            */

            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(ball, new Rectangle((int)ballPosition.X, (int)ballPosition.Y, 100, 100), Color.White);
            spriteBatch.Draw(paddle, paddleRect, Color.Red);

            // All draws in here
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
