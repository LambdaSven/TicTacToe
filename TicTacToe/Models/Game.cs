/*
 * Stephen Smith
 */
using System.Collections.Generic;

namespace TicTacToe.Models
{
  /*
   * This class represents a single game of tic tac toe.
   * The class has a Type, a current state, a list of previous moves,
   * a computer player, as well as a boolean indicating if the game
   * is currently loading (preventing the AI from playing during loading.)
   * 
   * The game has one primary function, which is the Move function, which accepts a
   * value as an input and uses bit operations to set the necessary state to include the
   * new move.
   * 
   * Additionally the game as a ComputerMove() method, which is only used interally and 
   * allows the computer to make a move.
   */
  public class Game
  {
    public GameType Type;
    public  GameState State;
    internal List<int> Moves;
    public ComputerPlayer ai;
    internal bool loadState = false;
    internal Game(GameType type)
    {
      Type = type;
      State = new GameState();
      Moves = new List<int>();
    }

    internal Game(GameType type, XOEnum AI, bool loading)
    {
      Type = type;
      ai = new ComputerPlayer(AI);
      State = new GameState();
      Moves = new List<int>();

      // if ai goes first, start with a move.
      if (AI == XOEnum.X && !loading)
      {
        ComputerMove();
      }
    }


    public void StartLoading()
    {
      loadState = true;
    }

    public void EndLoading()
    {
      loadState = false;
      // if AI needs to move, move.
      if(ai != null && State.ToPlay == ai.Side && !(State.IsTieState() || State.IsWinState()))
      {
        ComputerMove();
      }
    }

    /*
     * This is the move function, which takes an int representing the square that is being 
     * selected, and using bitwise OR to set the bit on the relevant state to indicate that the
     * tile has been selected.
     * 
     * There is no verification here that the move is legal, as that verification happens elsewhere.
     */
    public void Move(int Square)
    {
      Moves.Add(Square);
      if (State.ToPlay == XOEnum.X)
      {
        // add the move to XState 
        State = new GameState(State.XState | Square, State.OState, XOEnum.O);
      }
      else
      {
        // add the move to OState
        State = new GameState(State.XState, State.OState | Square, XOEnum.X);
      }
      if (Type == GameType.OnePlayer && !State.IsWinState() && !State.IsTieState() && !loadState)
      {
        ComputerMove();
      }
    }

    /*
     * This is the computer move function,
     * it is essentially the same as the Move function, however
     * it uses the ai to generate the move.
     * 
     * This is separate from the move function to prevent recursion errors, simply
     * calling Move(ai.GenerateMove(State)) would recurse infinitely.
     */
    public void ComputerMove()
    {
      int move = ai.GenerateMove(State);
      Moves.Add(move);
      if(State.ToPlay == XOEnum.X)
      {
        State = new GameState(State.XState | move, State.OState, XOEnum.O);
      }
      else
      {
        State = new GameState(State.XState, State.OState | move, XOEnum.X);
      }
    }

    /*
     * This method checks if the requested move has any conflict with the current state,
     * and returns true if the move is legal.
     */
    public bool CanMove(int Square) 
      => (State.XState & Square) == 0 && (State.OState & Square) == 0;
  }
}