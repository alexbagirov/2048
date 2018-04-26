using System.Windows.Forms;
using System;

namespace Game2048
{
    internal class Program
    {

        public static void Main()
        {
            var game = new Game(4, 4);
            while (true)
            {
                game.Print();

                while (true)
                {
                    var move = Console.ReadLine();
                    var moved = false;

                    switch (move)
                    {
                        case "W":
                            moved = game.TryMove(Direction.Up);
                            break;
                        case "A":
                            moved = game.TryMove(Direction.Left);
                            break;
                        case "S":
                            moved = game.TryMove(Direction.Down);
                            break;
                        case "D":
                            moved = game.TryMove(Direction.Right);
                            break;
                    }
                    
                    if (moved)
                        break;

                }
                game.NewTile();
                Console.Clear();
                Console.SetCursorPosition(0, 0);
            }
            /*var form = new GameForm(game);
            Application.Run(form);*/
        }
    }
}