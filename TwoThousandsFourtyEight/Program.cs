using System.Windows.Forms;
using System;

namespace TwoThousandsFourtyEight
{
    internal class Program
    {

        public static void Main()
        {
            var game = new Game(4, 4);
            while (true)
            {
                game.Print();
                var movement = Console.ReadLine();
                game.Move(movement);
                game.AddCell();
                Console.Clear();
                Console.SetCursorPosition(0, 0);
            }
            /*var form = new GameForm(game);
            Application.Run(form);*/
        }
    }
}