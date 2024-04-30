using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonListModel
  {
    public int Id_Lesson { get; set; }
    public string Lesson_Title { get; set; }
    //public string Lesson_Parameter { get; set; }
    public LessonListModel(int id_Lesson, string lesson_Title)
    {
      Id_Lesson = id_Lesson;
      Lesson_Title = lesson_Title;
      //Lesson_Parameter = lesson_Parameter;
    }
  }
}
