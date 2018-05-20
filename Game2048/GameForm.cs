﻿using System.Drawing;
using System.Windows.Forms;
using System;

namespace Game2048
{
    public sealed partial class GameForm : Form
    {
        public GameForm(int width, int height)
        {
            var game = new Game(width, height);

            Size = Screen.PrimaryScreen.WorkingArea.Size;
            MinimumSize = Size;
            MaximumSize = Size;
            BackColor = ColorTranslator.FromHtml("#faf8ef");
            KeyPreview = true;

            var table = new TableLayoutPanel { Dock = DockStyle.Fill };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, Screen.PrimaryScreen.WorkingArea.Width / 3 + 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Screen.PrimaryScreen.WorkingArea.Width / 3 + 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 0);
            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 1);
            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 2);
            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 1, 2);
            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 2, 0);
            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 2, 1);
            table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 2, 2);

            var head = new TableLayoutPanel { Dock = DockStyle.Fill };

            head.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            head.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            head.Controls.Add(new Label
            {
                Text = "2048", Dock = DockStyle.Fill,
                Font = new Font("Arial", 50, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#776e65"),
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);
            head.Controls.Add(new Label
            {
                Text = "0", ForeColor = ColorTranslator.FromHtml("#776e65"),
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
                FlatAppearance = { BorderSize = 0 },
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Padding = new Padding(4)
            }, 1, 1);

            table.Controls.Add(head, 1, 0);

            var gameField = new Label
            {
                //Dock = DockStyle.Fill,
                Size = new Size(469, 469),
                BackColor = ColorTranslator.FromHtml("#bbada0"),
            };
            table.Controls.Add(gameField , 1, 1);
            
            Controls.Add(table);

            var labels = StartGame(game, gameField, head);

            KeyDown += (sender, args) => MakeMove(game, labels, head, args.KeyData);
            head.Controls[3].Click += (sender, args) =>
            {
                game = new Game(4, 4);

                StartGame(game, gameField, head);
                UpdateColors(game, labels);
            };
        }

        private static Control[,] StartGame(Game game, TableLayoutPanel field, TableLayoutPanel head)
        {
            var labels = new Control[game.Width, game.Height];
            field.Controls.Clear();
            head.Controls[1].Text = "0";
            var size = new Size(field.Size.Height / game.Height - 15, field.Size.Height / game.Height - 15);
            var dx = (field.Size.Width - game.Width * size.Width) / (game.Width+1);
            for (var i = 0; i < game.Height; i++)
                for (var j = 0; j < game.Width; j++)
                {
                    labels[j, i] = new Label
                    {
                        Size = size,
                        BackColor = game[j, i].Color,
                        Text = game[j, i].Value == 0 ? "" : game[j, i].Value.ToString(),
                        Font = new Font("Arial", 30, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = ColorTranslator.FromHtml("#776e65"),
                        Margin = new Padding(0),
                        Location = new Point(size.Width * j + (j + 1) * dx, size.Height * i + (i + 1) * dx)
                    };
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

        private void MakeMove(Game game, Control[,] labels, TableLayoutPanel head, Keys key)
        {
            var moved = false;
            switch (key)
            {
                case Keys.W: 
                case Keys.Up:
                    moved = game.TryMove(Direction.Up);
                    break;
                case Keys.A: 
                case Keys.Left:
                    moved = game.TryMove(Direction.Left);
                    break;
                case Keys.S: 
                case Keys.Down:
                    moved = game.TryMove(Direction.Down);
                    break;
                case Keys.D: 
                case Keys.Right:
                    moved = game.TryMove(Direction.Right);
                    break;
                case Keys.Q: 
                case Keys.Back:
                    game.Undo();
                    break;
                default:
                    return;
            }

            if (moved)
            {
                game.AddRandomTile();
                head.Controls[1].Text = game.Score.ToString();
            }
                
            UpdateColors(game, labels);
            if (game.HasEnded())
                ShowMessage(String.Format("Game is Over. Your score is {0}", game.Score.ToString()));
        }

        private void ShowMessage(string message)
        {
            var caption = "";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            var result = MessageBox.Show(message, caption, buttons);
        }
    }
}