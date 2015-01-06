using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS
{    
    public class Bike
    {
        private Vector2 BikePos;
        private Rectangle BikeRect;
        private Texture2D BikeSprite;

        private SpriteBatch SingularSprite;
        private ContentManager SingularContent;

        public Bike(ContentManager Content, SpriteBatch Sprite)
        {
            SingularContent = Content;
            SingularSprite = Sprite;


            BikePos = new Vector2(0, 350);
            BikeRect = new Rectangle(0, 0, 100, 46);            
        }

        public void Load()
        {
            BikeSprite = SingularContent.Load<Texture2D>("bicycle_top");
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
            BikeRect = new Rectangle((int)BikePos.X, (int)BikePos.Y, 100, 46);
            spriteBatch.Draw(BikeSprite, BikeRect, Color.White);            
        }
    }
}
