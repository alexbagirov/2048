using System.Windows.Forms;

namespace Game2048
{
    internal class Program
    {
        public static void Main()
        {
            var game = new Game(4, 4);
            var form = new GameForm(game);
            Application.Run(form);
        }
    }
}