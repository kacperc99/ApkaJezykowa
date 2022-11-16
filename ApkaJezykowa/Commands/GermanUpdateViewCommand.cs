using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class GermanUpdateViewCommand : ICommand
  {
    private GermanViewModel viewModel;

    public GermanUpdateViewCommand(GermanViewModel viewModel)
    {
      this.viewModel = viewModel;
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "GermanLesson")
      {
        viewModel.SelectedViewModel = new GermanLessonViewModel();
      }
    }
  }
}
