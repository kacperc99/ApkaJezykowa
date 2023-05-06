using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.Repositories;
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
    public List<string> par = new List<string>();
    private ILessonRepository lessonRepository;

    public FrenchLessonUpdateViewCommand(FrenchLessonViewModel viewModel)
    {
      this.viewModel = viewModel;
      lessonRepository = new LessonRepository();
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      lessonRepository.Obtain_Pars(par, ExerciseLevelModel.Instance.Language);
      Console.WriteLine("Clicked!");
      int i = 1;
      foreach (string s in par)
      {
        if (parameter.ToString() == s)
        {
          ExerciseLevelModel.Instance.Level = i;
          viewModel.SelectedViewModel = new FrenchLessonViewModel();
        }
        else
          i++;
      }
      
      /*if(parameter.ToString() == "french1")
      {
        ExerciseLevelModel.Instance.Level = 1;
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "french2")
      {
        ExerciseLevelModel.Instance.Level = 2;
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "french3")
      {
        ExerciseLevelModel.Instance.Level = 3;
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "french4")
      {
        ExerciseLevelModel.Instance.Level = 4;
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }*/
    }
  }
}
