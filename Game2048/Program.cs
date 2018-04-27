using System.Windows.Forms;
using System;

namespace Game2048
{
    internal class Program
    {
        public static void Main()
        {
            var game = new Game(2, 2);
            while (true)
            {
                game.Print();

                while (true)
                {
                    var move = DirectionParser.Parse(Console.ReadLine());
                    var moved = false;
                    if (move != null)
                        moved = game.TryMove((Direction) move);

                    if (moved)
                        break;
                }

                game.AddNewTile();
                
                if (game.HasEnded())
                {
                    Console.Clear();
                    Console.WriteLine("GAME ENDED");
                    game.Print();
                    break;
                }

                Console.Clear();
                Console.SetCursorPosition(0, 0);
            }
            /*var form = new GameForm(game);
            Application.Run(form);*/
        }
    }
}