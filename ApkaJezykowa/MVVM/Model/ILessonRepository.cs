using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface ILessonRepository
  {
    LessonModel Display(int Level, string Language, string Lesson_Language);
    //string GetTitle(int Id, string Language);
    List<LessonListModel> Obtain_Lesson_List(string Language, string Lesson_Language);
    //decimal Obtain_Level(int Id, string Language);
    void Obtain_Lessons(List<LessonContentModel> Lessons, int TitleId);
    List<string> Obtain_Lesson_Names(string Country, string Language, int Level);
    LessonParamModel Obtain_Lesson_Parameters(string LNameguage);
    ObservableCollection<LessonData> Obtain_Lesson_Content(string Lesson);
    void AddLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string Title, int Level);
    void UpdateLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string OldTitle, string Title, int Level, int CourseID, int Lesson_Id, int Lesson_Title_Id);
    ObservableCollection<Clicker> GetButtons();
    byte[] GetIcon(string Lang);
  }
}
