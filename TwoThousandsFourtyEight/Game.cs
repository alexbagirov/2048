using System;

namespace TwoThousandsFourtyEight
{
    public class Game
    {
        public Cell[,] Map;
        public readonly int Width;
        public readonly int Height;
        
        public Game(int width, int height)
        {
            Width = width;
            Height = height;
            
            Initialize();
            AddCell();
            AddCell();
            
        }

        public void AddCell()
        {
            var random = new Random();
            while (true)
            {
                var x = random.Next(0, Width - 1);
                var y = random.Next(0, Height - 1);
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