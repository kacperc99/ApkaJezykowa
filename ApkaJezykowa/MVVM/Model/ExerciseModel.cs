using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ExerciseModel
  {
    public int Id { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Tip { get; set; }
    public int Id_Exercise { get; set; }
  }
}
