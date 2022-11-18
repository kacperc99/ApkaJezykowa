using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class EnglishUpdateViewCommand : ICommand
  {
    private EnglishViewModel viewModel;

    public EnglishUpdateViewCommand(EnglishViewModel viewModel)
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
      if (parameter.ToString() == "EnglishLesson")
      {
       // viewModel.SelectedViewModel = new EnglishLessonViewModel();
      }
      if (parameter.ToString() == "EnglishReturn")
      {
        viewModel.SelectedViewModel = new LessonsViewModel();
      }
    }
  }
}
