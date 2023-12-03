using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class LessonViewModel : BaseViewModel
  {
    public string Lang;
    public int LessonLevel;
    public List<LessonListModel> lessonsList = new List<LessonListModel>();
    public List<LessonContentModel> lessons = new List<LessonContentModel>();
    private BaseViewModel _selectedViewModel;
    public string _lessonTitle;
    private ILessonRepository lessonRepository;


    public string LessonTitle { get { return _lessonTitle; } set { _lessonTitle = value; OnPropertyChanged(nameof(LessonTitle)); } }
    public List<LessonListModel> LessonsList { get { return lessonsList; } set { lessonsList = value; OnPropertyChanged(nameof(LessonsList)); } }
    public List<LessonContentModel> Lessons { get { return lessons; } set { lessons = value; OnPropertyChanged(nameof(Lessons)); } }
    public BaseViewModel SelectedViewModel
  {
    get { return _selectedViewModel; }
    set
    {
      _selectedViewModel = value;
      OnPropertyChanged(nameof(SelectedViewModel));
    }
  }
  public ICommand FrenchLessonUpdateViewCommand { get; set; }
  public LessonViewModel(string Lang, int LessonLevel)
  {
      this.Lang = Lang;
      this.LessonLevel = LessonLevel;
      lessonRepository = new LessonRepository();
      FrenchLessonUpdateViewCommand = new LessonUpdateViewCommand(this, Lang);
      LoadLesson();
  }

    public void LoadLesson()
    {
      if (LessonLevel != 0 && Lang != null)
      {
        var lesson = lessonRepository.Display(LessonLevel, Lang, Properties.Settings.Default.Language);
        LessonTitle = lesson.Lesson_Title;
        lessonRepository.Obtain_Lesson_List(LessonsList, Lang, Properties.Settings.Default.Language);
        lessonRepository.Obtain_Lessons(Lessons, lesson.Lesson_Title, Properties.Settings.Default.Language);
      }
    }
  }
}
