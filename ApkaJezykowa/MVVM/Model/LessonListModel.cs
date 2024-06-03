using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonListModel
  {
    public int Id_Lesson_Title { get; set; }
    public string Lesson_Title { get; set; }
    public decimal Lesson_Level {  get; set; }
    //public string Lesson_Parameter { get; set; }
    /*public LessonListModel(int id_Lesson_Title, string lesson_Title, decimal lesson_Level)
    {
      Id_Lesson_Title = id_Lesson_Title;
      Lesson_Title = lesson_Title;
      Lesson_Level = lesson_Level;
      //Lesson_Parameter = lesson_Parameter;
    }*/
  }
}
