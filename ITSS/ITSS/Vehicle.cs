using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS
{
    class Vehicle
    {
        ContentManager SingularContent;
        
        Vector2 Begin;
        Vector2 End;
        Vector2 Pause;

        Vector2 VehiclePos;
        Vector2 MoveDirection;

        public Boolean DisposeMe = false;
        public Boolean Enabled = false;
        public Boolean Moving = false;
        public Boolean RightOfWay = true;

        public Texture2D VehicleSprite;
        public Rectangle VehicleRect;
        
        public Vehicle(ContentManager Content)
        {
            SingularContent = Content;
        }
        public void MoveCloserToEnd()
        {            
            if (Enabled && Moving)
            {
                MoveDirection = new Vector2(End.X - Begin.X, End.Y - Begin.Y);
                MoveDirection.Normalize();

                if (VehiclePos != Pause)
                {
                    if (VehiclePos != Pause)
                        VehiclePos += MoveDirection;
                    else
                        Moving = false;
                }
                else if (End == Pause && VehiclePos == Pause)
                    DisposeMe = true;
            }
        }

        public void setBegin(Vector2 begin)
        {            
            Begin = begin;
        }

        public void setPause(Vector2 pause)
        {
            Pause = pause;
        }

        public void setEnd(Vector2 end)
        {          
            End = end;
        }
        public Vector2 getEnd()
        {
            return End;
        }

        public Vector2 getPosition()
        {
            return VehiclePos;
        }

        public void Load()
        {
            VehicleSprite = SingularContent.Load<Texture2D>("tex/vehicle/car_top");
            VehiclePos = Begin;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            if (Enabled && !DisposeMe)
            {                
                VehicleRect = new Rectangle((int)VehiclePos.X, (int)VehiclePos.Y, 24, 48);
                spriteBatch.Draw(VehicleSprite, VehicleRect, Color.White);                
            }
        }

        public void Dispose()
        {
            VehicleSprite.Dispose();
        }
    }
}
