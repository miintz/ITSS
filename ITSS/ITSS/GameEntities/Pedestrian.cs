using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITSS.GameEntities
{
    class Pedestrian
    {
        Boolean Zebrapath;

        Vector2 Begin;
        Vector2 End;
        Vector2 Pause;

        Vector2 PedestrianPos;
        Vector2 MoveDirection;

        public Boolean DisposeMe = false;
        public Boolean Moving = false;
        public Boolean RightOfWay = true;

        public Texture2D PedestrianSprite;
        public Rectangle PedestrianRect;

        public ContentManager SingularContent;
        
        public Pedestrian(ContentManager Content)
        {
            SingularContent = Content;
            Zebrapath = false;
        }

        public void Load()
        {
            PedestrianSprite = SingularContent.Load<Texture2D>("tex/vehicle/pedestrian");
            PedestrianPos = Begin;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            if (!DisposeMe)
            {
                PedestrianRect = new Rectangle((int)PedestrianPos.X, (int)PedestrianPos.Y, 19, 24);
                spriteBatch.Draw(PedestrianSprite, PedestrianRect, Color.White);
            }
        }
        public void MoveCloserToEnd()
        {
            if (Moving)
            {
                MoveDirection = new Vector2(End.X - Begin.X, End.Y - Begin.Y);
                MoveDirection.Normalize();

                if (PedestrianPos != Pause)
                {
                    if (PedestrianPos != Pause)
                        PedestrianPos += MoveDirection;
                    else
                        Moving = false;
                }
                else if (End == Pause && PedestrianPos == Pause)
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
            return PedestrianPos;
        }
    }
}
