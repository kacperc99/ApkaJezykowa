using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonListModel
  {
    public string Lesson_Title { get; set; }
    public string Lesson_Parameter { get; set; }
    public LessonListModel(string lesson_Title, string lesson_Parameter)
    {
      Lesson_Title = lesson_Title;
      Lesson_Parameter = lesson_Parameter;
    }
  }
}
