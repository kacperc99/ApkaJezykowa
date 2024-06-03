using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonModel
  {
    public int Id { get; set; }
    public int Lesson_Level { get; set; }
    public string Lesson_Title { get; set; }
    public int Id_Course { get; set; }
  }
}
