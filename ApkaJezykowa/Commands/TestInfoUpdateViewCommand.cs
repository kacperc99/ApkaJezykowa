using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  public class TestData
  {
    public int TestId { get; set; }
    public string TestTasks { get; set; }
    public bool TestDone { get; set; }
  }
  internal class TestInfoUpdateViewCommand : ICommand
  {
    public string Lang;
    public int id;
    private TestInfoViewModel viewModel;
    private IExerciseRepository exerciseRepository;

    public TestInfoUpdateViewCommand(TestInfoViewModel viewModel,string Lang, int id)
    {
      this.Lang = Lang;
      this.viewModel = viewModel;
      exerciseRepository = new ExerciseRepository();
      this.id = id;
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "Continue")
      {
        ObservableCollection<TestData> TestingData = new ObservableCollection<TestData>();
        //TestModel.instance.TestMode = true;
        exerciseRepository.Enter_Test_Mode(id, Lang, TestingData);
        //TestModel.instance.Test_Points = 0;
        viewModel.SelectedViewModel = new ExerciseViewModel(TestingData, Lang);
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new ExerciseMenuViewModel(Lang);
      }
    }
  }
}
