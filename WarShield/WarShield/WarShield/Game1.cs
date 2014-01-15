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


        protected override void Initialize()
        {

            xnaDisplayDevice = new xTile.Display.XnaDisplayDevice(Content, GraphicsDevice);
            viewport = new xTile.Dimensions.Rectangle(new xTile.Dimensions.Size(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));

            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = Content.Load<Map>("BasicMap");
            map.LoadTileSheets(xnaDisplayDevice);

            enemySheet = Content.Load<Texture2D>("Enemies");

            enemy = new Sprite(Convert(298),
                                 enemySheet,
                                 new Rectangle(0, 0, 64, 64),
                                 new Vector2(0, 0));
        }

      
        protected override void UnloadContent()
        {
           
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState ms = Mouse.GetState();
            map.TileSheets[0].GetTileIndex(new xTile.Dimensions.Location(ms.X, ms.Y));
            int index = Convert(new Vector2(ms.X, ms.Y));
            Tile tile = map.GetLayer("Level").Tiles[1, 1];


            Window.Title = index.ToString();

            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Right))
            {
                viewport.X += 3;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            map.Draw(xnaDisplayDevice, viewport);

            enemy.Draw(spriteBatch);


            base.Draw(gameTime);
            spriteBatch.End();
        }




        //Block and Coordinate Conversions


        public Vector2 Convert(int block)
        {
            return new Vector2(((block % 20) * 64), ((block / 20) * 64));

        }

        public int Convert(Vector2 coordinate)
        {

            int block = ((int)(coordinate.X / 64) + ((int)(coordinate.Y / 64) * 20));

            return block;
        }



    }
}
