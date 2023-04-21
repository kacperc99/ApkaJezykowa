using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  internal class LessonRepository : BaseRepository, ILessonRepository
  {
    public LessonModel Display(int Level, string Language)
    {
      LessonModel lesson = null;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id, Lesson_Level, Lesson_Title, Id_Course from [Lesson] where Lesson_Level=@level and Id_Course =(Select Id from [Course] where [Course_Name] = @language and Course_Level = @level)";
        command.Parameters.Add("@level", SqlDbType.Int).Value = Level;
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            lesson = new LessonModel()
            {
              Id = (int)reader[0],
              Lesson_Level = (decimal)reader[1],
              Lesson_Title = reader[2].ToString(),
              Id_Course = reader[3].ToString(),
            };
          }
        }
      }
      return lesson;
    }
    public List<LessonListModel> Obtain_Lesson_List(string Language)
    {
      List<LessonListModel> LessonLista = new List<LessonListModel>();
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Title, Lesson_Parameter from [Lesson] where Id_Course in (Select Id from [Course] where [Course_Name] = @language)";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonLista.Add(new LessonListModel(reader["Lesson_Title"].ToString(), reader["Lesson_Parameter"].ToString()));
          }
          reader.NextResult();
          LessonLista.ForEach(x => Console.WriteLine("{0}\t", x.ToString()));
        }
        return LessonLista;
      }
    }
  }
}
