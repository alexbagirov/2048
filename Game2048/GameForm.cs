using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    public partial class GameForm : Form
    {
        private void RenderText1(PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, "AAAAAAAA", Font,
                new Point(10, 10), SystemColors.ControlText);
        }

        public GameForm(Game game)
        {
            InitializeComponent();
            Paint += (sender, args) => RenderText1(args);
        }
    }
}