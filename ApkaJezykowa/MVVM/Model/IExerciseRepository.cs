using ApkaJezykowa.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface IExerciseRepository
  {
    void Display(List<ExerciseModel> TaskList, int Id);
    void Display_Exercise_List(List<ExerciseListModel> ExerciseList, string Language);
    void Obtain_Pars(List<Pars> pars, string Language);
  }
}
