/*
 * Stephen Smith
 */
namespace TicTacToe.Models
{
  /*
   * This is the class representing our gamestate, which
   * also contains the logic for winning or losing games.
   * 
   * The Game is represented as 2 bit boards of 9 bits each, 
   * where each bit is a unique tile on the board. By using 
   * a bit board like this, we can perform all of our 
   * win logic and state updates purely with bitwise operations.
   * 
   * This is very efficient on the CPU as bitwise operations are very fast,
   * as well as having better complexty O(8), constant time.
   * 
   * We also store the player who must go next, as well as an array of the
   * 8 possible win states of the game. 
   */
  public class GameState
  {
    public int XState, OState;
    public XOEnum ToPlay;
    public int[] WinBitMaps = { 
     //Vertical Lines
      0b100100100
     ,0b010010010
     ,0b001001001
    //Horizontal Lines
     ,0b111000000
     ,0b000111000
     ,0b000000111
    //Diagonal Lines
     ,0b100010001
     ,0b001010100};

    public GameState(int X, int O, XOEnum toplay)
    {
      ToPlay = toplay;
      XState = X;
      OState = O;
    }

    public GameState()
    {
      ToPlay = XOEnum.X;
      XState = 0;
      OState = 0;
    }

    /*
     * This method determines if the board is a win state by performing
     * bitwise comparisons between the state of the board and the possible
     * win states.
     * 
     * Because 0 & 1 == 0 and 1 & 1 == 1, by comparing an arbitrary state with
     * one of the win states, all the 0's in the win state will be discounted and
     * the result will only have 1s in spaces that the win state has 1s.
     * 
     * If the current state AND the win state is equal to the win state, that means
     * that all three bits are shared, meaning that the player has won.
     */
    public bool IsWinState()
    {
      foreach(int e in WinBitMaps)
      {
        if((XState & e) == e || (OState & e) == e)
        {
          return true;
        }
      }
      return false;
    }

    /*
     * This method is a simple method to determine if the game is in a tie state.
     * 
     * To determine this, we simply perform OR on the two states, and determine if 
     * it matches a filled board. If it does, then the game is a tie, otherwise there 
     * is still space to move.
     */
    public bool IsTieState()
     => !IsWinState() && (XState | OState) == 0b111111111;
  }
}
