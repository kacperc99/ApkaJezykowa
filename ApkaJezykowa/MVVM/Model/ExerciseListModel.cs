using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ExerciseListModel
  {
    public int Id_Exercise { get; set; }
    public decimal Exercise_Level { get; set; }
    public string Exercise_Title { get; set; }
    //public string Exercise_Parameter { get; set; }
    public string Task_Text { get; set; }
  }
}
