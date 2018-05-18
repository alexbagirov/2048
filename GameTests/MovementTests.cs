using System.Drawing;
using NUnit.Framework;
using Game2048;
using System.Collections.Generic;
using System;

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
            game.MakeMove(Direction.Up);
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
                new Transition(new Point(1,1),new Point(1,0), 2, Condition.Moved),
                new Transition(new Point(1,2), new Point(1, 0), 2, Condition.Merged),
                new Transition(new Point(3,1),new Point(3,0), 2, Condition.Moved)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions.Pop(), expectedTransitions));
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
            game.MakeMove(Direction.Down);
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
                new Transition(new Point(1,1),new Point(1,3), 2, Condition.Moved),
                new Transition(new Point(1,2), new Point(1, 3), 2,Condition.Merged),
                new Transition(new Point(3,1),new Point(3,3), 2, Condition.Moved)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions.Pop(), expectedTransitions));
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
            game.MakeMove(Direction.Left);
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
                new Transition(new Point(1,1),new Point(0,1), 2, Condition.Moved),
                new Transition(new Point(2,1), new Point(0,1), 2,  Condition.Merged),
                new Transition(new Point(3,3),new Point(0,3), 2,Condition.Moved)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions.Pop(), expectedTransitions));
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
            game.MakeMove(Direction.Right);
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
                new Transition(new Point(1,1),new Point(3,1), 2, Condition.Moved),
                new Transition(new Point(2,1), new Point(3,1), 2, Condition.Merged),
                new Transition(new Point(0,3),new Point(3,3), 2, Condition.Moved)
            };
            Assert.IsTrue(TransitionsAreEqual(game.Transitions.Pop(), expectedTransitions));
        }

        [Test]
        public void TestMoves()
        {
            var game = BuildGameMap(new[,]
            {
                {0, 0},
                {0, 2}
            });
            var moves = new List<Direction>
            {
                Direction.Up, Direction.Left, Direction.Down,
                Direction.Right, Direction.Left, Direction.Up
            };
            foreach (var direction in moves)
                game.MakeMove(direction);
            moves.Reverse();
            foreach (var direction in moves)
                Assert.AreEqual(direction, game.Moves.Pop());
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
            game.MakeMove(Direction.Right);
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
            Assert.IsFalse(game.MakeMove(Direction.Right));
            Assert.IsFalse(game.MakeMove(Direction.Left));
            Assert.IsFalse(game.MakeMove(Direction.Down));
            Assert.IsTrue(game.MakeMove(Direction.Up));
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
            game.MakeMove(Direction.Down);
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
            game.MakeMove(Direction.Down);
            game.MakeMove(Direction.Down);
            game.MakeMove(Direction.Right);
            game.MakeMove(Direction.Up);
            game.MakeMove(Direction.Up);
            game.MakeMove(Direction.Left);
            game.MakeMove(Direction.Down);
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
            game.MakeMove(Direction.Down);
            Assert.True(game.IsEmpty(new Point(1, 0)));
            Assert.True(game.IsEmpty(new Point(1, 1)));
            Assert.True(game.IsEmpty(new Point(0, 0)));
            Assert.True(game.IsEmpty(new Point(2, 0)));
        }

        [Test]
        public void TestUndoAfterDown()
        {
            var gameMap = new[,]
            {
                {2,0,64,128},
                {2,16,64,0},
                {4,16,32,0},
                {4,8,32,128}
            };

            var game = BuildGameMap(gameMap);
            game.MakeMove(Direction.Down);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestUndoAfteUp()
        {
            var gameMap = new[,]
            {
                {2,0,64,128},
                {2,16,64,0},
                {4,16,32,0},
                {4,8,32,128}
            };

            var game = BuildGameMap(gameMap);
            game.MakeMove(Direction.Up);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestTryMoveWithFilledLine()
        {
            var gameMap = new[,]
           {
                {0,0,0,0},
                {0,0,0,4},
                {0,0,2,8},
                {4,16,4,2}
            };

            var game = BuildGameMap(gameMap);
            Assert.False(game.MakeMove(Direction.Right));
            Assert.True(game.MakeMove(Direction.Left));
            var result = new[,]
            {
                {0,0,0,0},
                {4,0,0,0},
                {2,8,0,0},
                {4,16,4,2}
            };
            Assert.IsTrue(ValuesAreEqual(game, result));
        }

        [Test]
        public void TestUndoAfterRight()
        {
            var gameMap = new[,]
            {
                {4,4,2,2},
                {8,16,16,0},
                {32,32,64,64},
                {128,0,0,128}
            };

            var game = BuildGameMap(gameMap);
            game.MakeMove(Direction.Right);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestUndoAfterLeft()
        {
            var gameMap = new[,]
            {
                {4,4,2,2},
                {8,16,16,0},
                {32,32,64,64},
                {128,0,0,128}
            };

            var game = BuildGameMap(gameMap);
            game.MakeMove(Direction.Left);
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestCommonUndo()
        {
            var gameMap = new[,]
            {
                {8,0,0,2},
                {4,4,2,2},
                {2,4,0,4},
                {8,4,8,8}
            };

            var game = BuildGameMap(gameMap);
            game.MakeMove(Direction.Right);
            game.AddRandomTile();
            game.MakeMove(Direction.Down);
            game.AddRandomTile();
            game.Undo();
            game.Undo();
            Assert.IsTrue(ValuesAreEqual(game, gameMap));
            Assert.AreEqual(0, game.Score);
        }

        [Test]
        public void TestMassiveRandomUndo()
        {
            var gameMap = new[,]
            {
                {0,0,4,0},
                {0,0,0,0},
                {0,0,2,0},
                {0,0,0,0}
            };
            var game = BuildGameMap(gameMap);
            var rnd = new Random();
            for (var i = 0; i < 13; i++)
            {
                var direction = (Direction)rnd.Next(Enum.GetNames(typeof(Direction)).Length);
                game.MakeMove(direction);
                game.AddRandomTile();
            }
            for (var i = 0; i < 13; i++)
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
