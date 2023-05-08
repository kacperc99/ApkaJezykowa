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
  internal class FrenchExerciseMenuViewModel : BaseViewModel
  {
    List<ExerciseListModel> exerciseList = new List<ExerciseListModel>();
    private BaseViewModel _selectedViewModel;
    private IExerciseRepository exerciseRepository;

    public List<ExerciseListModel> ExerciseList { get { return exerciseList; } set { exerciseList = value; OnPropertyChanged(nameof(ExerciseList)); } }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));
      }
    }
    public ICommand FrenchExerciseMenuUpdateViewCommand { get; set; }
    public FrenchExerciseMenuViewModel()
    {
      FrenchExerciseMenuUpdateViewCommand = new FrenchExerciseMenuUpdateViewCommand(this);
      exerciseRepository = new ExerciseRepository();
      LoadList();
    }
    public void LoadList()
    {
      exerciseRepository.Display_Exercise_List(ExerciseList, ExerciseLevelModel.Instance.Language);
      foreach (ExerciseListModel p in ExerciseList) { Console.WriteLine(p.Exercise_Level.ToString(), p.Exercise_Title, p.Exercise_Parameter, p.Task_Text); }
    }
  }
}
