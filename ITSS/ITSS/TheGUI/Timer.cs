using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS.TheGUI
{
    class GUITimer
    {        
        public Boolean Active = true;
        public Int32 Percentage = 0;
        private Rectangle TimerRect;
        ContentManager SingularContent;

        public GUITimer(ContentManager Content)
        {
            SingularContent = Content;
            TimerRect = new Rectangle(0, 0, 0, 0);
            Active = false;    
        }

        public void Load(Vector2 initialposition)
        {
            TimerSprite = SingularContent.Load<Texture2D>("tex/gui/timer");      
            TimerPos = Vector2.Zero;

            TimerRect = new Rectangle((int)initialposition.X, (int)initialposition.Y, 5, 5); 
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            if (Active)
            {                
                spriteBatch.Draw(TimerSprite, TimerRect, Color.White);
            }
        }

        public void SetPosition(Vector2 newpos)
        {
            TimerPos = newpos;
            TimerRect = new Rectangle((int)newpos.X, (int)newpos.Y - 50, 5 + Percentage, 5 + Percentage); //set pos and update scale
        }

        public void GrowTimer()
        {
            this.Percentage++;
            
            //TimerRect = new Rectangle(TimerRect.X, TimerRect.Y, 5 + Percentage, 5 + Percentage);
            //TimerRect = new Rectangle((int)TimerRect.X - (int)(2.5 + Percentage), (int)TimerRect.Y - (int)(2.5 + Percentage), (int)5 + Percentage, (int)5 + Percentage); //set pos and update scale
            
            System.Console.WriteLine(TimerRect.ToString());
        }

        public Texture2D TimerSprite { get; set; }

        public Vector2 TimerPos { get; set; }
    }
}
