using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
  public class ParamModel
  {
    public string country { get; set; }
    public string language { get; set; }
    public string title { get; set; }
    public decimal level { get; set; }
    //private ParamModel() { }
    //public static readonly ParamModel Instance = new ParamModel();
  }
  internal class LessonEditorViewModel : BaseViewModel
  {

    public List<string> lessonNames = new List<string>();
    public ObservableCollection<LessonData> editedLessons = new ObservableCollection<LessonData>();
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
    public ObservableCollection<LessonData> EditedLessons { get { return editedLessons; } set { editedLessons = value; OnPropertyChanged(nameof(EditedLessons)); } }
    public string Country { get { return _country; } set { _country = value; OnPropertyChanged(nameof(Country)); if(!IsLessonBeingEdited) LoadData(); } }
    public string Language { get { return _language; } set { _language = value; OnPropertyChanged(nameof(Language)); if (!IsLessonBeingEdited) LoadData(); } }
    public string Lesson { get { return _lesson; } set { _lesson = value; OnPropertyChanged(nameof(Lesson));
        if (Lesson == "None")
        {
          EditedLessons.Clear();
          Country = "None";
          Language = "None";
          Title = null;
          Level = 0;
          IsLessonBeingEdited = false;
        }
        else
        {
          EditedLessons = lessonRepository.Obtain_Lesson_Content(Lesson);
          var param = lessonRepository.Obtain_Lesson_Parameters(Lesson);
          IsLessonBeingEdited = true;
          Country = param.country;
          Language = param.language;
          Title = param.title;
          Level = param.level;
          
        }
      } }
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
    public decimal Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); if (!IsLessonBeingEdited) LoadData(); } }
    public string EditedContent { get { return _editedContent; } set { _editedContent = value; OnPropertyChanged(nameof(EditedContent)); } }
    public byte[] EditedImage { get { return _editedImage; } set { _editedImage = value; OnPropertyChanged(nameof(EditedImage)); } }
    public ICommand ChooseCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand AddContentCommand { get; set; }
    public ICommand AddLessonCommand { get; }
    public ICommand DeleteCommand { get; }
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
      ChooseCommand = new RelayCommand(ExecuteChooseCommand);
      DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
      ClearCommand = new RelayCommand(ExecuteClearCommand);
      AddContentCommand = new RelayCommand(ExecuteAddContentCommand, CanExecuteAddContentCommand);
      AddLessonCommand = new RelayCommand(ExecuteAddLessonCommand, CanExecuteAddLessonCommand);
      LoadData();
    }
    public void LoadData()
    {
      EditedContent = "Write lesson content here";
      LessonNames = lessonRepository.Obtain_Lesson_Names(Country, Language, Level);
    }
    public void SetPath(byte[] filePath)
    {
      EditedImage = filePath;
    }
    public void ExecuteChooseCommand(object parameter)
    {
      var value = EditedLessons.First(x => x.LessonID == (int)parameter);
      EditedContent = value.LessonText;
      EditedImage = value.LessonImage;
      ModifiedContentId = value.LessonID;
    }
    public void ExecuteDeleteCommand(object parameter)
    {
      var value = EditedLessons.First(x => x.LessonID == (int)parameter);
      EditedLessons.Remove(value);
    }
    public void ExecuteClearCommand(object obj)
    {
      EditedContent = null;
      EditedImage = null;
      ModifiedContentId = 0;
    }
    private bool CanExecuteAddContentCommand(object obj)
    {
      if (!string.IsNullOrEmpty(EditedContent) && EditedContent.Length <= 4000)
      {
        return true;
      }
      else
        return false;
    }
    public void ExecuteAddContentCommand(object obj)
    {
      LessonData lsn = new LessonData();
      lsn.LessonID = EditedLessons.Count() + 1;
      lsn.LessonText = EditedContent;
      if (EditedImage != null)
        lsn.LessonImage = EditedImage;
      else
        lsn.LessonImage = null;
      EditedLessons.Add(lsn);
      foreach (LessonData p in EditedLessons) { Console.WriteLine(p.LessonText, p.LessonImage, p.LessonID); }
    }
    private bool CanExecuteAddLessonCommand(object obj)
    {
      if ((EditedLessons.Count() > 0 && Country !="None" && Language!="None" && Title!=null && Level>0) || Lesson!="None")
        return true;
      else
        return false;
    }
    public void ExecuteAddLessonCommand(object obj)
    {

    }

  }
}
