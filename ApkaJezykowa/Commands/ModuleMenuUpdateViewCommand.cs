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
  internal class ModuleMenuUpdateViewCommand : ICommand
  {
    private ModuleMenuViewModel viewModel;
    private string Lang;

    public ModuleMenuUpdateViewCommand(ModuleMenuViewModel viewModel, string Lang)
    {
      this.viewModel = viewModel;
            this.Lang = Lang;
            //comment
        }


        public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "FrenchLesson")
      {
        //ExerciseLevelModel.Instance.Level = 1;
        //ExerciseLevelModel.Instance.Language = "French";
        viewModel.SelectedViewModel = new LessonListViewModel(Lang);
      }
      if (parameter.ToString() == "FrenchReturn")
      {
        viewModel.SelectedViewModel = new LessonsViewModel();
      }
      if (parameter.ToString() == "FrenchExercise")
      {
        //ExerciseLevelModel.Instance.Language = "French";
        viewModel.SelectedViewModel = new ExerciseMenuViewModel(Lang);
      }
    }
  }
}

