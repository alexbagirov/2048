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
                    if (game.TryMove(Console.ReadLine()))
                        break;
                    
                }
                game.AddTile();
                Console.Clear();
                Console.SetCursorPosition(0, 0);
            }
            /*var form = new GameForm(game);
            Application.Run(form);*/
        }
    }
}