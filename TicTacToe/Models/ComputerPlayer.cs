/*
 * Stephen Smith
 */

using System.Collections.Generic;
namespace TicTacToe.Models
{
  /*
   * This file is a model of a computer player for Tic Tac Toe.
   * 
   * The Computer Player only has knowledge of what side it is playing,
   * and every time it needs to make a move it evaluates the board based on
   * it's state.
   * 
   * It generates a Game Tree of all the possible positions that can originate from
   * the current state, and then it uses a minimax algorithm to evaluate the best move
   * from a given state.
   * 
   * In doing so, this Chess AI should draw or win every game it plays against a human.
   */
  public class ComputerPlayer
  {
    public XOEnum Side;

    public ComputerPlayer(XOEnum side)
    {
      Side = side;
    }

    /*
     * Generating a move is a very involves process. First we generate the total game tree,
     * and attempt to find the move with the maximum scores. Scores are calculated locally
     * within the game tree using the minimax algorithm.
     */
    public int GenerateMove(GameState state)
    {
      GameTree root = GenerateTree(state);
      root.CalculateScore(Side);
      GameState desired = null;
      int MaxScore = int.MinValue;
      foreach(GameTree g in root.Leaves)
      {
        if (g.Score > MaxScore)
        {
          desired = g.State;
          MaxScore = g.Score;
        }
      }
      // we can identify the move played to move the current state into the desired state with xor
      return Side == XOEnum.X ? desired.XState ^ state.XState : desired.OState ^ state.OState;
    }

    /*
     * This method generates the total game tree and returns the root. It does so recursively,
     * at every node it generates the game trees of all the states that can occur directly from that node.
     * 
     * This creates a tree structure which we can navigate easily from the root.
     */
    public GameTree GenerateTree(GameState State)
    {
      List<GameState> Moves = GenerateAllMoves(State);
      GameTree Root = new GameTree(State, EvaluateState(State));

      foreach(GameState s in Moves)
      {
        Root.Leaves.Add(GenerateTree(s));
      }
      return Root;
    }

    /*
     * This method gives an evaluation of the board, for tic tac toe
     * we have a simple evaluation - if the state is a win for us +10,
     * and if it's a win for the opponent -10. Otherwise it's a neutral board,
     * with an evaluation of 0.
     */
    public int EvaluateState(GameState state)
    {
      if(state.IsWinState() && state.ToPlay == Side)
      {
        return -10;
      }
      else if(state.IsWinState() && state.ToPlay != Side)
      {
        return 10;
      }
      return 0;
    }

    /*
     * To generate all moves from a state, we cycle through all possible moves
     * and determine if the move is legal. If it is, we add the move to our
     * list of states, and return the list when finished
     */
    public List<GameState> GenerateAllMoves(GameState state)
    {
      List<GameState> ret = new List<GameState>();
      //log₂ 259 = 9 so this will run 9 times
      for (int i = 256; i > 0; i /= 2)
      {
        if ((state.XState & i) != 0 || (state.OState & i) != 0 || state.IsWinState())
        {
          continue;
        }
        if (state.ToPlay == XOEnum.X)
        {
          ret.Add(new GameState(state.XState | i, state.OState, XOEnum.O));
        }
        else
        {
          ret.Add(new GameState(state.XState, state.OState | i, XOEnum.X));
        }
      }
      return ret;
    }
  }
}
