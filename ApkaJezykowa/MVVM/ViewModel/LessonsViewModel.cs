using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApkaJezykowa.Main;
using System.Windows.Input;
using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;

namespace ApkaJezykowa.MVVM.ViewModel
{
  class LessonsViewModel : BaseViewModel
  {
    public List<string> lessonNames = new List<string>();
    public List<LessonContentModel> lessons = new List<LessonContentModel>();
    private BaseViewModel _selectedViewModel;
    public string _country;
    public string _language;
    public string _lesson;
    public string _title;
    public decimal _level;
    public string _newLessonText;
    public byte[] _newLessonImage;

    public List<string> LessonNames { get { return lessonNames; } set { lessonNames = value; OnPropertyChanged(nameof(LessonNames)); } }
    public List<LessonContentModel> Lessons { get { return lessons; } set { lessons = value; OnPropertyChanged(nameof(Lessons));} }
    public string Country { get { return _country; } set { _country = value; OnPropertyChanged(nameof(Country)); } }  
    public string Language { get { return _language; } set { _language = value; OnPropertyChanged(nameof(Language)); } }
    public string Lesson { get { return _lesson; } set { _lesson = value; OnPropertyChanged(nameof(Lesson)); } }  
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
    public decimal Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); } }

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));
      }
    }
    public ICommand LessonUpdateViewCommand { get; set; }
    public LessonsViewModel()
    {
      LessonUpdateViewCommand = new LessonUpdateViewComand(this);
      LoadData();
    }
    public void LoadData()
    {

    }
  }
}
