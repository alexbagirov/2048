using System.Drawing;
using NUnit.Framework;
using Game2048;
using System.Collections.Generic;

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
            var expectedTransitions = new List<Transition>
            {
                new Transition(new Point(1,1),new Point(1,0), 2, false),
                new Transition(new Point(1,2), new Point(1, 0), 2, true),
                new Transition(new Point(3,1),new Point(3,0), 2, false)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions, expectedTransitions));
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
            var expectedTransitions = new List<Transition>
            {
                new Transition(new Point(1,1),new Point(1,3), 2, false),
                new Transition(new Point(1,2), new Point(1, 3), 2, true),
                new Transition(new Point(3,1),new Point(3,3), 2, false)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions, expectedTransitions));
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
            var expectedTransitions = new List<Transition>
            {
                new Transition(new Point(1,1),new Point(0,1), 2, false),
                new Transition(new Point(2,1), new Point(0,1), 2, true),
                new Transition(new Point(3,3),new Point(0,3), 2, false)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions, expectedTransitions));
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
            var expectedTransitions = new List<Transition>
            {
                new Transition(new Point(1,1),new Point(3,1), 2, false),
                new Transition(new Point(2,1), new Point(3,1), 2, true),
                new Transition(new Point(0,3),new Point(3,3), 2, false)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions, expectedTransitions));
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
            Assert.True(game.IsEmpty(new Point(1, 0)));
            Assert.True(game.IsEmpty(new Point(1, 1)));
            game.TryMove(Direction.Down);
            Assert.True(game.IsEmpty(new Point(1, 0)));
            Assert.True(game.IsEmpty(new Point(1, 1)));
            Assert.True(game.IsEmpty(new Point(0, 0)));
            Assert.True(game.IsEmpty(new Point(2, 0)));
        }

        [Test]
        public void TestMoveBackAfterDown()
        {
            var gameMap = new[,]
            {
                {2,0,64,128},
                {2,16,64,0},
                {4,16,32,0},
                {4,8,32,128}
            };

            var game = BuildGameMap(gameMap);
            game.TryMove(Direction.Down);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestMoveBackAfteUp()
        {
            var gameMap = new[,]
            {
                {2,0,64,128},
                {2,16,64,0},
                {4,16,32,0},
                {4,8,32,128}
            };

            var game = BuildGameMap(gameMap);
            game.TryMove(Direction.Up);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestMoveBackAfterRight()
        {
            var gameMap = new[,]
            {
                {4,4,2,2},
                {8,16,16,0},
                {32,32,64,64},
                {128,0,0,128}
            };

            var game = BuildGameMap(gameMap);
            game.TryMove(Direction.Right);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestMoveBackAfterLeft()
        {
            var gameMap = new[,]
            {
                {4,4,2,2},
                {8,16,16,0},
                {32,32,64,64},
                {128,0,0,128}
            };

            var game = BuildGameMap(gameMap);
            game.TryMove(Direction.Left);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        public static Game BuildGameMap(int[,] mapToBuild)
        {
            var width = mapToBuild.GetLength(1);
            var height = mapToBuild.GetLength(0);
            var game = new Game(width, height);
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    game.AddTile(new Point(x, y), mapToBuild[y, x]);
            return game;
        }

        public static bool ValuesAreEqual(Game game, int[,] result)
        {
            for (var y = 0; y < game.Height; y++)
                for (var x = 0; x < game.Width; x++)
                    if (game[x, y].Value != result[y, x])
                        return false;
            return true;
        }

        public static bool TransitionsAreEqual(List<Transition> list1,
            List<Transition> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            foreach (var element in list1)
            {
                if (!list2.Contains(element))
                    return false;
            }
            return true;
        }
    }
}
