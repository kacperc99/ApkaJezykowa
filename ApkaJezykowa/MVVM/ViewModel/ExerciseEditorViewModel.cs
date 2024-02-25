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
  public class ExerciseData : BaseViewModel
  {
    public int _exercise_Content_Id;
    public string _task;
    public string _answer1;
    public string _answer2;
    public string _answer3;
    public string _tip;

    public int Exercise_Content_Id { get { return _exercise_Content_Id; } set { _exercise_Content_Id = value; OnPropertyChanged(nameof(Exercise_Content_Id)); } }
    public string Task { get { return _task; } set { _task = value; OnPropertyChanged(nameof(Task)); } }
    public string Answer1 { get { return _answer1; } set { _answer1 = value; OnPropertyChanged(nameof(Answer1)); } }
    public string Answer2 { get { return _answer2; } set { _answer2 = value; OnPropertyChanged(nameof(Answer2)); } }
    public string Answer3 { get { return _answer3; } set { _answer3 = value; OnPropertyChanged(nameof(Answer3)); } }
    public string Tip { get { return _tip; } set { _tip = value; OnPropertyChanged(nameof(Tip)); } }
  }

  public class ExerciseParamModel
  {
    public int courseId { get; set; }
    public int exerciseID {  get; set; }
    public string country { get; set; }
    public string language { get; set; }
    public string title {  get; set; }
    public string task_Text { get; set; }
    public decimal level { get; set; }
  }
  public class ExerciseEditorViewModel : BaseViewModel
  {
    public List<string> exerciseNames = new List<string>();
    public ObservableCollection<ExerciseData> editedExercises = new ObservableCollection<ExerciseData>();
    private BaseViewModel _selectedViewModel;
    public string _country;
    public string _language;
    public string _exercise;
    public string _title;
    public decimal _level;
    public string _taskText;
    public string _task;
    public string _answer1;
    public string _answer2;
    public string _answer3;
    public string _tip;
    public bool IsExerciseBeingEdited = false;
    public bool IsTaskBeingEdited = false;
    public int ModifiedExerciseId = 0;
    public int ModifiedTaskId = 0;
    public int CourseID;
    public string _errorMessage;
    public bool _enabler;
    public string OldTitle;
    public string OldTaskText;
    private IExerciseRepository exerciseRepository;

    public List<string> ExerciseNames { get { return exerciseNames; } set { exerciseNames = value; OnPropertyChanged(nameof(ExerciseNames)); } }
    public ObservableCollection<ExerciseData> EditedExercises { get {  return editedExercises; } set {  editedExercises = value;OnPropertyChanged(nameof(EditedExercises)); } }
    public string Country { get { return _country; } set { _country = value; OnPropertyChanged(nameof(Country)); if (!IsExerciseBeingEdited) LoadData(); } }
    public string Language { get { return _language; } set { _language = value; OnPropertyChanged(nameof(Language)); if (!IsExerciseBeingEdited) LoadData(); } }
    public string Exercise { get { return _exercise; } set { _exercise = value; OnPropertyChanged(nameof(Exercise)); 
      if(Exercise == "None")
      {
        EditedExercises.Clear();
        Country = "None";
        Language = "None";
        TaskText = null;
        Title = null;
        Level = 0;
        CourseID = 0;
        IsExerciseBeingEdited = false;
        Enabler = true;
      }
      else
      {
          EditedExercises = exerciseRepository.Obtain_Exercise_Content(Exercise);
          var param = exerciseRepository.Obtain_Exercise_Parameters(Exercise);
          IsExerciseBeingEdited = true;
          Country = param.country;
          Language = param.language;
          Title = param.title;
          OldTitle = param.title;
          TaskText = param.task_Text;
          Level = param.level;
          CourseID = param.courseId;
          ModifiedExerciseId = param.exerciseID;
          Enabler = false;
        }
      } }
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
    public decimal Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); if (!IsExerciseBeingEdited) LoadData(); } }
    public string TaskText {  get { return _taskText; } set { _taskText = value; OnPropertyChanged(nameof(TaskText)); } }
    public string Task { get { return _task; } set { _task = value; OnPropertyChanged(nameof(Task)); } }
    public string Answer1 { get { return _answer1; } set { _answer1 = value; OnPropertyChanged(nameof(Answer1)); } }
    public string Answer2 { get { return _answer2; } set { _answer2 = value; OnPropertyChanged(nameof(Answer2)); } }
    public string Answer3 { get { return _answer3; } set { _answer3 = value; OnPropertyChanged(nameof(Answer3)); } }
    public string Tip { get { return _tip; } set { _tip = value; OnPropertyChanged(nameof(Tip)); } }
    public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
    public bool Enabler { get { return _enabler; } set { _enabler = value; OnPropertyChanged(nameof(Enabler)); } }
    public ICommand ChooseCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand AddTaskCommand { get; set; }
    public ICommand AddExerciseCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public BaseViewModel SelectedViewModel { get {  return _selectedViewModel; } set { _selectedViewModel = value;OnPropertyChanged(nameof(SelectedViewModel)); } }
    public ICommand ExerciseEditorUpdateViewCommand { get; set; }
    public ExerciseEditorViewModel()
    {
      exerciseRepository = new ExerciseRepository();
      ExerciseEditorUpdateViewCommand = new ExerciseEditorUpdateViewCommand(this);
      ChooseCommand = new RelayCommand(ExecuteChooseCommand);
      DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
      ClearCommand = new RelayCommand(ExecuteClearCommand);
      AddTaskCommand = new RelayCommand(ExecuteAddTaskCommand, CanExecuteAddTaskCommand);
      AddExerciseCommand = new RelayCommand(ExecuteAddExerciseCommand, CanExecuteAddExerciseCommand);
      LoadData();
    }
    public void LoadData()
    {
      ExerciseNames = exerciseRepository.Obtain_Exercise_Names(Country, Language, Level);
    }
    public void ExecuteChooseCommand(object parameter)
    {
      var value = EditedExercises.First(x=>x.Exercise_Content_Id == (int)parameter);
      Task = value.Task;
      Answer1 = value.Answer1;
      Answer2 = value.Answer2;
      Answer3 = value.Answer3;
      Tip = value.Tip;
      ModifiedTaskId = value.Exercise_Content_Id;
      IsTaskBeingEdited = true;
    }
    public void ExecuteDeleteCommand(object parameter) 
    {
      var value = EditedExercises.First(x => x.Exercise_Content_Id == (int)parameter);
      EditedExercises.Remove(value);
    }
    public void ExecuteClearCommand(object obj)
    {
      Task = null;
      Answer1 = null;
      Answer2 = null;
      Answer3 = null;
      Tip = null;
      ModifiedTaskId = 0;
    }
    public bool CanExecuteAddTaskCommand(object obj)
    {
      if(!string.IsNullOrEmpty(Task) && Task.Length <=200)
      {
        if (!string.IsNullOrEmpty(Answer1) && Answer1.Length <= 50)
        {
          if ((Answer2 != null && Answer2.Length <= 50) || Answer2 == null)
          {
            if ((Answer3 != null && Answer3.Length <= 50) || Answer3 == null)
            {
              if (!string.IsNullOrEmpty(Tip) && Tip.Length <= 200)
              {
                return true;
              }
              else
                ErrorMessage = "Przekroczono maksymalną ilość znaków/brak treści w polu wskazówki";
            }
            else
              ErrorMessage = "Przekroczono maksymalną ilość znaków w polu odpowiedzi nr 3";
          }
          else
            ErrorMessage = "Przekroczono maksymalną ilość znaków w polu odpowiedzi nr 2";
        }
        else
          ErrorMessage = "Przekroczono maksymalną ilość znaków/brak treści w polu odpowiedzi nr 1";
      }
      else
        ErrorMessage = "Przekroczono maksymalną ilość znaków/brak treści w polu zadanie";
      return false;
    }
    public void ExecuteAddTaskCommand(object obj)
    {
      if(!IsTaskBeingEdited)
      {
        ExerciseData ex = new ExerciseData();
        ex.Exercise_Content_Id = 0;
        ex.Task = Task;
        ex.Answer1 = Answer1;
        ex.Answer2 = Answer2;
        ex.Answer3 = Answer3;
        ex.Tip = Tip;
        EditedExercises.Add(ex);
        Task = null;
        Answer1 = null;
        Answer2 = null;
        Answer3 = null;
        Tip = null;
      }
      if(IsTaskBeingEdited)
      {
        var value = EditedExercises.First(x => x.Exercise_Content_Id == ModifiedTaskId);
        int i = EditedExercises.IndexOf(value);
        EditedExercises[i].Task = Task;
        EditedExercises[i].Answer1 = Answer1;
        EditedExercises[i].Answer2 = Answer2;
        EditedExercises[i].Answer3 = Answer3;
        EditedExercises[i].Tip = Tip;
        Task = null;
        Answer1 = null;
        Answer2 = null;
        Answer3 = null;
        Tip = null;
        ModifiedTaskId = 0;
        IsTaskBeingEdited = false;
      }
    }
    public bool CanExecuteAddExerciseCommand(object obj)
    {
      if ((EditedExercises.Count > 0 && Country != "None" && Language != "None" && Title != null && TaskText != null && Level > 0) || Exercise != "None")
        if(exerciseRepository.DoesLessonExist(Country, Language, Level))
          return true;
        else
        {
          ErrorMessage = "Nie istnieje lekcja na danym poziomie!";
          return false;
        }
      else
      {
        ErrorMessage = "Nie podano brakujących informacji";
        return false;
      }
    }
    public void ExecuteAddExerciseCommand(object obj)
    {
      if(!IsExerciseBeingEdited)
      {
        exerciseRepository.AddExercise(Country, Language, EditedExercises, Title, Level, TaskText);
        EditedExercises.Clear();
        Country = "None";
        Language = "None";
        Title = null;
        Level = 0;
        ModifiedTaskId = 0;
        ErrorMessage = "Dodano podane ćwiczenie!";
      }
      if(IsExerciseBeingEdited)
      {
        exerciseRepository.EditExercise(Country, Language, EditedExercises, TaskText, OldTitle, Title, Level, CourseID, ModifiedExerciseId);

      }
    }
  }
}
