using System;
using NUnit.Framework;
using Game2048;
using MovementTests;

namespace GameTests
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void TestTwoTilesAtBeginning()
        {
            var game = new Game(4, 4);
            var nonZeroTiles = 0;
            for (var y = 0; y < game.Height; y++)
                for (var x = 0; x < game.Width; x++)
                    if (game[x, y].Value != 0)
                    {
                        var value = game[x, y].Value;
                        nonZeroTiles++;
                        Assert.IsTrue(value == 2 || value == 4);
                    }
            Assert.AreEqual(2, nonZeroTiles);
        }

        [Test]
        public void TestAddNewTile()
        {
            var game = new Game(5, 5);
            var nonZeroTiles = 0;
            for (var i = 0; i < 23; i++)
                game.AddRandomTile();
            for (var y = 0; y < game.Height; y++)
                for (var x = 0; x < game.Width; x++)
                    if (game[x, y].Value != 0)
                    {
                        var value = game[x, y].Value;
                        nonZeroTiles++;
                        Assert.IsTrue(value == 2 || value == 4);
                    }
            Assert.AreEqual(25, nonZeroTiles);
        }

        [Test]
        public void TestDifferentMapParameters()
        {
            Assert.Throws<ArgumentException>(() => new Game(-5, -2));
            Assert.Throws<ArgumentException>(() => new Game(1, 1));
            Assert.Throws<ArgumentException>(() => new Game(-4, 4));
            Assert.Throws<ArgumentException>(() => new Game(0, 100));
            var game = new Game(10, 10);
            Assert.AreEqual(10, game.Width);
            Assert.AreEqual(10, game.Height);
        }
    }

    [TestFixture]
    public class EndTests
    {
        [Test]
        public void TestSimpleGameEnd()
        {
            var game = TryMoveTests.BuildGameMap(new[,]
            {
                {8,64,2}
            });
            Assert.IsTrue(game.HasEnded());
           
        }

        [Test]
        public void TestGameNotEnd()
        {
            var game = TryMoveTests.BuildGameMap(new[,]
            {
                {8,64,0}
            });
            Assert.IsFalse(game.HasEnded());
        }

        [Test]
        public void TestGameEndedAfterMove()
        {
            var game = TryMoveTests.BuildGameMap(new[,]
            {
                {2,4,8},
                {8,64,32},
                {8,32,16}
            });
            game.TryMove(Direction.Up);
            game.AddRandomTile();
            Assert.IsTrue(game.HasEnded());
        }

        [Test]
        public void TestGameNotEndedWhenMovesAvailable()
        {
            var game = TryMoveTests.BuildGameMap(new[,]
            {
                {2,4,8},
                {8,64,32},
                {8,32,16}
            });
            Assert.IsFalse(game.HasEnded());
        }

        [Test]
        public void TestGameNotEndedWhenNewEmptyTiles()
        {
            var game = TryMoveTests.BuildGameMap(new[,]
            {
                {8,32,0},
                {2,64,32},
                {2,32,16}
            });
            game.TryMove(Direction.Down);
            game.AddRandomTile();
            Assert.IsFalse(game.HasEnded());
        }
    }
}
