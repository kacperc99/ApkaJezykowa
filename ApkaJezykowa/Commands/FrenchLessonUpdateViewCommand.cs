using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class FrenchLessonUpdateViewCommand : ICommand
  {
    private FrenchLessonViewModel viewModel;


    public FrenchLessonUpdateViewCommand(FrenchLessonViewModel viewModel)
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
      if(parameter.ToString() == "French1")
      {
        ExerciseLevelModel.Instance.Level = 1;
        ExerciseLevelModel.Instance.Language = "Francuski";
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "French2")
      {
        ExerciseLevelModel.Instance.Level = 2;
        ExerciseLevelModel.Instance.Language = "Francuski";
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "French3")
      {
        ExerciseLevelModel.Instance.Level = 3;
        ExerciseLevelModel.Instance.Language = "Francuski";
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
    }
  }
}
