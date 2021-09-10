/*
 * Stephen Smith
 */
using System.Collections.Generic;


namespace TicTacToe.Models
{
  /*
   * This class represents the Game Tree of all possible games
   * from a given Game State (the root).
   * 
   * Each Game Tree has a list of game trees (the leaves) as well as 
   * the score of the tree. The score of the tree on initalization is
   * the AI evaulation of the state (±10). 
   * 
   * However, when Calculate Score is called on a node of an assembled tree,
   * the tree preforms a basic min-max algorithm to determine the best move.
   */
  public class GameTree
  {
    public List<GameTree> Leaves;
    public GameState State;
    public int Score;
    public GameTree(GameState s, int eval)
    {
      State = s;
      Score = eval;
      Leaves = new List<GameTree>();
    }
    /*
     * This method is a basic min-max for the best move. It recieves as a 
     * paramter the side that it is performing a calculation for, and then
     * performing the min max based on that side.
     * 
     * During each phase, the node goes through all it's leaves to determine which has
     * the maximum or minimum score, depending on who's turn it is analysing, and 
     * passes that value back up the tree.
     * 
     * As such, this node will recieve a score that is the best possible move to play
     * for a given side, based on the ai's evaluation algorithm.
     */
    public void CalculateScore(XOEnum side)
    {
      if(State.IsWinState() || State.IsTieState())
      {
        return;
      }
      else
      {
        if(State.ToPlay == side)
        {
          //maximize
          int Max = int.MinValue;
          foreach(GameTree g in Leaves)
          {
            g.CalculateScore(side);
            Max = g.Score > Max ? g.Score : Max;
          }
          Score = Max;
        }
        else
        {
          //minimize
          int Min = int.MaxValue;
          foreach (GameTree g in Leaves)
          {
            g.CalculateScore(side);
            Min = g.Score < Min ? g.Score : Min;
          }
          Score = Min;
        }
      }
    }
  }
}
