/*
 * Stephen Smith
 */
using Caliburn.Micro;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
  /*
   * This is the veiw model for the Game Screen, and 
   * controls all the rendering and message passing that
   * the game view must perform. 
   * 
   * The view model has a reference to it's parent, to allow
   * for invoking the shell to load the main menu.
   * 
   * It also has a Game, an array of string for display on the board,
   * a string that will change to indicate who's turn it is, and a 
   * clock to run the render function every 100ms.
   * 
   */
  class GameViewModel : Screen
  {
    public ShellViewModel Parent;
    private Game Game;
    private BindableCollection<string> _xoarray;
    private Timer Clock;
    private string _toPlayString = "X's Turn";

    public string ToPlayString
    {
      get { return _toPlayString; }
      set 
      { 
        _toPlayString = value;
        NotifyOfPropertyChange(() => ToPlayString);
      }
    }

    public bool CanMove {
      get {
        return !(Game.State.IsWinState() || Game.State.IsTieState());
      }
    }

    public BindableCollection<string> XOArray
    {
      get { return _xoarray; }
      set 
      { 
        _xoarray = value;
        NotifyOfPropertyChange(() => XOArray);
      }
    }


    public GameViewModel(ShellViewModel parent, Game game)
    {
      Parent = parent;
      Game = game;
      XOArray = new BindableCollection<string>();
      // init
      for (int i = 0; i < 9; i++)
      {
        XOArray.Add("");
      }
      // this timer will call the Render() function every 100ms.
      Clock = new Timer(e => Render(), null, 0, 100);
    }

    /*
     * This function is called by all of the button on the View, which each pass
     * a unique paramater to this method.
     * 
     * This method communicates to the game logic to make a move, and adjusts the 
     * ToPlayString such that the game will always show who is supposed to play next.
     * 
     * Additionally, if a move ends the game, it will handle opening a message box to
     * indicate the end of the game.
     */
    public void Move(int Square)
    {
      //only act if the move is legal
      if (Game.CanMove(Square))
      {
        Game.Move(Square);
        ToPlayString = (Game.State.ToPlay == XOEnum.X ? "X" : "O") + "'s Turn";
        if (Game.State.IsWinState())
        {
          System.Windows.MessageBox.Show($"{(Game.State.ToPlay == XOEnum.X ? "O" : "X")} wins!", "Winner!", MessageBoxButton.OK);
          ToPlayString = "Game Over";
          NotifyOfPropertyChange(() => CanMove);
        }
        if (Game.State.IsTieState())
        {
          System.Windows.MessageBox.Show($"The Game is a Draw", "Tie", MessageBoxButton.OK);
          ToPlayString = "Game Over";
          NotifyOfPropertyChange(() => CanMove);
        }
      }
    }
    /*
     * This method is called every 100ms to render the screen, so that when the AI performs moves
     * on the Game it does not need to directly communicate with the front end to alter the view,
     * rather the view will always match the game state.
     */
    public void Render()
    {
      for(int i = 256; i > 0; i /= 2)
      {
        // 9 - Log₂(Square) - 1 gets us our index
        //Log₂ because we just need the bit index of the only set bit in the number
        //9 - x - 1 because we need to have 256 = 0 and 1 = 8
        int log = 9 - (int)Math.Log(i, 2) - 1;
        if (XOArray[log] == "")
        {
          if((Game.State.XState & i) != 0)
          {
            XOArray[log] = "X";
          }
          else if((Game.State.OState & i) != 0)
          {
            XOArray[log] = "O";
          }
        }
      }
    }

    /*
     * This method attempts to load a game by communicating to the FileAccessor.
     * 
     * If the FileAccessor returns null (cancelled operation), then it will simply
     * return and do nothing. Otherwise the viewmodel will set the Game to be the game
     * loaded by the file accessor, and will adjust it's internal state based on the 
     * returned game.
     * 
     * If the FileAccessor errors, meaning the XML was ill formatted, it will pop up
     * an error box, and then do nothing.
     */
    public void LoadGame()
    {
      try
      {
        Game load = FileAccessor.LoadGame();
        if(load == null)
        {
          return;
        }
        else
        {
          Game = load;
          ClearXOArray();
          NotifyOfPropertyChange(() => CanMove);
          if (Game.State.IsWinState() || Game.State.IsTieState())
          {
            ToPlayString = "Game Over";
          }
          else
          {
            ToPlayString = (Game.State.ToPlay == XOEnum.X ? "X" : "O") + "'s Turn";
          }
        }
      }
      catch (Exception e)
      {
        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
      }
    }

    /*
     * This is a utlity method to clear the XO array for a new game.
     */
    private void ClearXOArray()
    {
      for(int i = 0; i < 9; i++)
      {
        XOArray[i] = "";
      }
    }


    public void SaveGame()
    {
      FileAccessor.SaveGame(Game);
    }

    /*
     * This method resets the board state to 0, while
     * maintaining the game type selected.
     * 
     * It will not reload the previously requests save file.
     */
    public void Reset()
    {
      if(Game.Type == GameType.OnePlayer)
      {
        Game = new Game(Game.Type, Game.ai.Side, false);
      }
      else
      {
        Game = new Game(Game.Type);
      }
      ClearXOArray();
      NotifyOfPropertyChange(() => CanMove);
    }


    public void BackToMainMenu()
    {
      //we don't want the clock to keep running.
      Clock.Dispose();     
      Parent.LoadLandingViewModel();
    }
  }
}
