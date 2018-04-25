using System;
using System.Text;
using System.Drawing;
using System.Linq;

namespace TwoThousandsFourtyEight
{
    public class Game
    {
        public Cell[,] Map;
        public readonly int Width;
        public readonly int Height;
        public bool IsRunning = true;

        public Game(int width, int height)
        {
            Width = width;
            Height = height;
            Initialize();
            AddCell();
            AddCell();
        }

        public void Move( string movement)
        {
            var xRange =  Enumerable.Range(0,0);
            var yRange = Enumerable.Range(0, 0);
            var movePoint = new Point(0, 0);
            if (movement == "W")
            {
                movePoint = new Point(0,-1);
                xRange = Enumerable.Range(0, Width);
                yRange = Enumerable.Range(0, Height);
            }
            else if (movement == "A")
            {
                movePoint = new Point(-1, 0);
                xRange = Enumerable.Range(0, Width);
                yRange = Enumerable.Range(0, Height);
            }
            else if (movement == "S")
            {
                movePoint = new Point(0,1);
                xRange = Enumerable.Range(0, Width);
                yRange = Enumerable.Range(0, Height).Reverse();
            }
            else
            {
                movePoint = new Point(1, 0);
                xRange = Enumerable.Range(0,Width).Reverse();
                yRange = Enumerable.Range(0, Height);
            }

            foreach (var y in yRange)
            {
                foreach (var x in xRange)
                {
                    if (Map[x, y].Value != 0)
                    {
                        var curPos = new Point(x, y);
                        while(true)
                        {
                            var nextPos= new Point(curPos.X + movePoint.X, curPos.Y + movePoint.Y);
                            if (nextPos.X >= Width || nextPos.Y >= Height || nextPos.X < 0 || nextPos.Y < 0)
                                break;

                            if (Map[nextPos.X, nextPos.Y].Value!=0)
                            {
                                if (Map[nextPos.X, nextPos.Y].Value == Map[curPos.X, curPos.Y].Value)
                                {
                                    Map[nextPos.X, nextPos.Y].Value += Map[curPos.X, curPos.Y].Value;
                                    Map[curPos.X, curPos.Y].Value = 0;
                                    curPos = nextPos;
                                }
                                break;
                            }
                            else
                            {
                                Map[nextPos.X, nextPos.Y].Value += Map[curPos.X, curPos.Y].Value;
                                Map[curPos.X, curPos.Y].Value = 0;
                                curPos = nextPos;
                            }
                        }
                    }
                }
            }
        }

        public void Print()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                    Console.Write(Map[x, y].Value.ToString().PadRight(4));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void AddCell()
        {
            var random = new Random();
            while (true)
            {
                var x = random.Next(0, Width);
                var y = random.Next(0, Height);
                if (Map[x, y].Value == 0)
                {
                    Map[x, y] = new Cell(2);
                    break;   
                }
            }
        }

        private void Initialize()
        {
            Map = new Cell[Width, Height];
            for (var i = 0; i < Width; i++)
            for (var j = 0; j < Width; j++)
                Map[i, j] = new Cell(0);
        }
    }
}