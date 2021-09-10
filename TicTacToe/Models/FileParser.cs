/*
 * Stephen Smith
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TicTacToe.Models
{
  public static class FileParser
  {
    /*
     * This class is a static File Parser, which can handle two operations,
     * loading a game from a stream and saving a game to a stream.
     * 
     * Essentiall this is middleware between the game logic and the file accessor, 
     * translating files into games and games into files.
     */


    /*
     * This method loads a game from a given stream. To do this we walk through
     * the file getting all of the moves, and adding them to the move list.
     * 
     * All we need to reconstruct a game is a list of moves, however the file will
     * have additional meta information which we can use to validate that the game we are loading 
     * is a legal game.
     * 
     * Method throws file format exception if the xml is ill formatted.
     */
    public static Game LoadGame(Stream stream)
    {
      GameType? Type = null;
      List<int> Moves = new List<int>();
      XOEnum ToPlay = XOEnum.X;
      XOEnum? ComputerSide = null;
      using (XmlReader reader = XmlReader.Create(stream))
      {
        while (reader.Read())
        {
          if(reader.IsStartElement())
          {
            // get the type of the game, and throw exception if no type specified
            if (reader.Name == "Game")
            {
              switch (reader.GetAttribute("type"))
              {
                case "OnePlayer":
                  Type = GameType.OnePlayer;
                  break;
                case "TwoPlayer":
                  Type = GameType.TwoPlayer;
                  break;
                default:
                  throw new FileFormatException("Invalid Game File");
              }
            }
            else if (reader.Name == "Move")
            {
              // we only need to read which side the AI is playing as once.
              if(reader.GetAttribute("type") == "Computer" && ComputerSide == null && Type == GameType.OnePlayer)
              {
                ComputerSide = ToPlay;
              }
            }
            else if (reader.Name == "Play")
            {
              string t = reader.GetAttribute("sign");
              if(t == "O" || t == "X")
              {
                XOEnum Play = t == "X" ? XOEnum.X : XOEnum.O;
                // if the move is legal
                if(Play == ToPlay)
                {
                  int play = reader.ReadElementContentAsInt();
                  // if Log₂(play) is an integer, it's a legal move. If not, multiple bits are set which is illegal.
                  if(Math.Log(play, 2) ==(int)Math.Log(play, 2)) 
                  {
                    Moves.Add(play);

                    ToPlay = ToPlay == XOEnum.X ? XOEnum.O : XOEnum.X;
                  }
                  else
                  {
                    throw new FileFormatException("Invalid Game File");
                  }
                  
                }
                else
                {
                  throw new FileFormatException("Invalid Game File");
                }
              }
              else
              {
                Console.WriteLine("Invalid game specified.");
                throw new FileFormatException("Invalid Game File");
              }
            }
          }
        }
      }
      /*
       * once we have the list of moves, we can generate the game by 
       * walking through the moves and playing them on the board.
       * 
       * We must set the game into a loading state, to prevent the AI
       * player from attempting to play while the game is loading.
       */
      Game ret;
      if (Type == GameType.OnePlayer)
      {
        ret = new Game(GameType.OnePlayer, ComputerSide.Value, true);
        ret.StartLoading();
        foreach(int m in Moves)
        {
          if(ret.CanMove(m))
          {
            ret.Move(m);
          }
          else
          {
            throw new FileFormatException("Invalid Game File");
          }
        }
        ret.EndLoading();
      }
      else
      {
        ret = new Game(GameType.TwoPlayer);
        ret.StartLoading();
        foreach (int m in Moves)
        {
          if (ret.CanMove(m))
          {
            ret.Move(m);
          }
          else
          {
            throw new FileFormatException("Invalid Game File");
          }
        }
        ret.EndLoading();
      }
      stream.Close();
      return ret;
    }

    /*
     * This method handles saving a game. It is a simple method,
     * simply walking through the list of moves that occured in the game,
     * and saving them in order - along with the metadata necessary.
     */
    public static void SaveGame(Stream stream, Game game)
    {

      using (XmlWriter writer = XmlWriter.Create(stream))
      {
        writer.WriteStartElement("Game");
        writer.WriteAttributeString("type", game.Type == GameType.OnePlayer ? "OnePlayer" : "TwoPlayer");

        XOEnum ToPlay = XOEnum.X;
        foreach (int m in game.Moves)
        {
          writer.WriteStartElement("Move");
          if (game.Type == GameType.OnePlayer && ToPlay == game.ai.Side)
          {
            writer.WriteAttributeString("type", "Computer");
          }
          else
          {
            writer.WriteAttributeString("type", "Player");
          }
          writer.WriteStartElement("Play");
          writer.WriteAttributeString("sign", ToPlay == XOEnum.X ? "X" : "O");
          ToPlay = ToPlay == XOEnum.X ? XOEnum.O : XOEnum.X;
          writer.WriteValue(m);
          writer.WriteEndElement();
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }
      stream.Close();
    }
  }
}
