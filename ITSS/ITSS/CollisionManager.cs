using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ITSS
{
    //dit werkt alleen voor de weg, in een grid, met een vaste grote. meh.
    class CollisionManager
    {
        //background collision
        List<Rectangle> BackgroundCollidables = new List<Rectangle>();

        //sidewalkcollision
        List<Rectangle> SidewalkCollidables = new List<Rectangle>();

        //menucollision
        public Rectangle[] MenuCollidables = new Rectangle[4];
        
        //menucollision
        Rectangle[] EntranceCollidables = new Rectangle[2];

        public Rectangle CurrentBackgroundCollidable = Rectangle.Empty;
        public Rectangle CurrentSidewalkCollidable = Rectangle.Empty;
        public Rectangle CurrentMenuCollidable = Rectangle.Empty;
        public Rectangle CurrentEntranceCollidable = Rectangle.Empty;
        public Rectangle CurrentExitCollidable = Rectangle.Empty;

        public Rectangle EntranceCollidable;
        public Rectangle ExitCollidable;

        public Vehicle ClosestVehicle;

        public int CurrentMenuCollidableType = 0;

        public void addCollidable(Rectangle collidable, int type)
        {
            BackgroundCollidables.Add(collidable);

            if (type != 0)
            {
                //now get a sidewalk hitbox, are 2
                switch (type)
                {
                    case 1:
                        //left
                        Rectangle left = new Rectangle(collidable.X, collidable.Y, collidable.Width / 4, collidable.Height);
                        Rectangle right = new Rectangle(collidable.X + (collidable.Width - (collidable.Width / 4)), collidable.Y, collidable.Width / 4, collidable.Height);
                        SidewalkCollidables.Add(left);
                        SidewalkCollidables.Add(right);
                        //right
                        break;
                    case 2:
                        break;
                    case 3:
                        Rectangle top = new Rectangle(collidable.X, collidable.Y, collidable.Width, collidable.Height / 4);
                        Rectangle down = new Rectangle(collidable.X, collidable.Y + (collidable.Height - (collidable.Height / 4)), collidable.Width, collidable.Height / 4);
                        SidewalkCollidables.Add(top);
                        SidewalkCollidables.Add(down);
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 20:
                        MenuCollidables[0] = collidable;
                        break;
                    case 21:
                        MenuCollidables[1] = collidable;
                        break;
                    case 22:                        
                        MenuCollidables[2] = collidable;
                        break;
                    case 23:
                        MenuCollidables[3] = collidable;
                        break;
                    case 31:
                        EntranceCollidable = collidable;
                        break;
                    case 32:
                        ExitCollidable = collidable;
                        break;                    
                }
            }
        }

        public void checkCollisions(Vector2 position, Boolean checkmenu)
        {
            if(checkmenu)
                checkMenuCollisions(position);

            checkBackgroundCollisions(position);
            checkEntranceCollisions(position);
            checkExitCollisions(position);
        }

        private void checkEntranceCollisions(Vector2 position)
        {
            Boolean inx = (position.X > EntranceCollidable.X && position.X < (EntranceCollidable.X + EntranceCollidable.Width));
            Boolean iny = (position.Y > EntranceCollidable.Y && position.Y < (EntranceCollidable.Y + EntranceCollidable.Height));

            if (inx && iny) //er is een collision met een entrance / exit
                CurrentEntranceCollidable = EntranceCollidable;
            else
                CurrentEntranceCollidable = Rectangle.Empty;
        }
        private void checkExitCollisions(Vector2 position)
        {
            Boolean inx = (position.X > ExitCollidable.X && position.X < (ExitCollidable.X + ExitCollidable.Width));
            Boolean iny = (position.Y > ExitCollidable.Y && position.Y < (ExitCollidable.Y + ExitCollidable.Height));

            if (inx && iny) //er is een collision met een entrance / exit
                CurrentExitCollidable = ExitCollidable;
            else
                CurrentExitCollidable = Rectangle.Empty;
        }
        
        public Timer Delay;        

        public void checkMenuCollisions(Vector2 position)
        {
            if (Delay == null)
            {
                int i = 0;
                foreach (Rectangle Collidable in MenuCollidables)
                {
                    if (Collidable != Rectangle.Empty)
                    {
                        Boolean inx = (position.X > Collidable.X && position.X < (Collidable.X + Collidable.Width));
                        Boolean iny = (position.Y > Collidable.Y && position.Y < (Collidable.Y + Collidable.Height));

                        if (inx && iny) //er is een collision met een weg
                        {
                            B = Collidable;
                            A = i;

                            Delay = new Timer(SetCurrentMenuCollidable, this, 1000, 1000);
                            break;
                        }
                    }
                    i++;
                }
            }           
        }
        
        public Rectangle B = Rectangle.Empty;
        public Int32 A = 0;

        private void SetCurrentMenuCollidable(object state)
        {
            ((CollisionManager)state).CurrentMenuCollidable = ((CollisionManager)state).B;
            ((CollisionManager)state).CurrentMenuCollidableType = ((CollisionManager)state).A;
            ((CollisionManager)state).Delay = null;
        }

        public void checkBackgroundCollisions(Vector2 position)
        {
            int i = 0;
            foreach (Rectangle Collidable in BackgroundCollidables)
            {                
                Boolean inx = (position.X > Collidable.X && position.X < (Collidable.X + Collidable.Width));
                Boolean iny = (position.Y > Collidable.Y && position.Y < (Collidable.Y + Collidable.Height));                

                if (inx && iny) //er is een collision met een weg
                {
                    CurrentBackgroundCollidable = Collidable; 
                    //check de sidewalk nu
                    checkSidewalkCollision(position);
                    break;
                }

                i++;
            }
        }

        public void checkSidewalkCollision(Vector2 position)
        {
            foreach(Rectangle Sidewalk in SidewalkCollidables)
            {
                Boolean inx = (position.X > Sidewalk.X && position.X < (Sidewalk.X + Sidewalk.Width));
                Boolean iny = (position.Y > Sidewalk.Y && position.Y < (Sidewalk.Y + Sidewalk.Height));

                if (inx && iny)
                {
                    CurrentSidewalkCollidable = Sidewalk;
                    Console.WriteLine(Sidewalk);
                    return;
                }
            }

            CurrentSidewalkCollidable = Rectangle.Empty; //lang leve empty
        }

        public Boolean ObjectToObjectDistance(Vector2 o1, Vector2 o2)
        {
            double dist = Vector2.Distance(o1, o2);
            return (dist < 160 && dist > 80);
        }

        public Boolean IsObjectTooCloseTo(Vector2 o1, Vector2 o2, object save = null)
        {
            double dist = Vector2.Distance(o1, o2);
            ClosestVehicle = (Vehicle)save;
            
            return (dist < 120);            
        }
    }
}

