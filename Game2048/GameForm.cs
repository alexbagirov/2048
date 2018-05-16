using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace Game2048
{
    public partial class GameForm : Form
    {
        public GameForm(Game game)
        {
            Size = Screen.PrimaryScreen.WorkingArea.Size;
            MinimumSize = Size;
            MaximumSize = Size;
              
            var table = new TableLayoutPanel();
            table.RowStyles.Clear();
            table.Dock = DockStyle.Fill;
            
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, Screen.PrimaryScreen.WorkingArea.Width / 3 + 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Screen.PrimaryScreen.WorkingArea.Width / 3 + 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 1, 0);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 0, 0);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 0, 2);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 0, 1);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 2, 1);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 1, 2);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 2, 0);
            table.Controls.Add(new Panel{Dock = DockStyle.Fill}, 2, 2);

            var gameField = new TableLayoutPanel();
            gameField.BackColor = ColorTranslator.FromHtml("#776e65");
            gameField.RowStyles.Clear();
            gameField.Dock = DockStyle.Fill;
            for (var i = 0; i < game.Height;i++)
                gameField.RowStyles.Add(new RowStyle(SizeType.Absolute, table.RowStyles[1].Height/ game.Height));
            for (var i = 0; i < game.Width;i++)
                gameField.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, table.ColumnStyles[1].Width / game.Width));
            table.Controls.Add(gameField , 1, 1);
            /*var lol = new Panel{Left = 50, Top = 50, Size = new Size(50, 50), BackColor = Color.Black};
            Controls.Add(lol);*/
            Controls.Add(table);

            /*KeyPress += (sender, args) =>
            {
                Console.WriteLine(args.KeyChar);
                for (var i = 0; i < 300; i++)
                {
                    lol.Location = new Point(lol.Location.X + 1, lol.Location.Y);
                    Thread.Sleep(2);
                    Refresh();
                    lol.Capture = false;
                }
            };*/
            var panels = new Control[game.Width, game.Height];
            for(var i = 0; i<game.Height;i++)
                for (var j = 0; j < game.Width; j++)
                {
                    panels[j, i] = new Panel { Dock = DockStyle.Fill, BackColor = game[j, i].Color };
                    gameField.Controls.Add(panels[j,i], j, i);
                }
            
            KeyPress += (sender, args) =>
            {
                var key = args.KeyChar;
                switch (key)
                {
                    case 'w':
                        game.TryMove(Direction.Up);
                        break;
                    case 'a':
                        game.TryMove(Direction.Left);
                        break;
                    case 's':
                        game.TryMove(Direction.Down);
                        break;
                    case 'd':
                        game.TryMove(Direction.Right);
                        break;
                    default:
                        return;
                }
                game.AddRandomTile();
                for (var k = 0; k < 300; k++)
                {
                    for(var i = 0; i<game.Height;i++)
                    for (var j = 0; j < game.Width; j++)
                        panels[j,i].BackColor = game[j, i].Color;
                    Invalidate();
                }
            };
            
            BackColor = ColorTranslator.FromHtml("#faf8ef");
        }
        
    }
}