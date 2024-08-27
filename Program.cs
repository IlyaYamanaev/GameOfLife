using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public class Size
    {
        public int X, Y;
        public Size(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public class Сolony
    {
        public Dictionary<int, Dictionary<int, byte>> Data; 
        private Dictionary<int, Dictionary<int, byte>> NextData;
        private struct RulesOfWorld
        {
            static public Size worldBorders;      
            static public byte bordersType;                      
            static public List<byte> B;
            static public List<byte> S;
        }
        public Сolony(int x, int y, string b, string s, byte BordersType)
        {
            Data = new Dictionary<int, Dictionary<int, byte>>();
            NextData = new Dictionary<int, Dictionary<int, byte>>();
            RulesOfWorld.worldBorders = new Size(x, y);
            RulesOfWorld.bordersType = BordersType;
            RulesOfWorld.B = new List<byte>();
            RulesOfWorld.S = new List<byte>();
            SetRules(b, s);
        }

        public void SetRules(string b, string s)
        { 
            RulesOfWorld.B.Clear();
            RulesOfWorld.S.Clear();
            if (b != "")
                foreach (char c in b)
                    if (!RulesOfWorld.B.Contains((byte)(c - '0')))
                        RulesOfWorld.B.Add((byte)(c - '0'));      
            if (s != "")                    
                foreach (char c in s)                
                    if (!RulesOfWorld.S.Contains((byte)(c - '0')))                    
                        RulesOfWorld.S.Add((byte)(c - '0'));             
        }
        public void NextGen()
        { 
            foreach (var x in Data)
            {
                foreach (var y in x.Value)
                {
                    if (LiveOut(x.Key, y.Key)) SetInNextData(x.Key, y.Key);
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            int nx, ny;
                            bool gri = false, grj = false;
                            nx = (int)(x.Key + a);
                            ny = (int)(y.Key + b);
                            if (x.Key <= 0 && a == -1)
                            {
                                gri = true;
                                nx = RulesOfWorld.worldBorders.X - 1;
                            }
                            if (x.Key >= RulesOfWorld.worldBorders.X - 1 && a == 1)
                            {
                                gri = true;
                                nx = 0;
                            }
                            if (y.Key <= 0 && b == -1)
                            {
                                grj = true;
                                ny = RulesOfWorld.worldBorders.Y - 1;
                            }
                            if (y.Key >= RulesOfWorld.worldBorders.Y - 1 && b == 1)
                            {
                                grj = true;
                                ny = 0;
                            }
                            if (!(RulesOfWorld.bordersType == 1 && (gri == true || grj == true)) && LiveOut(nx, ny)) 
                                SetInNextData(nx, ny);
                        }
                    }
                }
            }
            Data.Clear();
            foreach (var x in NextData)
            {
                if (!Data.ContainsKey(x.Key)) Data.Add(x.Key, new Dictionary<int, byte>());
                foreach (var y in x.Value)
                {
                    Data[x.Key].Add(y.Key, y.Value);
                }
            }
            NextData.Clear();
        }
        private bool LiveOut(int x, int y) 
        {                                       
            byte CountOfNeighbors = 0;          
            for (int a = -1; a < 2; a++)
            {
                for (int b = -1; b < 2; b++)
                {
                    if (a == 0 && b == 0)
                    {
                        continue;
                    }
                    int nx, ny;
                    bool gri = false, grj = false;
                    nx = (int)(x + a);
                    ny = (int)(y + b);
                    if (x <= 0 && a == -1)
                    {
                        gri = true;
                        nx = RulesOfWorld.worldBorders.X - 1;
                    }
                    if (x >= RulesOfWorld.worldBorders.X - 1 && a == 1)
                    {
                        gri = true;
                        nx = 0;
                    }
                    if (y <= 0 && b == -1)
                    {
                        grj = true;
                        ny = RulesOfWorld.worldBorders.Y - 1;
                    }
                    if (y >= RulesOfWorld.worldBorders.Y - 1 && b == 1)
                    {
                        grj = true;
                        ny = 0;
                    }
                    if (RulesOfWorld.bordersType == 1 || RulesOfWorld.bordersType == 2)
                    {
                        if (gri == false && grj == false && Convert.ToBoolean(Get(nx, ny)))
                        {
                            if (x == 2 && y == 1)
                            {
                                Console.WriteLine(nx + " " + ny);
                            }
                            CountOfNeighbors += 1;
                        }
                    }
                    if (RulesOfWorld.bordersType == 0)
                    {
                        if (Convert.ToBoolean(Get(nx, ny))) CountOfNeighbors += 1;
                    }

                }
            }
            if (!Convert.ToBoolean(Get(x, y)))
            {
                return RulesOfWorld.B.Contains(CountOfNeighbors);
            }
            else
            {
                return RulesOfWorld.S.Contains(CountOfNeighbors);
            }
        }
        private void SetInNextData(int x, int y)
        {
            if (!NextData.ContainsKey(x))
            {
                NextData.Add(x, new Dictionary<int, byte>());
            }
            if (!NextData[x].ContainsKey(y))
            {
                NextData[x].Add(y, 1);
            }
        }


        public void Clear()
        {
            Data.Clear();
        }
        public void Set(int x, int y)
        {
            if (x >= RulesOfWorld.worldBorders.X || y >= RulesOfWorld.worldBorders.Y || x < 0 || y < 0)
            {
                return;
            }
            if (!Data.ContainsKey(x))
            {
                Data.Add(x, new Dictionary<int, byte>());
            }
            if (!Data[x].ContainsKey(y))
            {
                Data[x].Add(y, 1);
            }
            else
            {
                Data[x].Remove(y);
            }
        }
        public byte Get(int x, int y)
        { //
            if (Data.ContainsKey(x) && Data[x].ContainsKey(y)) return Data[x][y];
            return 0;
        }
        public string GetB
        {
            get{
                string res = "";
                foreach (byte c in RulesOfWorld.B) res += c;
                return res;
            }
        }
        public string GetS
        {
            get
            {
                string res = "";
                foreach (byte c in RulesOfWorld.S) res += c;
                return res;
            }
           
        }
        public void SetSize(int x, int y)
        {
            RulesOfWorld.worldBorders = new Size(x, y);
        }
        public int Weight
        {
            get { return RulesOfWorld.worldBorders.X; }            
        }
        public int Hight
        {
            get { return RulesOfWorld.worldBorders.Y; }
            
        }
        public int Type
        {
            set { RulesOfWorld.bordersType = (byte)value; }
            get { return RulesOfWorld.bordersType; }
        }    

    }
}