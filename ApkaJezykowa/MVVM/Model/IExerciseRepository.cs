using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface IExerciseRepository
  {
    void Display(ObservableCollection<ExerciseModel> Exercises, int Id);
    void Display_Exercise_List(List<ExerciseListModel> ExerciseList, string Language, string Country);
    void Obtain_Pars(List<Pars> pars, string Language);
    void Enter_Test_Mode(int Id, string Language, ObservableCollection<TestData> TestingData);
    List<string> Obtain_Exercise_Names(string Country, string Language, decimal Level);
    ObservableCollection<ExerciseData> Obtain_Exercise_Content(string Exercise);
    ExerciseParamModel Obtain_Exercise_Parameters(string Exercise);
  }
}
