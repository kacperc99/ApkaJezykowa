using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ExerciseLevelModel
  {
    public int Level { get; set; }
    public string Language { get; set; }
    public int id { get; set; }
    public string Task_text { get; set; }
    private ExerciseLevelModel() { }
    public static readonly ExerciseLevelModel Instance = new ExerciseLevelModel();
  }
}
