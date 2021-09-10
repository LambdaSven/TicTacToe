/*
 * Stephen Smith
 */
using Caliburn.Micro;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
  /*
   * This is the view model for the main menu, or the landing screen.
   * 
   * The view model only has one method, which is PlayGame, which loads the game view
   * models as required.
   */
  class LandingViewModel : Screen
  {
    public ShellViewModel Parent;
    public LandingViewModel(ShellViewModel parent)
    {
      Parent = parent;
    }

    /*
     * This method recieves the button from the view as a parameter, allowing us 
     * to determine which button was clicked. Once we have determined that we 
     * can alter the load state based on the requested load.
     */
    public void PlayGame(Button button)
    {
      //parse gamestate intended and pass to Game - possibly load a new game already

      GameType? type; //nullable so we can determine if we are supposed to load a game, and then determine state
      switch(button.Name)
      {
        case "TwoPlayer":
          type = GameType.TwoPlayer;
          break;
        case "OnePlayer":
          type = GameType.OnePlayer;
          break;
        case "LoadGame":
          type = null;
          break;
        default:
          type = null;
          break;
      }

      // if it's null, then the third button "load game" was pressed.
      if (type == null)
      {
        /*
         * Catch invalid files and error.
         */
        try
        {
          Game g = FileAccessor.LoadGame();
          if(g != null)
          {
            Parent.LoadGameViewModel(g);
          }
        }
        catch (Exception e)
        {
          MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
        }
      }
      else
      {
        if(type == GameType.OnePlayer)
        {
          MessageBoxResult result = MessageBox.Show("Would you Like to play as X", "Player Selection", MessageBoxButton.YesNo);
          if(result == MessageBoxResult.Yes)
          {
            Parent.LoadGameViewModel(new Game(type.Value, XOEnum.O, false));
          }
          else if(result == MessageBoxResult.No)
          {
            Parent.LoadGameViewModel(new Game(type.Value, XOEnum.X, false));
          }
        }
        else
        {
          Parent.LoadGameViewModel(new Game(type.Value));
        } 
      }
    }
  }
}
