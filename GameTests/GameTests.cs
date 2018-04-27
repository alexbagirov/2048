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
            for (var y = 0; y < game.Map.Height; y++)
                for (var x = 0; x < game.Map.Width; x++)
                    if (game.Map[x, y].Value != 0)
                    {
                        var value = game.Map[x, y].Value;
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
                game.AddNewTile();
            for (var y = 0; y < game.Map.Height; y++)
                for (var x = 0; x < game.Map.Width; x++)
                    if (game.Map[x, y].Value != 0)
                    {
                        var value = game.Map[x, y].Value;
                        nonZeroTiles++;
                        Assert.IsTrue(value == 2 || value == 4);
                    }
            Assert.AreEqual(25, nonZeroTiles);
        }

        [Test]
        public void TestDifferentMapParameters()
        {
            Assert.Throws<ArgumentException>(() => new GameMap(-5, -2));
            Assert.Throws<ArgumentException>(() => new GameMap(1, 1));
            Assert.Throws<ArgumentException>(() => new GameMap(-4, 4));
            Assert.Throws<ArgumentException>(() => new GameMap(0, 100));
            var map = new GameMap(10, 10);
            Assert.AreEqual(10, map.Width);
            Assert.AreEqual(10, map.Height);
        }

        [Test]
        public void TestGameEnded()
        {
            var game = TryMoveTests.BuildGameMap(new[,]
           {
                {8,64,0}
            });
            game.TryMove(Direction.Right);
            Assert.IsFalse(game.IsRunning);
            game = TryMoveTests.BuildGameMap(new[,]
           {
                {2,4,8},
                {8,64,32},
                {8,32,16}
            });
            game.TryMove(Direction.Up);
            Assert.IsFalse(game.IsRunning);
            game = TryMoveTests.BuildGameMap(new[,]
           {
                {2,4,8},
                {8,64,32},
                {8,32,16}
            });
            Assert.IsTrue(game.IsRunning);
            game = TryMoveTests.BuildGameMap(new[,]
          {
                {8,32,0},
                {2,64,32},
                {2,32,16}
            });
            game.TryMove(Direction.Down);
            Assert.IsTrue(game.IsRunning);
        }
    }
}
