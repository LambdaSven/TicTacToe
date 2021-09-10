using Caliburn.Micro;
using System.Windows;
using TicTacToe.ViewModels;

namespace TicTacToe
{
  public class Bootstrapper : BootstrapperBase
  {
    public Bootstrapper()
    {
      Initialize();
    }
    protected override void OnStartup(object sender, StartupEventArgs e)
      => DisplayRootViewFor<ShellViewModel>();
  }
}
