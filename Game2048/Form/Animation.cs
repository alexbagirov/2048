using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Game2048
{
    public sealed partial class GameForm
    {
        private static void Animate(Game game, Control field, Control[,] staticLabels)
        {
            var animations = new List<Animation>();
            var movingLabels = new Label[game.Width, game.Height];
            
            foreach (var transition in game.Transitions.Peek().Where(t => t.Condition != Condition.Appeared))
            {
                var j = transition.Start.X;
                var i = transition.Start.Y;

                staticLabels[j, i].BackColor = Tile.GetColor(0);
                staticLabels[j, i].Text = "";
                
                var location = new Point(RectSize.Width * j + (j + 1) * Distance, 
                    RectSize.Height * i + (i + 1) * Distance);
                
                movingLabels[transition.Start.X, transition.Start.Y] = CreateLabel(
                    Tile.GetColor(transition.StartValue),
                    transition.StartValue == 0 ? "" : transition.StartValue.ToString(), 
                    location);
                
                field.Controls.Add(movingLabels[j, i]);
                animations.Add(new Animation(transition));
            }

            MoveLabels(field, animations, movingLabels);

            foreach (var label in movingLabels)
                field.Controls.Remove(label);
        }

        private static void MoveLabels(Control field, List<Animation> animations, Label[,] movingLabels)
        {
            var count = 0;
            var coeficient = 1;
            if (animations.Count > 4)
                coeficient = 2;
            while (true)
            {
                if (count == animations.Count)
                    break;

                foreach (var animation in animations)
                {
                    if (animation.RemainingX == 0 && animation.RemainingY == 0)
                        continue;

                    var startX = animation.Start.X;
                    var startY = animation.Start.Y;
                    var dx = animation.RemainingX < 0 ? -coeficient : (animation.RemainingX == 0 ? 0 : coeficient);
                    var dy = animation.RemainingY < 0 ? -coeficient : (animation.RemainingY == 0 ? 0 : coeficient);

                    if (Math.Abs(animation.RemainingX) < coeficient)
                        dx = animation.RemainingX;
                    if (Math.Abs(animation.RemainingY) < coeficient)
                        dy = animation.RemainingY;
                    
                    animation.RemainingX -= dx;
                    movingLabels[startX, startY].Left += dx;
                    animation.RemainingY -= dy;
                    movingLabels[startX, startY].Top += dy;

                    movingLabels[startX, startY].BringToFront();

                    if (animation.RemainingX == 0 && animation.RemainingY == 0)
                        count++;

                    field.Update();
                }
            }
        }
    }
    
    public class Animation
    {
        public int RemainingX;
        public int RemainingY;
        public Point Start;

        public Animation(Transition transiton)
        {
            Start = transiton.Start;
            RemainingX = (transiton.Finish.X - transiton.Start.X) * (GameForm.Distance + GameForm.RectSize.Width);
            RemainingY = (transiton.Finish.Y - transiton.Start.Y) * (GameForm.Distance + GameForm.RectSize.Height);
        }
    }
}