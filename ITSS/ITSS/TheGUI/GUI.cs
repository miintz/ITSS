using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS.TheGUI
{
    public class GUI
    {
        Int32 Score;
        String Level;

        SpriteFont Font;

        List<FadingItem> FadingItems = new List<FadingItem>();
        List<FeedbackMessage> FeedbackItems = new List<FeedbackMessage>();

        private Texture2D Cursor;
        private Vector2 CursorPos;

        ContentManager SingularContent;
        
        public GUI(ContentManager Content)
        {
            SingularContent = Content;
            Score = 100;
            Level = "1";
        }

        public void setLevel(String level)
        {
            Level = level;
        }

        public void addScore()
        {
            Score++;
        }

        public void addScore(int amount)
        {
            Score += amount;            
        }
        public void addScore(int amount, Vector2 pos, int showtime)
        {
            showFadingMessage(pos, showtime, "+" + amount.ToString() + " punten");
            Score += amount;
        }

        public void subtractScore()
        {
            this.Score--;
        }

        public void subtractScore(int amount)
        {
            this.Score -= amount;
        }

        public void subtractScore(int amount, Vector2 pos, int showtime)
        {
            showFadingMessage(pos, showtime, "-" + amount.ToString() + " punten");
            Score -= amount;
        }


        public void showFadingMessage(Vector2 pos, int showtime, object message)
        {
            FadingItem f = new FadingItem(pos, showtime, message, SingularContent);
            f.Load();
            FadingItems.Add(f);            
        }

        public void showFeedbackMessage(String message, Vector2 position, Int32 timelimit = 0)
        {
            FeedbackMessage f = new FeedbackMessage(SingularContent, message, position, timelimit);
            f.Load();
            FeedbackItems.Add(f);
        }

        public void Draw(ref SpriteBatch spriteBatch, Boolean drawScore)
        {
            //only draw score if the level is not a gui level
            if (drawScore)
            {
                //draw score
                spriteBatch.DrawString(Font, "Punten: " + Score, new Vector2(0, 0), Color.Black);

                //draw score
                spriteBatch.DrawString(Font, "Level: " + Level, new Vector2(0, 25), Color.Black);
            }
            
            //draw cursor
            spriteBatch.Draw(Cursor, new Rectangle((int)CursorPos.X, (int)CursorPos.Y, 25, 25), Color.White);
            
            foreach (FadingItem f in FadingItems)
            {
                f.Draw(ref spriteBatch);
            }

            foreach (FeedbackMessage f in FeedbackItems)
            {
                f.Draw(ref spriteBatch);
            }
        }

        public void Load()
        {
            CursorPos = new Vector2(0, 0);

            Cursor = SingularContent.Load<Texture2D>("tex/gui/cursor");
            Font = SingularContent.Load<SpriteFont>("font/GuiFont"); // Use the name of your sprite font file here instead of 'Score'.
        }

        public void handleMouseState()
        {
            MouseState ms = Mouse.GetState();

            CursorPos.X = ms.X;
            CursorPos.Y = ms.Y;
        }

    }
}
