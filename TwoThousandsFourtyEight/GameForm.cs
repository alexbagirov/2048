using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.Paint += (sender, args) => RenderText1(args);
        }

    }
}
