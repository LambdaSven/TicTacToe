/*
 * Stephen Smith
 */

using Microsoft.Win32;
using System;
using System.IO;
using System.Xml;

namespace TicTacToe.Models
{
  /*
   * This class is a backend class to handle the File Dialogues and 
   * opening of files. The actual writing and parsing happens in the 
   * middleware file parser, the sole responsibility of this class
   * is to facilitate opening of files into streams.
   */
  class FileAccessor
  {
    private static string init = Directory.GetParent(Environment.CurrentDirectory)
                                                                .Parent
                                                                .FullName + "\\Saves";
    /*
     *  This method opens a file dialog for loading a game,
     *  and it will pass the stream to the File Parser, which
     *  will handle the responsibility of closing the stream.
     */
    internal static Game LoadGame()
    {
      //load the game from file
      OpenFileDialog o = new OpenFileDialog();

      o.InitialDirectory = init;

      o.Title = "Load Save File";
      o.Filter = "Save Files (*.xml)|*.xml";

      if (o.ShowDialog() == true)
      {
        return FileParser.LoadGame(o.OpenFile());
      }
      else
      {
        return null;
      }
    }

    /*
     * This method handles saving of games. It will open
     * a save dialog and allow players to save games,
     * passing the open file stream to the file parser
     * which will convert the designated game into a properly
     * formatted XML file.
     */
    internal static void SaveGame(Game game)
    {
      SaveFileDialog s = new SaveFileDialog();

      s.InitialDirectory = init;

      s.Title = "Save Game";
      s.Filter = "xml files (*.xml)|*.xml";

      if(s.ShowDialog() == true)
      {
        FileParser.SaveGame(s.OpenFile(), game);
      }
    }
  }
}
