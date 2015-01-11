#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using ITSS.TheGUI;
using System.Runtime.InteropServices;
using System.Reflection;
#endregion

namespace ITSS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ITSS : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Listener TUIOListener;        

        public Vector2 Resolution;

        public GUI TheGUI;
        private Level TheLevel;

        private ITSS that;

        public ITSS()
            : base()
        {
            that = this;

            TUIOListener = new Listener();
            TUIOListener.Construct(new String[] { "3333" });

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferHeight = 1080 - (1080 / 8);
            //graphics.PreferredBackBufferWidth = 1920 - (1920 / 8);

            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1440;

            Resolution.Y = 900;
            Resolution.X = 1440;

            //Window.IsBorderless = true;
            Window.AllowUserResizing = true;
            Window.SetPosition(new Point(0, 0));
            graphics.ToggleFullScreen();
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            TheGUI = new GUI(Content);
            TheLevel = new Level(Content, ref that);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //GameBackground = Content.Load<Texture2D>("tex/terrain/background_void");
            TheGUI.Load();
            TheLevel.Load("-1");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //muis ingeklikt?
            handleMouseState();
            handleKeyPressed();

            //we moeten mogelijk iets doen met bepaalde collisions dingen...
            if ((TheLevel.collisionManager.CurrentMenuCollidable != Rectangle.Empty) && (TheLevel.currentLevel == "Help" || TheLevel.currentLevel == "-1" || TheLevel.currentLevel == "0"))
            {
                switch (TheLevel.collisionManager.CurrentMenuCollidableType)
                {
                    case 0: //initial start screen
                        TheLevel.collisionManager.MenuCollidables[0] = Rectangle.Empty;
                        TheLevel.Load("0");
                        break;
                    case 1: //help screen
                        TheLevel.Load("Help");
                        break;
                    case 2: //first level
                        TheLevel.Load("1");
                        break;
                    case 3:
                        TheLevel.Load("-1");
                        break;
                }

                //reset menu collision
                TheLevel.collisionManager.CurrentMenuCollidable = Rectangle.Empty;
                TheLevel.collisionManager.CurrentMenuCollidableType = Int32.MaxValue;
            }

            if (TheLevel.collisionManager.CurrentSidewalkCollidable != Rectangle.Empty)
                TheGUI.subtractScore();

            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            TheLevel.Draw(ref spriteBatch);

            if (TheLevel.currentLevel != "0" && TheLevel.currentLevel != "-1" && TheLevel.currentLevel != "4" && TheLevel.currentLevel != "Help") //STATISCH, niet goed
                TheGUI.Draw(ref spriteBatch, true);
            else
                TheGUI.Draw(ref spriteBatch, false);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void handleMouseState()
        {
            TheLevel.handleMouseState();
            TheGUI.handleMouseState();
        }

        private void handleKeyPressed()
        {
            //level switching
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.F1))
            {
                TheLevel.Load("1");
                TheGUI.setLevel("1");
            }
            if (ks.IsKeyDown(Keys.F2))
            {
                TheLevel.Load("2");
                TheGUI.setLevel("2");
            }
            if (ks.IsKeyDown(Keys.F3))
            {
                TheLevel.Load("3");
                TheGUI.setLevel("3");
            }
        }
    }

    public static class GameWindowExtensions
    {
        public static void SetPosition(this GameWindow window, Point position)
        {
            OpenTK.GameWindow OTKWindow = GetForm(window);
            if (OTKWindow != null)
            {
                OTKWindow.X = position.X;
                OTKWindow.Y = position.Y;
            }
        }

        public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        {
            Type type = typeof(OpenTKGameWindow);
            System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                return field.GetValue(gameWindow) as OpenTK.GameWindow;
            return null;
        }
    }
}
    