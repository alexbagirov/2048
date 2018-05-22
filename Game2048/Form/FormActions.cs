using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    public sealed partial class GameForm
    {
        private static Control[,] StartGame(Game game, Control field, TableLayoutPanel head)
        {
            var labels = new Control[game.Width, game.Height];
            field.Controls.Clear();
            head.Controls[1].Text = "0";
            RectSize = new Size(field.Size.Height / game.Height - 15, field.Size.Height / game.Height - 15);
            Distance = (field.Size.Width - game.Width * RectSize.Width) / (game.Width + 1);
            for (var i = 0; i < game.Height; i++)
            for (var j = 0; j < game.Width; j++)
            {
                var location = new Point(RectSize.Width * j + (j + 1) * Distance, 
                    RectSize.Height * i + (i + 1) * Distance);
                labels[j, i] = CreateLabel(
                    game[j, i].Color, 
                    game[j, i].Value == 0 ? "" : game[j, i].Value.ToString(), 
                    location);
                field.Controls.Add(labels[j, i]);
            }

            return labels;
        }

        private void UpdateColors(Game game, Control[,] labels)
        {
            for (var i = 0; i < game.Height; i++)
            {
                for (var j = 0; j < game.Width; j++)
                {
                    labels[j, i].Text = game[j, i].Value == 0 ? "" : game[j, i].Value.ToString();
                    labels[j, i].BackColor = game[j, i].Color;
                }
            }
            Invalidate();
        }

        private void MakeMove(Game game, Control gameField, Control[,] labels, TableLayoutPanel head, Keys key)
        {
            var moved = ParseKey(game, labels, key);

            if (!moved)
                return;
            game.AddRandomTile();
            head.Controls[1].Text = game.Score.ToString();

            Animate(game, gameField, labels);
            UpdateColors(game, labels);
            
            if (game.HasEnded())
            {
                ShowMessage($"Game is Over. Your score is {game.Score.ToString()}");
                game = new Game(game.Width, game.Height);
                labels = StartGame(game, gameField, head);
                UpdateColors(game, labels);
            }
        }

        private bool ParseKey(Game game, Control[,] labels, Keys key)
        {
            var moved = false;
            switch (key)
            {
                case Keys.W:
                    moved = game.MakeMove(Direction.Up);
                    break;
                case Keys.A:
                    moved = game.MakeMove(Direction.Left);
                    break;
                case Keys.S:
                    moved = game.MakeMove(Direction.Down);
                    break;
                case Keys.D:
                    moved = game.MakeMove(Direction.Right);
                    break;
                case Keys.Q:
                    game.Undo();
                    UpdateColors(game, labels);
                    break;
                default:
                    return false;
            }
            return moved;
        }
    }
}