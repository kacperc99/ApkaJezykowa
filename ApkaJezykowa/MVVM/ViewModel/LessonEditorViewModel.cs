using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
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
  public class LessonData : BaseViewModel
  {
    public int _lessonID;
    public string _lessonText;
    public byte[] _lessonImage;

    public int LessonID { get { return _lessonID; } set { _lessonID = value; OnPropertyChanged(nameof(LessonID)); } }
    public string LessonText { get { return _lessonText; } set { _lessonText = value; OnPropertyChanged(nameof(LessonText)); } }
    public byte[] LessonImage { get { return _lessonImage; } set { _lessonImage = value; OnPropertyChanged(nameof(LessonImage)); } }
  }
  internal class LessonEditorViewModel : BaseViewModel
  {

    public List<string> lessonNames = new List<string>();
    public List<LessonData> editedLessons = new List<LessonData>();
    private BaseViewModel _selectedViewModel;
    public string _country;
    public string _language;
    public string _lesson;
    public string _title;
    public decimal _level;
    public string _editedContent;
    public byte[] _editedImage;
    public bool IsLessonBeingEdited = false;
    public int ModifiedContentId = 0;
    private ILessonRepository lessonRepository;

    public List<string> LessonNames { get { return lessonNames; } set { lessonNames = value; OnPropertyChanged(nameof(LessonNames)); } }
    public List<LessonData> EditedLessons { get { return editedLessons; } set { editedLessons = value; OnPropertyChanged(nameof(EditedLessons)); } }
    public string Country { get { return _country; } set { _country = value; OnPropertyChanged(nameof(Country)); LoadData(); } }
    public string Language { get { return _language; } set { _language = value; OnPropertyChanged(nameof(Language)); LoadData(); } }
    public string Lesson { get { return _lesson; } set { _lesson = value; OnPropertyChanged(nameof(Lesson));
        if (Lesson == "None")
        {
          EditedLessons.Clear();
          IsLessonBeingEdited = false;
        }
        else
        {
          EditedLessons = lessonRepository.Obtain_Lesson_Content(Lesson);
          IsLessonBeingEdited = true;
        }
      } }
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
    public decimal Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); LoadData(); } }
    public string EditedContent { get { return _editedContent; } set { _editedContent = value; OnPropertyChanged(nameof(EditedContent)); } }
    public byte[] EditedImage { get { return _editedImage; } set { _editedImage = value; OnPropertyChanged(nameof(EditedImage)); } }
    public ICommand ChooseCommand { get; }
    public ICommand ClearCommand { get; }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));

      }
    }
    public ICommand LessonEditorUpdateViewCommand { get; set; }
    public LessonEditorViewModel()
    {
      lessonRepository = new LessonRepository();
      LessonEditorUpdateViewCommand = new LessonEditorUpdateViewCommand(this);
      ChooseCommand = new RelayCommand(ExecuteChooseCommand, CanExecuteChooseCommand);
      ClearCommand = new RelayCommand(ExecuteClearCommand);
      LoadData();
    }
    public void LoadData()
    {
      LessonNames = lessonRepository.Obtain_Lesson_Names(Country, Language, Level);
    }
    private bool CanExecuteChooseCommand(object parameter)
    {
        return true;
    }
    public void ExecuteChooseCommand(object parameter)
    {
      var value = EditedLessons.First(x => x.LessonID == (int)parameter);
      EditedContent = value.LessonText;
      EditedImage = value.LessonImage;
      ModifiedContentId = value.LessonID;
    }
    public void ExecuteClearCommand(object obj)
    {
      EditedContent = null;
      EditedImage = null;
      ModifiedContentId = 0;
    }

  }
}
