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
  public class FrenchLessonViewModel : BaseViewModel
  {

    private BaseViewModel _selectedViewModel;
    public string _lessonTitle;
    public string _lessonText;
    private ILessonRepository lessonRepository;

    public string LessonTitle { get { return _lessonTitle; } set { _lessonTitle = value; OnPropertyChanged(nameof(LessonTitle)); } }
    public string LessonText { get { return _lessonText; } set { _lessonText = value; OnPropertyChanged(nameof(LessonText)); } }

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
  public FrenchLessonViewModel()
  {
      lessonRepository = new LessonRepository();
      FrenchLessonUpdateViewCommand = new FrenchLessonUpdateViewCommand(this);
      LoadLesson();
  }

    public void LoadLesson()
    {
      if (ExerciseLevelModel.Instance.Level != 0 && ExerciseLevelModel.Instance.Language != null)
      {
        var lesson = lessonRepository.Display(ExerciseLevelModel.Instance.Level, ExerciseLevelModel.Instance.Language);
        LessonTitle = lesson.Lesson_Title;
        LessonText = lesson.Lesson_Text;
      }
    }
  }
}
