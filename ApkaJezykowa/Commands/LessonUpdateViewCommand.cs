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
  internal class LessonUpdateViewCommand : ICommand
  {
    public string Lang;
    private LessonViewModel viewModel;
    public List<string> par = new List<string>();
    private ILessonRepository lessonRepository;

    public LessonUpdateViewCommand(LessonViewModel viewModel, string Lang)
    {
      this.Lang = Lang;
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
      var values = (object[])parameter;
      var id_Lesson_Title = values[0];
      var lesson_Title = values[1];
      var lesson_Level = values[2];
      //decimal LessonLevel = lessonRepository.Obtain_Level((int)parameter, Lang);
      viewModel.SelectedViewModel = new LessonViewModel(Lang, (int)id_Lesson_Title, lesson_Title.ToString(), (decimal)lesson_Level);
    }
  }
}
