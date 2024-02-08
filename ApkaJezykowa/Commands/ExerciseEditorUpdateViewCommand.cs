using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class ExerciseEditorUpdateViewCommand : ICommand
  {
    private ExerciseEditorViewModel viewModel;

    public ExerciseEditorUpdateViewCommand(ExerciseEditorViewModel viewModel)
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
      if (parameter.ToString() == "Lesson")
      {
        viewModel.SelectedViewModel = new LessonEditorViewModel();
        // ExerciseLevelModel.Instance.Language = "Niemiecki";
      }
      if (parameter.ToString() == "French")
      {
        //viewModel.SelectedViewModel = new ModuleMenuViewModel();
        //ExerciseLevelModel.Instance.Language = "French";
      }
      if (parameter.ToString() == "English")
      {
        // viewModel.SelectedViewModel = new EnglishViewModel();
        // ExerciseLevelModel.Instance.Language = "Angielski";
      }
    }
  }
}
