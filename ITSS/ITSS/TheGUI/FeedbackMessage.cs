using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS.TheGUI
{
    public class FeedbackMessage
    {
        String Message;
        ContentManager SingularContent;
        Int32 Timelimit;
        Int32 Time;

        SpriteFont Font;

        Boolean Dispose;
        Vector2 Position;

        public FeedbackMessage(ContentManager content, String message, Vector2 position, int timelimit = 0)
        {
            SingularContent = content;
            Timelimit = timelimit;
            Position = position;

            Message = message;
        }

        public void Load()
        {
            Font = SingularContent.Load<SpriteFont>("font/GuiFont"); // Use the name of your sprite font file here instead of 'Score'.
        }

        public void Draw(ref SpriteBatch spriteBatch)
        { 
            //draw score           
            if (Time != Timelimit)
            {
                spriteBatch.DrawString(Font, Message, Position, Color.White);
                Time++;
            }            
        }
    }
}
