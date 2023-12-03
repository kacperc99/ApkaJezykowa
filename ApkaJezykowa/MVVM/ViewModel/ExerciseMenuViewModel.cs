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
  internal class ExerciseMenuViewModel : BaseViewModel
  {
    public string Lang;
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
    public ExerciseMenuViewModel(string Lang)
    {
      this.Lang = Lang;
      FrenchExerciseMenuUpdateViewCommand = new ExerciseMenuUpdateViewCommand(this, Lang);
      exerciseRepository = new ExerciseRepository();
      LoadList();
    }
    public void LoadList()
    {
      exerciseRepository.Display_Exercise_List(ExerciseList, Lang, Properties.Settings.Default.Language);
    }
  }
}
