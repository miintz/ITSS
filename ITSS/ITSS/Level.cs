using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using ITSS.GameEntities;
using ITSS.TheGUI;

namespace ITSS
{
    class Level
    {
        public CollisionManager collisionManager = new CollisionManager();
        private ContentManager SingularContent;

        Texture2D[] RoadTiles = new Texture2D[999];
        Texture2D[] SignTiles = new Texture2D[3];

        public List<Vehicle> Vehicles = new List<Vehicle>();
        public List<Pedestrian> Pedestrians = new List<Pedestrian>();
        Bike TheBike;
        GUITimer Timer;
        GUITimer GTimer;

        ITSS GameRef;

        Boolean PossibleRightOfWaySituationVehicle;
        Boolean PossibleRightOfWaySituationPedestrian;
        Boolean BikeTooCloseToVehicle = false;

        public String currentLevel = "-1";

        Timer Delay = null;

        SpriteFont Font;

        public bool BikeTooCloseToPedestrian { get; set; }

        public Level(ContentManager Content, ref ITSS Ref)
        {
            GameRef = Ref;

            SingularContent = Content;
            TheBike = new Bike(Content);
        }

        public void Load(String level)
        {
            currentLevel = level;

            TheBike = new Bike(SingularContent);
            TheBike.Entrance = collisionManager.EntranceCollidable;
            TheBike.Load();

            if (level != "-1" && level != "0")
            {
                LoadSigns();
                LoadVehicles();
                TheBike.Enabled = true;
            }

            LoadLevel();

            GTimer = null;

            collisionManager.Delay = null;
            collisionManager.CurrentMenuCollidableType = -1;
            collisionManager.CurrentMenuCollidable = Rectangle.Empty;
        }

        private void LoadVehicles()
        {
            int x = 0;
            int y = 0;

            Vehicles = new List<Vehicle>();
            Pedestrians = new List<Pedestrian>();
            if (File.Exists("Content/maps/area" + currentLevel + "_vehicles.txt"))
            {
                String[] Lines = System.IO.File.ReadAllLines("Content/maps/area" + currentLevel + "_vehicles.txt");
                foreach (String Line in Lines)
                {
                    //dit werkt dus maar met 1 auto nog                
                    String[] types = Line.Split(new char[] { ',' });
                    foreach (String type in types)
                    {
                        //alleen car begin en car end
                        switch (type)
                        {
                            case "pb":
                                if (Pedestrians.Count != 0)
                                    Pedestrians.First().setBegin(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Pedestrian pedestrian = new Pedestrian(SingularContent);

                                    pedestrian.setBegin(new Vector2((150 * x) + 50, 150 * y));
                                    Pedestrians.Add(pedestrian);
                                }
                                break;
                            case "pe":
                                if (Pedestrians.Count != 0)
                                    Pedestrians.First().setEnd(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Pedestrian pedestrian = new Pedestrian(SingularContent);

                                    pedestrian.setEnd(new Vector2((150 * x) + 50, 150 * y));
                                    Pedestrians.Add(pedestrian);
                                }
                                break;
                            case "p1b":
                                if (Pedestrians.Count != 0)
                                    Pedestrians.First().setPause(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Pedestrian pedestrian = new Pedestrian(SingularContent);

                                    pedestrian.setBegin(new Vector2((150 * x) + 50, 150 * y));
                                    Pedestrians.Add(pedestrian);
                                }
                                break;
                            case "p1e":
                                if (Vehicles.Count != 0)
                                    Pedestrians.First().setBegin(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Pedestrian pedestrian = new Pedestrian(SingularContent);

                                    pedestrian.setEnd(new Vector2((150 * x) + 50, 150 * y));
                                    Pedestrians.Add(pedestrian);
                                }
                                break;
                            case "cb":
                                if (Vehicles.Count != 0)
                                    Vehicles.First().setBegin(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Vehicle car = new Vehicle(SingularContent);

                                    car.setBegin(new Vector2((150 * x) + 50, 150 * y));
                                    Vehicles.Add(car);
                                }
                                break;
                            case "ce":
                                if (Vehicles.Count != 0)
                                    Vehicles.First().setEnd(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Vehicle car = new Vehicle(SingularContent);

                                    car.setEnd(new Vector2((150 * x) + 50, 150 * y));
                                    Vehicles.Add(car);
                                }
                                break;
                            case "cp":
                                if (Vehicles.Count != 0)
                                    Vehicles.First().setPause(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Vehicle car = new Vehicle(SingularContent);

                                    car.setPause(new Vector2((150 * x) + 50, 150 * y));
                                    Vehicles.Add(car);
                                }
                                break;
                            case "c1b":
                                if (Vehicles.Count != 0)
                                {
                                    Vehicles.First().setBegin(new Vector2((150 * x) + 50, 150 * y));
                                }
                                else
                                {
                                    Vehicle car = new Vehicle(SingularContent);
                                    car.RightOfWay = false;
                                    car.setBegin(new Vector2((150 * x) + 50, 150 * y));
                                    Vehicles.Add(car);
                                }
                                break;
                            case "c1e":
                                if (Vehicles.Count != 0)
                                    Vehicles.First().setEnd(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Vehicle car = new Vehicle(SingularContent);
                                    car.RightOfWay = false;
                                    car.setEnd(new Vector2((150 * x) + 50, 150 * y));
                                    Vehicles.Add(car);
                                }
                                break;
                            case "c1p":
                                if (Vehicles.Count != 0)
                                    Vehicles.First().setPause(new Vector2((150 * x) + 50, 150 * y));
                                else
                                {
                                    Vehicle car = new Vehicle(SingularContent);
                                    car.RightOfWay = false;
                                    car.setPause(new Vector2((150 * x) + 50, 150 * y));
                                    Vehicles.Add(car);
                                }
                                break;
                        }

                        x++;
                    }

                    x = 0;
                    y++;
                }

                //load vehicles
                foreach (Vehicle v in Vehicles)
                {
                    v.Load();
                }

                //load pedestrians
                foreach (Pedestrian p in Pedestrians)
                {
                    p.Load();
                }
            }

            //load timer
            Timer = new GUITimer(SingularContent);
            Timer.Load(TheBike.GetPosition());
        }

        private void LoadSigns()
        {
            if (File.Exists("Content/maps/area" + currentLevel + "_signs.txt"))
            {
                String[] Lines = System.IO.File.ReadAllLines("Content/maps/area" + currentLevel + "_signs.txt");

                int x = 0;
                int y = 0;

                foreach (String Line in Lines)
                {
                    String[] indices = Line.Split(new char[] { ',' });
                    foreach (String indice in indices)
                    {
                        switch (indice)
                        {
                            case "1":
                                SignTiles[1] = SingularContent.Load<Texture2D>("tex/sign/sign_takeway");
                                break;
                            case "2":
                                SignTiles[2] = SingularContent.Load<Texture2D>("tex/sign/sign_giveway");
                                break;
                            case "3":
                                SignTiles[2] = SingularContent.Load<Texture2D>("tex/sign/sign_stop");
                                break;
                        }

                        x++;
                    }

                    x = 0;
                    y++;
                }
            }
        }

        private void LoadLevel()
        {
            String[] Lines = System.IO.File.ReadAllLines("Content/maps/area" + currentLevel + ".txt");

            Font = SingularContent.Load<SpriteFont>("font/GuiFont"); // Use the name of your sprite font file here instead of 'Score'.

            int x = 0;
            int y = 0;

            RoadTiles = new Texture2D[100]; //dynamische lengte?

            foreach (String Line in Lines)
            {
                String[] indices = Line.Split(new char[] { ',' });
                foreach (String indice in indices)
                {
                    switch (indice)
                    {
                        case "0":
                            RoadTiles[0] = SingularContent.Load<Texture2D>("tex/terrain/background");
                            break;
                        case "1":
                            RoadTiles[1] = SingularContent.Load<Texture2D>("tex/terrain/ver");
                            break;
                        case "2":
                            RoadTiles[2] = SingularContent.Load<Texture2D>("tex/terrain/cross");
                            break;
                        case "3":
                            RoadTiles[3] = SingularContent.Load<Texture2D>("tex/terrain/hor");
                            break;
                        case "4":
                            RoadTiles[4] = SingularContent.Load<Texture2D>("tex/terrain/zebhor");
                            break;
                        case "5":
                            RoadTiles[5] = SingularContent.Load<Texture2D>("tex/terrain/zebver");
                            break;
                        case "31":
                            RoadTiles[31] = SingularContent.Load<Texture2D>("tex/terrain/hor");
                            TheBike.Entrance = new Rectangle(150 * x, 150 * y, 150, 150);
                            break;
                        case "32":
                            RoadTiles[32] = SingularContent.Load<Texture2D>("tex/terrain/hor");
                            TheBike.Exit = new Rectangle(150 * x, 150 * y, 150, 150);
                            break;
                        case "6":
                            RoadTiles[6] = SingularContent.Load<Texture2D>("tex/terrain/turn_rtt");
                            break;
                        case "7":
                            RoadTiles[7] = SingularContent.Load<Texture2D>("tex/terrain/turn_ttr");
                            break;
                        case "20":
                            RoadTiles[20] = SingularContent.Load<Texture2D>("tex/gui/start_btn");
                            break;
                        case "21":
                            RoadTiles[21] = SingularContent.Load<Texture2D>("tex/gui/help_btn");
                            break;
                        case "22":
                            RoadTiles[22] = SingularContent.Load<Texture2D>("tex/gui/newgame_btn");
                            break;
                        case "23":
                            RoadTiles[23] = SingularContent.Load<Texture2D>("tex/gui/return_btn");
                            break;
                        case "99":
                            RoadTiles[99] = SingularContent.Load<Texture2D>("tex/gui/endgame_btn");
                            break;
                    }

                    if (indice != "Help")
                        collisionManager.addCollidable(new Rectangle(150 * x, 150 * y, 150, 150), Int32.Parse(indice));

                    x++;
                }

                x = 0;
                y++;
            }
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            //DRAW stuff
            Int32 sizex = (Int32)GameRef.Resolution.X / 10;
            Int32 sizey = (Int32)GameRef.Resolution.Y / 10;

            int x = 0;
            int y = 0;

            String[] AreaLines = System.IO.File.ReadAllLines("Content/maps/area" + currentLevel + ".txt");

            foreach (String Line in AreaLines)
            {
                String[] indices = Line.Split(new char[] { ',' });
                foreach (String indice in indices)
                {
                    if (indice == "0")
                    {
                        if (currentLevel != "-1" && currentLevel != "0" && currentLevel != "4")
                            spriteBatch.Draw(RoadTiles[Int32.Parse(indice)], new Rectangle(150 * x, 150 * y, 150, 150), Color.White);
                        //else
                        //    spriteBatch.Draw(SingularContent.Load<Texture2D>("tex/terrain/background_void"), new Rectangle(150 * x, 150 * y, 150,150), Color.White);
                    }
                    else if (indice == "Help")
                    {
                        spriteBatch.DrawString(Font, "Dit is het helpscherm, lees hier dingen over de dingen!\nPlaats fiets op ding om terug te gaan", new Vector2(50, 50), Color.Black);
                        spriteBatch.Draw(RoadTiles[0], new Rectangle(150 * x, 150 * y, 150, 150), Color.White); //draw backhground to fill up thing
                    }
                    else
                        spriteBatch.Draw(RoadTiles[Int32.Parse(indice)], new Rectangle(150 * x, 150 * y, 150, 150), Color.White);

                    x++;
                }
                x = 0;
                y++;
            }

            x = 0;
            y = 0;

            if (File.Exists("Content/maps/area" + currentLevel + "_signs.txt"))
            {
                String[] SignLines = System.IO.File.ReadAllLines("Content/maps/area" + currentLevel + "_signs.txt");

                foreach (String Line in SignLines)
                {
                    String[] indices = Line.Split(new char[] { ',' });
                    foreach (String indice in indices)
                    {
                        if (indice != "0")
                            spriteBatch.Draw(SignTiles[Int32.Parse(indice)], new Rectangle((150 * x) - 40, (150 * y) + 120, 75, 75), Color.White);

                        x++;
                    }
                    x = 0;
                    y++;
                }
            }

            TheBike.Draw(ref spriteBatch);

            if ((this.PossibleRightOfWaySituationVehicle || PossibleRightOfWaySituationPedestrian) && Delay == null)
            {
                if (!Timer.Active && Timer.Percentage != -1)
                {
                    Timer.Active = true;
                    Timer.Percentage = 0;
                    Timer.SetPosition(TheBike.GetPosition());
                }
                else
                {
                    if (Timer.Percentage != 100 && Timer.Percentage != -1)
                    {
                        Timer.GrowTimer();
                        Timer.SetPosition(TheBike.GetPosition());
                        Timer.Draw(ref spriteBatch);
                    }
                    else if (Timer.Percentage != -1)
                    {
                        //add to score, maar alleen als het de bedoeling was om te wachten...
                        //vehicle is closer
                        if (collisionManager.ObjectToObjectDistance(TheBike.GetPosition(), collisionManager.ClosestPedestrian.getPosition()) > collisionManager.ObjectToObjectDistance(TheBike.GetPosition(), collisionManager.ClosestVehicle.getPosition()))
                        {
                            if (collisionManager.ClosestVehicle.RightOfWay)
                            {
                                Timer.Percentage = -1;
                                Timer.Active = false;

                                GameRef.TheGUI.addScore(25, new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 25), 150);
                                ContinueMovingVehicles();
                            }
                            else
                            {
                                Timer.Percentage = -1;
                                Timer.Active = false;

                                GameRef.TheGUI.subtractScore(25, new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 25), 150);
                                ContinueMovingVehicles();

                                GameRef.TheGUI.showFeedbackMessage("Je hebt voorrang!", new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 75), 200);
                            }
                        }

                        //pedestrian is closer
                        if (collisionManager.ObjectToObjectDistance(TheBike.GetPosition(), collisionManager.ClosestPedestrian.getPosition()) < collisionManager.ObjectToObjectDistance(TheBike.GetPosition(), collisionManager.ClosestVehicle.getPosition()))
                        {
                            if (collisionManager.ClosestPedestrian.RightOfWay)
                            {
                                Timer.Percentage = -1;
                                Timer.Active = false;

                                GameRef.TheGUI.addScore(25, new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 25), 150);
                                ContinueMovingPedestrians();
                            }
                            else
                            {
                                Timer.Percentage = -1;
                                Timer.Active = false;

                                GameRef.TheGUI.subtractScore(25, new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 25), 150);
                                ContinueMovingPedestrians();

                                GameRef.TheGUI.showFeedbackMessage("Je hebt voorrang!", new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 75), 200);
                            }
                        }
                    }
                }
            }
            else if (Timer != null)
            {
                //mogelijk is het te dichtbij
                if (Timer.Active && (BikeTooCloseToVehicle || BikeTooCloseToPedestrian))
                {
                    //fietser is doorgereden, als de vehicle right-of-way heeft moet er van de score af anders niet
                    if (collisionManager.ClosestVehicle.RightOfWay)
                    {
                        Timer.Active = false;
                        GameRef.TheGUI.subtractScore(25, new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 25), 150);

                        Delay = new Timer(ContinueMovingVehicles, this, 2000, 2000);

                        if (collisionManager.ObjectToObjectDistance(TheBike.GetPosition(), collisionManager.ClosestPedestrian.getPosition()) < collisionManager.ObjectToObjectDistance(TheBike.GetPosition(), collisionManager.ClosestVehicle.getPosition()))
                            GameRef.TheGUI.showFeedbackMessage("De voetganger heeft voorrang", new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 75), 200);
                        else
                            GameRef.TheGUI.showFeedbackMessage("De auto heeft voorrang", new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 75), 200);
                    }
                    else
                    {
                        Timer.Active = false;
                        GameRef.TheGUI.addScore(25, new Vector2(TheBike.GetPosition().X, TheBike.GetPosition().Y - 25), 150);

                        Delay = new Timer(ContinueMovingVehicles, this, 2000, 2000);
                    }
                }
            }

            //COLLISION stuff
            foreach (Vehicle vehicle in Vehicles)
            {
                if (!vehicle.DisposeMe)
                {
                    //draw and check distance to bike   
                    vehicle.MoveCloserToEnd();
                    vehicle.Draw(ref spriteBatch);

                    this.PossibleRightOfWaySituationVehicle = collisionManager.ObjectToObjectClose(TheBike.GetPosition(), vehicle.getPosition());
                    this.BikeTooCloseToVehicle = collisionManager.IsObjectTooCloseTo(TheBike.GetPosition(), vehicle.getPosition(), vehicle);
                }
            }

            foreach (Pedestrian pedestrian in Pedestrians)
            {
                if (!pedestrian.DisposeMe)
                {
                    //draw and check distance to bike   
                    pedestrian.MoveCloserToEnd();
                    pedestrian.Draw(ref spriteBatch);

                    this.PossibleRightOfWaySituationPedestrian = collisionManager.ObjectToObjectClose(TheBike.GetPosition(), pedestrian.getPosition());
                    this.BikeTooCloseToPedestrian = collisionManager.IsObjectTooCloseTo(TheBike.GetPosition(), pedestrian.getPosition(), pedestrian);
                }
            }

            if (TheBike.Enabled)
                collisionManager.checkCollisions(TheBike.GetPosition(), (currentLevel == "Help" || currentLevel == "-1" || currentLevel == "0" || currentLevel == "1"));

            if (collisionManager.Delay != null)
            {
                if (GTimer == null)
                {
                    GTimer = new GUITimer(this.SingularContent);
                    GTimer.Load(TheBike.GetPosition());
                    GTimer.Active = true;
                    GTimer.Percentage = 0;
                    GTimer.SetPosition(TheBike.GetPosition());
                }
                else
                {
                    GTimer.Draw(ref spriteBatch);
                    GTimer.GrowTimer();
                    GTimer.SetPosition(TheBike.GetPosition());
                }
            }
            else
                GTimer = null;

            if (collisionManager.CurrentExitCollidable != Rectangle.Empty)
            {
                Int32 c = Int32.Parse(currentLevel);

                c++;
                Load(c.ToString());
                collisionManager.CurrentExitCollidable = Rectangle.Empty;
                collisionManager.CurrentEntranceCollidable = Rectangle.Empty;
            }

            if (collisionManager.CurrentEntranceCollidable != Rectangle.Empty)
            {
                //doe iets                
            }

            //finally, check if we need to dispose things
            DisposeContent();
        }

        public void EnableVehicles()
        {
            foreach (Vehicle vehicle in Vehicles)
            {
                vehicle.Enabled = true;
                vehicle.Moving = true;
            }
        }

        private void ContinueMovingVehicles(object state)
        {
            foreach (Vehicle vehicle in ((Level)state).Vehicles)
            {
                vehicle.setPause(vehicle.getEnd());
                vehicle.Enabled = true;
                vehicle.Moving = true;
            }

            //((Level)state).Delay.Dispose();
            //((Level)state).Delay = null;
        }

        public void ContinueMovingVehicles()
        {
            foreach (Vehicle vehicle in Vehicles)
            {
                vehicle.setPause(vehicle.getEnd());
                vehicle.Enabled = true;
                vehicle.Moving = true;
            }
        }

        private void ContinueMovingPedestrians(object state)
        {
            foreach (Pedestrian pedestrian in Pedestrians)
            {
                pedestrian.Moving = true;
            }

            //((Level)state).Delay.Dispose();
            //((Level)state).Delay = null;
        }

        public void ContinueMovingPedestrians()
        {
            foreach (Pedestrian pedestrian in Pedestrians)
            {
                pedestrian.Moving = true;
            }
        }

        int tolx = 20;
        int toly = 0;

        internal void handleMouseState()
        {
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (TheBike.Enabled)
                {
                    Vector2 BikePos = TheBike.GetPosition();
                    if (ms.X < BikePos.X + (100 + tolx) && ms.X > BikePos.X - (25 + tolx))
                        TheBike.MoveTo(ms.X, ms.Y);

                    EnableVehicles();
                }
                else
                {
                    //initial
                    TheBike.Enabled = true;
                    TheBike.MoveTo(ms.X, ms.Y);

                    EnableVehicles();
                }
            }
        }

        private void DisposeContent()
        {
            foreach (Vehicle v in Vehicles)
            {
                if (v.DisposeMe)
                {
                    v.Dispose();
                }
            }
        }

    }
}
