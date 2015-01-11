using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS.TheGUI
{
    class FadingItem
    {
        public Vector2 Position;
        public int ShowTime = 0;
        public int ShowingTime;
        
        string Message;

        ContentManager SingularContent;
        SpriteFont Font;

        public FadingItem(Vector2 initpos, int showtime, object m, ContentManager c)
        {
            Position = initpos;
            ShowingTime = showtime;
            Message = m.ToString();
            SingularContent = c;            
        }

        public void Load()
        {
            Font = SingularContent.Load<SpriteFont>("font/GuiFont");
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            if (ShowingTime != ShowTime)
            {
                spriteBatch.DrawString(Font, Message.ToString(), Position, Color.White);
                ShowTime++;
            }
        }
    }
}
