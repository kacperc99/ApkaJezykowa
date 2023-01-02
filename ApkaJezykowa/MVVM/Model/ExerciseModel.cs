using System;
using System.Collections.Generic;
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
    public string Task { get; set; }
    public string Answer { get; set; }
    public string Tip { get; set; }
    public string Id_Course { get; set; }
  }
}
