using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface ILessonRepository
  {
    LessonModel Display(int Level, string Language);
    void Obtain_Lesson_List(List<LessonListModel> LessonsList, string Language);
    void Obtain_Pars(List<string> pars, string Language);
    void Obtain_Lessons(List<LessonContentModel> Lessons, int Id);
  }
}
