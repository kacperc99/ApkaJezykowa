using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.MVVM.Model;

namespace ApkaJezykowa.Commands
{
    internal class LessonUpdateViewComand : ICommand
    {
            private LessonsViewModel viewModel;

            public LessonUpdateViewComand(LessonsViewModel viewModel)
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
      if (parameter.ToString() == "German")
      {
        // viewModel.SelectedViewModel = new GermanViewModel();
        // ExerciseLevelModel.Instance.Language = "Niemiecki";
      }
      if (parameter.ToString() == "French")
      {
        viewModel.SelectedViewModel = new FrenchViewModel();
        ExerciseLevelModel.Instance.Language = "French";
      }
      if (parameter.ToString() == "English")
      {
        // viewModel.SelectedViewModel = new EnglishViewModel();
        // ExerciseLevelModel.Instance.Language = "Angielski";
      }
    }
    }
}
