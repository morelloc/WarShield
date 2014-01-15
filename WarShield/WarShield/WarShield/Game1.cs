using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using xTile;
using xTile.Display;
using xTile.Tiles;

namespace WarShield
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Texture2D Spritesheet;
        Map map;
        IDisplayDevice xnaDisplayDevice;
        xTile.Dimensions.Rectangle viewport;
        private static Song menuMusic;

        //Enemy Variables
        Texture2D enemySheet;
        Sprite enemy;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 960;
            graphics.ApplyChanges();
            Window.AllowUserResizing = true;

            IsMouseVisible = true;

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

            xnaDisplayDevice = new xTile.Display.XnaDisplayDevice(Content, GraphicsDevice);
            viewport = new xTile.Dimensions.Rectangle(new xTile.Dimensions.Size(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));

            this.IsMouseVisible = true;

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

            map = Content.Load<Map>("BasicMap");
            map.LoadTileSheets(xnaDisplayDevice);

            enemySheet = Content.Load<Texture2D>("Enemies");

            enemy = new Sprite(new Vector2(-150, 30), // Start at x=-150, y=30
                                 enemySheet,
                                 new Rectangle(164, 0, 163, 147), // Use this part of the superdog texture
                                 new Vector2(60, 20));



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState ms = Mouse.GetState();
            map.TileSheets[0].GetTileIndex(new xTile.Dimensions.Location(ms.X, ms.Y));
            int index = Convert(new Vector2(ms.X, ms.Y));
            Tile tile = map.GetLayer("Level").Tiles[1, 1];


            Window.Title = index.ToString();

            // TODO: Add your update logic here
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Right))
            {
                viewport.X += 3;
            }

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

            map.Draw(xnaDisplayDevice, viewport);

            spriteBatch.Begin();

            base.Draw(gameTime);
            spriteBatch.End();
        }

        public Vector2 Convert(int block)
        {
            return new Vector2(((block % 20) * 64), ((block / 14) * 64));

        }

        public int Convert(Vector2 coordinate)
        {

            int block = ((int)(coordinate.X / 64) + ((int)(coordinate.Y / 64) * 20));

            return block;
        }



    }
}
