using System.Windows.Forms;

namespace TwoThousandsFourtyEight
{
    internal class Program
    {
        public static void Main()
        {
            var game = new Game(4, 4);
            
            var form = new Form();
            Application.Run(form);
        }
    }
}