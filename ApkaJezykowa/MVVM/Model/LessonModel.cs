using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class LessonModel
  {
    public string Id { get; set; }
    public decimal Lesson_Level { get; set; }
    public string Lesson_Title { get; set; }
    public string Lesson_Text { get; set; }
    public string Id_Course { get; set; }
  }
}
