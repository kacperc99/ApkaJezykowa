using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class ExerciseUpdateViewCommand : ICommand
  {
    public int Points;
    public bool IsTesting;
    public string Lang;
    public ObservableCollection<TestData> TestingData = new ObservableCollection<TestData>();
    private ExerciseViewModel viewModel;

    public ExerciseUpdateViewCommand(ExerciseViewModel viewModel, bool IsTesting, string Lang)
    {
      this.IsTesting = IsTesting;
      this.viewModel = viewModel;
      this.Lang = Lang;
    }
    public ExerciseUpdateViewCommand(ExerciseViewModel viewModel, int Points, bool IsTesting, ObservableCollection<TestData> TestingData, string Lang)
    {
      this.Points = Points;
      this.IsTesting = IsTesting;
      this.viewModel = viewModel;
      this.TestingData = TestingData;
      this.Lang = Lang;
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if(parameter.ToString() == "NextTask" && IsTesting==true)
      {
        //viewModel.SelectedViewModel = new ExerciseViewModel(TestingData, Lang, Points);
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        Console.WriteLine("Że to niby działa?");
        if (IsTesting)
          IsTesting = false;
        viewModel.SelectedViewModel = new ExerciseMenuViewModel(Lang);
      }
    }
  }
}
