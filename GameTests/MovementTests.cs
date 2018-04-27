using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game2048;

namespace TryMoveTests
{
    [TestClass]
    public class TryMoveTests
    {
        [TestMethod]
        public void TestMoveUp()
        {
            var game = BuildGameMap(new int[,]
            {
                {0,0,0,0},
                {0,2,0,2},
                {0,2,0,0},
                {0,0,0,0}
            });
            game.TryMove(Direction.Up);
            var result = new int[,]
            {
                {0,4,0,2},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [TestMethod]
        public void TestMoveDown()
        {
            var game = BuildGameMap(new int[,]
            {
                {0,0,0,0},
                {0,2,0,2},
                {0,2,0,0},
                {0,0,0,0}
            });
            game.TryMove(Direction.Down);
            var result = new int[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {0,4,0,2}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [TestMethod]
        public void TestMoveLeft()
        {
            var game = BuildGameMap(new int[,]
            {
                {0,0,0,0},
                {0,2,2,0},
                {0,0,0,0},
                {0,0,0,2}
            });
            game.TryMove(Direction.Left);
            var result = new int[,]
            {
                {0,0,0,0},
                {4,0,0,0},
                {0,0,0,0},
                {2,0,0,0}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [TestMethod]
        public void TestMoveRight()
        {
            var game = BuildGameMap(new int[,]
            {
                {0,0,0,0},
                {0,2,2,0},
                {0,0,0,0},
                {2,0,0,0}
            });
            game.TryMove(Direction.Right);
            var result = new int[,]
            {
                {0,0,0,0},
                {0,0,0,4},
                {0,0,0,0},
                {0,0,0,2}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [TestMethod]
        public void TestOneTileMergeOnlyOnce()
        {
            var game = BuildGameMap(new int[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {4,0,2,2}
            });
            game.TryMove(Direction.Right);
            var result = new int[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,4,4}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [TestMethod]
        public void TestTilesDidntMove()
        {
            var game = BuildGameMap(new int[,]
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
        }

        [TestMethod]
        public void TestMassiveTileMerges()
        {
            var game = BuildGameMap(new int[,]
            {
                {2,0,64,128},
                {2,16,64,0},
                {4,16,32,0},
                {4,8,32,128}
            });
            game.TryMove(Direction.Down);
            var result = new int[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {4,32,128,0},
                {8,8,64,256}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [TestMethod]
        public void TestSeveraMoves()
        {
            var game = BuildGameMap(new int[,]
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
            var result = new int[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {128,0,0,0},
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        public Game BuildGameMap(int[,] mapToBuild)
        {
            var width = mapToBuild.GetLength(0);
            var height = mapToBuild.GetLength(1);
            var game = new Game(width, height);
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    game.Map.AddTile(new Point(x, y), mapToBuild[y,x]);
            return game;
        }

        public bool ValuesAreEqual(Game game, int[,] result)
        {
            for (var y = 0; y < game.Map.Height; y++)
                for (var x = 0; x < game.Map.Width; x++)
                    if (game.Map[x, y].Value != result[y, x])
                        return false;
            return true;
        }
    }
}
