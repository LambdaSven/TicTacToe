/*
 * Stephen Smith
 */
using Caliburn.Micro;
using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
  class ShellViewModel : Conductor<IScreen>
  {
    /*
     * This is the shell conductor, which simply handles
     * the rendering of the two view models and will be called
     * by those view models to show the requested screens.
     */
    public ShellViewModel()
      => LoadLandingViewModel();

    public void LoadLandingViewModel() => 
      ActivateItemAsync(new LandingViewModel(this));

    public void LoadGameViewModel(Game game) => 
      ActivateItemAsync(new GameViewModel(this, game));
  }
}
