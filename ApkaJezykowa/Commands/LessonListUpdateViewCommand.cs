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
  internal class LessonListUpdateViewCommand : ICommand
  {
    private string Lang;
    private LessonListViewModel viewModel;
    public List<string> par = new List<string>();
    private ILessonRepository lessonRepository;

    public LessonListUpdateViewCommand(LessonListViewModel viewModel, string Lang)
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
       decimal LessonLevel = lessonRepository.Obtain_Level((int)parameter, Lang);
       viewModel.SelectedViewModel = new LessonViewModel(Lang, LessonLevel, (int)parameter);
    }
  }
}
