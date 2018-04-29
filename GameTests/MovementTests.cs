using System.Drawing;
using NUnit.Framework;
using Game2048;

namespace MovementTests
{
    [TestFixture]
    public class TryMoveTests
    {
        [Test]
        public void TestMoveUp()
        {
            var game = BuildGameMap(new[,]
            {
                {0,0,0,0},
                {0,2,0,2},
                {0,2,0,0},
                {0,0,0,0}
            });
            game.TryMove(Direction.Up);
            var result = new[,]
            {
                {0,4,0,2},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(4, game.Score);
        }

        [Test]
        public void TestMoveDown()
        {
            var game = BuildGameMap(new[,]
            {
                {0,0,0,0},
                {0,2,0,2},
                {0,2,0,0},
                {0,0,0,0}
            });
            game.TryMove(Direction.Down);
            var result = new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {0,4,0,2}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(4, game.Score);
        }

        [Test]
        public void TestMoveLeft()
        {
            var game = BuildGameMap(new[,]
            {
                {0,0,0,0},
                {0,2,2,0},
                {0,0,0,0},
                {0,0,0,2}
            });
            game.TryMove(Direction.Left);
            var result = new[,]
            {
                {0,0,0,0},
                {4,0,0,0},
                {0,0,0,0},
                {2,0,0,0}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(4, game.Score);
        }

        [Test]
        public void TestMoveRight()
        {
            var game = BuildGameMap(new[,]
            {
                {0,0,0,0},
                {0,2,2,0},
                {0,0,0,0},
                {2,0,0,0}
            });
            game.TryMove(Direction.Right);
            var result = new[,]
            {
                {0,0,0,0},
                {0,0,0,4},
                {0,0,0,0},
                {0,0,0,2}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(4, game.Score);
        }

        [Test]
        public void TestOneTileMergeOnlyOnce()
        {
            var game = BuildGameMap(new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {4,0,2,2}
            });
            game.TryMove(Direction.Right);
            var result = new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,4,4}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(4, game.Score);
        }

        [Test]
        public void TestTilesDidntMove()
        {
            var game = BuildGameMap(new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {2,4,8,16}
            });
            Assert.IsFalse(game.TryMove(Direction.Right));
            Assert.IsFalse(game.TryMove(Direction.Left));
            Assert.IsFalse(game.TryMove(Direction.Down));
            Assert.IsTrue(game.TryMove(Direction.Up));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestMassiveTileMerges()
        {
            var game = BuildGameMap(new[,]
            {
                {2,0,64,128},
                {2,16,64,0},
                {4,16,32,0},
                {4,8,32,128}
            });
            game.TryMove(Direction.Down);
            var result = new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {4,32,128,0},
                {8,8,64,256}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(492, game.Score);
        }

        [Test]
        public void TestSeveraMoves()
        {
            var game = BuildGameMap(new[,]
            {
                {2,64,0,0},
                {2,32,0,0},
                {0,16,0,0},
                {4,8,0,0}
            });
            game.TryMove(Direction.Down);
            game.TryMove(Direction.Down);
            game.TryMove(Direction.Right);
            game.TryMove(Direction.Up);
            game.TryMove(Direction.Up);
            game.TryMove(Direction.Left);
            game.TryMove(Direction.Down);
            var result = new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {128,0,0,0},
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
            Assert.AreEqual(252, game.Score);
        }

        [Test]
        public void TestEmptyTiles()
        {
            var game = BuildGameMap(new[,]
            {
                {2, 0, 4},
                {2, 0, 4}
            });
            Assert.True(game.Map.EmptyPositions.Contains(new Point(1, 0)));
            Assert.True(game.Map.EmptyPositions.Contains(new Point(1, 1)));
            game.TryMove(Direction.Down);
            Assert.True(game.Map.EmptyPositions.Contains(new Point(1, 0)));
            Assert.True(game.Map.EmptyPositions.Contains(new Point(1, 1)));
            Assert.True(game.Map.EmptyPositions.Contains(new Point(0, 0)));
            Assert.True(game.Map.EmptyPositions.Contains(new Point(2, 0)));
        }

        public static Game BuildGameMap(int[,] mapToBuild)
        {
            var width = mapToBuild.GetLength(1);
            var height = mapToBuild.GetLength(0);
            var game = new Game(width, height);
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    game.Map.AddTile(new Point(x, y), mapToBuild[y,x]);
            return game;
        }

        public static bool ValuesAreEqual(Game game, int[,] result)
        {
            for (var y = 0; y < game.Map.Height; y++)
                for (var x = 0; x < game.Map.Width; x++)
                    if (game.Map[x, y].Value != result[y, x])
                        return false;
            return true;
        }
    }
}
