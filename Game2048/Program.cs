using System.Windows.Forms;

namespace Game2048
{
    internal static class Program
    {
        public static void Main()
        {
            var form = new GameForm(2, 2);
            Application.Run(form);
        }
    }
}