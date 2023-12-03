using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class LessonListViewModel : BaseViewModel
  {
    public List<LessonListModel> lessonsList = new List<LessonListModel>();
    string Lang;
    private BaseViewModel _selectedViewModel;
    private ILessonRepository lessonRepository;
    public List<LessonListModel> LessonsList { get { return lessonsList; } set { lessonsList = value; OnPropertyChanged(nameof(LessonsList)); } }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));
      }
    }
    public ICommand LessonListUpdateViewCommand { get; set; }
    public LessonListViewModel(string Lang) 
    { 
      this.Lang = Lang;
      lessonRepository = new LessonRepository();
      lessonRepository.Obtain_Lesson_List(LessonsList, Lang, Properties.Settings.Default.Language);
      LessonListUpdateViewCommand = new LessonListUpdateViewCommand(this, Lang);
    }
  }
}
