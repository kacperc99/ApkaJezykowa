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
    ObservableCollection<ExerciseContentModel> Display(int Id);
    List<ExerciseListModel> Display_Exercise_List(string Language, string Country);
    List<Pars> Obtain_Pars(string Language);
    ObservableCollection<TestData> Enter_Test_Mode(int Id, string Language);
    List<string> Obtain_Exercise_Names(string Country, string Language, int Level);
    ObservableCollection<ExerciseData> Obtain_Exercise_Content(string Exercise);
    ExerciseParamModel Obtain_Exercise_Parameters(string Exercise);
    bool DoesLessonExist(string Country, string Language, int Level);
    void AddExercise(string Country, string Language, ObservableCollection<ExerciseData> EditedExercises, string Title, int Level, string TaskText);
    void EditExercise(string Country, string Language, ObservableCollection<ExerciseData> EditedExercises, string TaskText, string OldTitle, string Title, int Level, int CourseID, int Exercise_Id);
  }
}
