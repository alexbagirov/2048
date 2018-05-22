using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    public sealed partial class GameForm : Form
    {
        public static Size RectSize { get; private set; }
        public static int Distance { get; private set; }
        
        public GameForm(int width, int height)
        {
            var game = new Game(width, height);

            WindowState = FormWindowState.Maximized;
            MaximizeBox = false;
            BackColor = ColorTranslator.FromHtml("#faf8ef");
            KeyPreview = true;

            var table = CreateTable();
            var head = CreateHead();

            table.Controls.Add(head, 1, 0);

            var gameField = new Label
            {
                Size = new Size(469, 469),
                BackColor = ColorTranslator.FromHtml("#bbada0"),
            };
            table.Controls.Add(gameField , 1, 1);
            
            Controls.Add(table);

            var labels = StartGame(game, gameField, head);

            KeyDown += (sender, args) => MakeMove(game, gameField, labels, head, args.KeyData);
            head.Controls[3].Click += (sender, args) =>
            {
                game = new Game(game.Width, game.Height);
                labels = StartGame(game, gameField, head);
                UpdateColors(game, labels);
            };
        }

        private static TableLayoutPanel CreateHead()
        {
            var head = new TableLayoutPanel {Dock = DockStyle.Fill};

            head.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            head.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            head.Controls.Add(new Label
            {
                Text = "2048",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 50, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#776e65"),
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);
            head.Controls.Add(new Label
            {
                Text = "0",
                ForeColor = ColorTranslator.FromHtml("#776e65"),
                Font = new Font("Arial", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill
            }, 1, 0);
            head.Controls.Add(new Label
            {
                Text = "Join the numbers and get to the 2048 tile!",
                Font = new Font("Arial", 15, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#776e65"),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            }, 0, 1);
            head.Controls.Add(new Button
            {
                Text = "New Game",
                Font = new Font("Arial", 13, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#8f7a66"),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = {BorderSize = 0},
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Padding = new Padding(4)
            }, 1, 1);
            return head;
        }

        private static TableLayoutPanel CreateTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, Screen.PrimaryScreen.WorkingArea.Width / 3 + 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Screen.PrimaryScreen.WorkingArea.Width / 3 + 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    if (i != 1 || j != 0 && j != 1)
                        table.Controls.Add(new Panel {Dock = DockStyle.Fill}, i, j);
            
            return table;
        }

        private static Label CreateLabel(Color color, string text, Point location)
        {
            return new Label
            {
                Size = RectSize,
                BackColor = color,
                Text = text,
                Font = new Font("Arial", 30, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ColorTranslator.FromHtml("#776e65"),
                Margin = new Padding(0),
                Location = location
            };
        }

        private static void ShowMessage(string message)
        {
            MessageBox.Show(message, "", MessageBoxButtons.OK);
        }
    }
}