using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class ExerciseModel
  {
    public string Id { get; set; }
    public decimal ExerciseLevel { get; set; }
    public string TaskText { get; set; }
    public List<string> Exercise { get; set; }
    public List<string> Answer { get; set; }
    public List<string> Tip { get; set; }
    public string Id_Course { get; set; }
    //List<Exercise> Exercises { get; set; } = new List<Exercise>();
    private ExerciseModel() { }
    public static readonly ExerciseModel Instance = new ExerciseModel();
  }
}
