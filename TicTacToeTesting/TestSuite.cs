using NUnit.Framework;
using System;
using System.IO;
using System.Xml;
using TicTacToe.Models;

namespace TicTacToeTesting
{
  public class Tests
  {
    string testDir = Directory.GetParent(Environment.CurrentDirectory)
                                                    .Parent
                                                    .Parent
                                                    .FullName + "\\Tests\\";
    [Test]
    public void TestValidGameAgainstComputerInProgress()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstComputerInProgress.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null // the file was loaded
        && test.State.ToPlay == XOEnum.X // X plays next
        && !(test.State.IsWinState() || test.State.IsTieState()) // the game is not terminated
        && test.ai != null // is AI Player
      );
    }

    [Test]
    public void TestValidGameAgainstPlayerInProgress()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstPlayerInProgress.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null // the file was loaded
        && test.State.ToPlay == XOEnum.O // X plays next
        && !(test.State.IsWinState() || test.State.IsTieState()) // the game is not terminated
        && test.ai == null // is not AI
      );
    }

    [Test]
    public void TestValidGameAgainstComputerWhereOWon()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstComputerWhereOWon.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null
        && test.State.ToPlay == XOEnum.X
        && test.State.IsWinState()
        && !test.State.IsTieState()
        && test.ai != null
      );
    }

    [Test]
    public void TestValidGameAgainstComputerWhereXWon()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstComputerWhereXWon.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null
        && test.State.ToPlay == XOEnum.O
        && test.State.IsWinState()
        && !test.State.IsTieState()
        && test.ai != null
      );
    }

    [Test]
    public void TestValidGameAgainstComputerWhereTie()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstComputerWhereTie.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null
        && test.State.ToPlay == XOEnum.O
        && !test.State.IsWinState()
        && test.State.IsTieState()
        && test.ai != null
      );
    }

    [Test]
    public void TestValidGameAgainstHumanWhereXWon()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstHumanWhereXWon.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null
        && test.State.ToPlay == XOEnum.O
        && test.State.IsWinState()
        && !test.State.IsTieState()
        && test.ai == null
      );
    }

    [Test]
    public void TestValidGameAgainstHumanWhereOWon()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstHumanWhereOWon.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null
        && test.State.ToPlay == XOEnum.X
        && test.State.IsWinState()
        && !test.State.IsTieState()
        && test.ai == null
      );
    }

    [Test]
    public void TestValidGameAgainstHumanWhereTie()
    {
      FileStream s = new FileStream(testDir + "TestValidGameAgainstHumanWhereTie.xml", FileMode.Open);
      Game test = FileParser.LoadGame(s);

      Assert.IsTrue
      (
        test != null
        && test.State.ToPlay == XOEnum.O
        && !test.State.IsWinState()
        && test.State.IsTieState()
        && test.ai == null
      );
    }

    [Test]
    public void TestInvalidGameMultipleXPlays()
    {
      FileStream s = new FileStream(testDir + "TestInvalidGameMultipleXPlays.xml", FileMode.Open);
      Assert.Throws<FileFormatException>(() => FileParser.LoadGame(s));
    }

    [Test]
    public void TestInvalidGameIllegalDoubleMove()
    {
      FileStream s = new FileStream(testDir + "TestInvalidGameIllegalDoubleMove.xml", FileMode.Open);
      Assert.Throws<FileFormatException>(() => FileParser.LoadGame(s));
    }

    [Test]
    public void TestInvalidGameIllegalMoveOnAlreadyPlayedTile() 
    {
      FileStream s = new FileStream(testDir + "TestInvalidGameIllegalMoveOnAlreadyPlayedTile.xml", FileMode.Open);
      Assert.Throws<FileFormatException>(() => FileParser.LoadGame(s));
    }

    [Test]
    public void TestInvalidXMLFile()
    {
      FileStream s = new FileStream(testDir + "TestInvalidXMLFile.xml", FileMode.Open);
      Assert.Throws<XmlException>(() => FileParser.LoadGame(s));
    }
  }
}