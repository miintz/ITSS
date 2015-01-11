using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS.GameEntities
{    
    public class Bike
    {
        private Vector2 BikePos;
        private Rectangle BikeRect;
        private Texture2D BikeSprite;

        public Boolean Enabled = false;

        private ContentManager SingularContent;

        public Rectangle Entrance;
        public Rectangle Exit;

        public Bike(ContentManager Content)
        {
            SingularContent = Content;
           
            BikePos = new Vector2(0, 0);
            BikeRect = new Rectangle(0, 0, 50, 23);            
        }

        public void Load()
        {
            BikePos = new Vector2(Entrance.X + 50, Entrance.Y + 50);
            BikeSprite = SingularContent.Load<Texture2D>("tex/vehicle/bicycle_top");
        }

        public void MoveTo(int x, int y)
        {
            BikePos.X = x;
            BikePos.Y = y;            
        }

        public Vector2 GetPosition()
        {
            return BikePos;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {            
            if(Enabled)
            { 
                BikeRect = new Rectangle((int)BikePos.X, (int)BikePos.Y, 50, 23);
                spriteBatch.Draw(BikeSprite, BikeRect, Color.White);            
            }
        }
    }
}
