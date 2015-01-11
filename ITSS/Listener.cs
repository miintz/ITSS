using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUIO;

namespace ITSS
{
    public class Listener : TuioListener
    {
        List<TuioCursor> CurrentCursors = new List<TuioCursor>();

        private TuioCursor firstCursor;

        public void addTuioObject(TuioObject tobj)
        {
            Console.WriteLine("add obj " + tobj.SymbolID + " " + tobj.SessionID + " " + tobj.X + " " + tobj.Y + " " + tobj.Angle);
        }

        public void updateTuioObject(TuioObject tobj)
        {
           // Console.WriteLine("set obj " + tobj.SymbolID + " " + tobj.SessionID + " " + tobj.X + " " + tobj.Y + " " + tobj.Angle + " " + tobj.MotionSpeed + " " + tobj.RotationSpeed + " " + tobj.MotionAccel + " " + tobj.RotationAccel);
        }

        public void removeTuioObject(TuioObject tobj)
        {
           // Console.WriteLine("del obj " + tobj.SymbolID + " " + tobj.SessionID);
        }

        public void addTuioCursor(TuioCursor tcur)
        {
            if (CurrentCursors.Count == 0)
                firstCursor = tcur; //zet de cursor als de lijst leeg is om null refs te voorkomen

            if (CurrentCursors.Where(p => p.CursorID == tcur.CursorID).ToList().Count == 0)
            {
                Console.WriteLine("add cur " + tcur.CursorID + " (" + tcur.SessionID + ") " + tcur.X + " " + tcur.Y);
                CurrentCursors.Add(tcur);
            }
            else
                Console.WriteLine("Cursor already added");
        }

        public void updateTuioCursor(TuioCursor tcur)
        {
          //  Console.WriteLine("set cur " + tcur.CursorID + " (" + tcur.SessionID + ") " + tcur.X + " " + tcur.Y + " " + tcur.MotionSpeed + " " + tcur.MotionAccel);
        }

        public void addTuioBlob(TuioBlob tblob)
        { 
        
        }

        public void removeTuioCursor(TuioCursor tcur)
        {
            int index = CurrentCursors.FindIndex(p => p.CursorID == tcur.CursorID);
            //als de tcur de id van de firstCursor heeft moet deze geupdate worden
            if(tcur.CursorID == firstCursor.CursorID)            
            {
                if (index == CurrentCursors.Count - 1)
                {
                    //pak de vorige
                    int newindex = index - 1;
                    if (newindex != -1)
                        firstCursor = CurrentCursors[newindex];
                    else
                        firstCursor = null; //er zit niks meer in de lijst, dus geen cursor beschikbaar
                }
                else
                { 
                    //pak de volgende
                    int newindex = index + 1;
                    firstCursor = CurrentCursors[newindex];
                }
            }

            //nu uit lijst halen met lambda expressions                       
            CurrentCursors.RemoveAt(index);
            //Console.WriteLine("del cur " + tcur.CursorID + " (" + tcur.SessionID + ")");
        }

        public void updateTuioBlob(TuioBlob tblb)
        {
          //  Console.WriteLine("set blb " + tblb.BlobID + " (" + tblb.SessionID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Angle + " " + tblb.Width + " " + tblb.Height + " " + tblb.Area + " " + tblb.MotionSpeed + " " + tblb.RotationSpeed + " " + tblb.MotionAccel + " " + tblb.RotationAccel);
        }

        public void removeTuioBlob(TuioBlob tblb)
        {
           // Console.WriteLine("del blb " + tblb.BlobID + " (" + tblb.SessionID + ")");
        }

        public void refresh(TuioTime frameTime)
        {
            //Console.WriteLine("refresh " + frameTime.getTotalMilliseconds());
        }

        public Vector2 getFirstBlobPos()
        {
            //ik heb dus een berg blobs en ik wil de eerste in die lijst
            
            /*
             *  Resolution.Y = 900;
             *  Resolution.X = 1440;
             */

            if (firstCursor != null)
            {
                Console.WriteLine(900 * firstCursor.X + " " + 1440 * firstCursor.Y);
                return new Vector2(900 * firstCursor.X, 1440 * firstCursor.Y); //static...
            }
            
            return Vector2.One;
        }
        

        public void Construct(String[] argv)
        {            
            TuioClient client = null;

            switch (argv.Length)
            {
                case 1:
                    int port = 0;
                    port = int.Parse(argv[0], null);
                    if (port > 0) client = new TuioClient(port);
                    break;
                case 0:
                    client = new TuioClient();
                    break;
            }

            if (client != null)
            {
                client.addTuioListener(this);
                client.connect();
                Console.WriteLine("listening to TUIO messages at port " + client.getPort());

            }
            else Console.WriteLine("usage: java TuioDump [port]");
        }
    }
}
