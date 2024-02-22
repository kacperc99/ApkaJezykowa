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
using System.Web.UI.WebControls;
using System.Windows.Input;
//things to solve: 
// - limit the level variable to current max level +1
// - allow user to only edit lessons of the courses they finished
namespace ApkaJezykowa.MVVM.ViewModel
{
  public class LessonImagesData : BaseViewModel
  {
    public int _imageID;
    public byte[] _image;
    public string _description;

    public int ImageID { get { return _imageID; } set { _imageID = value; OnPropertyChanged(nameof(ImageID)); } }
    public byte[] Image { get { return _image; } set { _image = value; OnPropertyChanged(nameof(Image)); } }
    public string Description { get { return _description; } set { _description = value; OnPropertyChanged(nameof(Description)); } }
  }
  public class LessonData : BaseViewModel
  {
    public int _lessonID;
    public string _lessonText;
    public ObservableCollection<LessonImagesData> _lessonImage;

    public int LessonID { get { return _lessonID; } set { _lessonID = value; OnPropertyChanged(nameof(LessonID)); } }
    public string LessonText { get { return _lessonText; } set { _lessonText = value; OnPropertyChanged(nameof(LessonText)); } }
    public ObservableCollection<LessonImagesData> LessonImage { get { return _lessonImage; } set { _lessonImage = value; OnPropertyChanged(nameof(LessonImage)); } }
  }
  public class LessonParamModel
  {
    public int CourseID { get; set; }
    public int Id {  get; set; }
    public int TitleId {  get; set; }
    public string country { get; set; }
    public string language { get; set; }
    public string title { get; set; }
    public decimal level { get; set; }
    //private LessonParamModel() { }
    //public static readonly LessonParamModel Instance = new LessonParamModel();
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
    public ObservableCollection<LessonImagesData> _editedImages = new ObservableCollection<LessonImagesData>();
    public bool IsLessonBeingEdited = false;
    public bool IsContentBeingEdited = false;
    public int ModifiedContentId = 0;
    public int ModifiedTitleID = 0;
    public int ModifiedLessonId = 0;
    public int ModifiedImageId = 1;
    public int CourseID;
    public string _errorMessage;
    public bool _enabler;
    public string OldTitle;
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
          CourseID = 0;
          IsLessonBeingEdited = false;
          Enabler = true;
        }
        else
        {
          //this can be also solved by using ref/out
          EditedLessons = lessonRepository.Obtain_Lesson_Content(Lesson);
          var param = lessonRepository.Obtain_Lesson_Parameters(Lesson);
          IsLessonBeingEdited = true;
          Country = param.country;
          Language = param.language;
          Title = param.title;
          OldTitle = param.title;
          Level = param.level;
          CourseID = param.CourseID;
          ModifiedLessonId = param.Id;
          ModifiedTitleID = param.TitleId;
          Enabler = false;
        }
      } }
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
    public decimal Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); if (!IsLessonBeingEdited) LoadData(); } }
    public string EditedContent { get { return _editedContent; } set { _editedContent = value; OnPropertyChanged(nameof(EditedContent)); } }
    public ObservableCollection<LessonImagesData> EditedImages { get { return _editedImages; } set { _editedImages = value; OnPropertyChanged(nameof(EditedImages)); } }
    public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value;OnPropertyChanged(nameof(ErrorMessage)); } } 
    public bool Enabler { get { return _enabler; } set { _enabler = value; OnPropertyChanged(nameof(Enabler)); } }
    public ICommand ChooseCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand AddContentCommand { get; set; }
    public ICommand AddLessonCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
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
      LoadComm();
    }
    public void LoadData()
    {
      LessonNames = lessonRepository.Obtain_Lesson_Names(Country, Language, Level);
    }
    public void LoadComm() 
    {
      EditedContent = "Write lesson content here";
    }
    public void SetPath(byte[] filePath)
    {
      LessonImagesData data = new LessonImagesData();
      data.ImageID = ModifiedImageId;
      data.Image = filePath;
      data.Description = "";
      EditedImages.Add(data);
      ModifiedImageId++;
      foreach(var x in EditedImages) { Console.WriteLine(x.Description, x.Image, x.ImageID); }
    }
    public void ExecuteChooseCommand(object parameter)
    {
      var value = EditedLessons.First(x => x.LessonID == (int)parameter);
      EditedContent = value.LessonText;
      if (value.LessonImage != null)
      {
        EditedImages = new ObservableCollection<LessonImagesData>(value.LessonImage);
      }
      else
        EditedImages = null;
      //EditedImages = value.LessonImage;
      ModifiedContentId = value.LessonID;
      IsContentBeingEdited = true;
    }
    public void ExecuteDeleteCommand(object parameter)
    {
      var value = EditedLessons.First(x => x.LessonID == (int)parameter);
      EditedLessons.Remove(value);
    }
    public void ExecuteClearCommand(object obj)
    {
      EditedContent = null;
      EditedImages.Clear();
      ModifiedContentId = 0;
    }
    private bool CanExecuteAddContentCommand(object obj)
    {
      if (!string.IsNullOrEmpty(EditedContent) && EditedContent.Length <= 4000)
      {
        return true;
      }
      else
      {
        ErrorMessage = "Przekroczono maksymalną ilość znaków/brak treści";
        return false;
      } 
    }
    public void ExecuteAddContentCommand(object obj)
    {
      if (!IsContentBeingEdited)
      {
        LessonData lsn = new LessonData();
        lsn.LessonID = EditedLessons.Count() + 1;
        lsn.LessonText = EditedContent;
        if (EditedImages != null)
        {
            lsn.LessonImage = new ObservableCollection<LessonImagesData>(EditedImages);
        }
        else
          lsn.LessonImage = null;
        foreach(var x in lsn.LessonImage) 
        { Console.WriteLine(x.Description, x.Image); }
        EditedLessons.Add(lsn);
        EditedContent = null;
        EditedImages.Clear();
        //foreach (LessonData p in EditedLessons) { Console.WriteLine(p.LessonText, p.LessonImage, p.LessonID); }
      }  
      if(IsContentBeingEdited)
      {
        var value = EditedLessons.First(x => x.LessonID == ModifiedContentId);
        int i = EditedLessons.IndexOf(value);
        EditedLessons[i].LessonText = EditedContent;
        if (EditedImages != null)
        {
          EditedLessons[i].LessonImage = new ObservableCollection<LessonImagesData>(EditedImages);
        }
        else
          EditedLessons[i].LessonImage = null;
        //EditedLessons[i].LessonImage = EditedImages;
        EditedContent = null;
        EditedImages.Clear();
        ModifiedContentId = 0;
        IsContentBeingEdited = false;
      }
    }
    private bool CanExecuteAddLessonCommand(object obj)
    {
      if ((EditedLessons.Count() > 0 && Country != "None" && Language != "None" && Title != null && Level > 0) || Lesson != "None")
        return true;
      else
      {
        ErrorMessage = "Nie podano brakujących informacji";
        return false;
      }
    }
    public void ExecuteAddLessonCommand(object obj)
    {
      if(!IsLessonBeingEdited)
      {
        lessonRepository.AddLesson(Country, Language, EditedLessons, Title, Level);
        EditedLessons.Clear();
        Country = "None";
        Language = "None";
        Title = null;
        Level = 0;
        EditedContent = null;
        EditedImages.Clear();
        ModifiedContentId = 0;
        ErrorMessage = "Dodano podaną lekcję!";
      }
      if(IsLessonBeingEdited)
      {
        lessonRepository.UpdateLesson(Country, Language, EditedLessons, OldTitle, Title, Level, CourseID, ModifiedLessonId, ModifiedTitleID);
        EditedLessons.Clear();
        Country = "None";
        Language = "None";
        Title = null;
        Level = 0;
        EditedContent = null;
        EditedImages.Clear();
        ModifiedContentId = 0;
        IsLessonBeingEdited = false;
        Enabler = true;
        ErrorMessage = "zaktualizowano podaną lekcję!";
      }
    }

  }
}
