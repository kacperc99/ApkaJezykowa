using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        command.CommandText = "select L.Id_Lesson, L.Lesson_Level, LT.Lesson_Title, L.Id_Course from [Lesson_Title] LT join [Lesson] L on LT.Id_Lesson = L.Id_Lesson where Lesson_Level=@level and Id_Course =(Select Id_Course from [Course] where [Course_Name] = @language and Course_Level = @level)";
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
    public void Obtain_Lesson_List(List<LessonListModel> LessonsList, string Language)
    {
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select LT.Lesson_Title, L.Lesson_Parameter from [Lesson_Title] LT join [Lesson] L on LT.Id_Lesson=L.Id_Lesson where L.Id_Course in (Select Id_Course from [Course] where [Course_Name] = @language)";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonsList.Add(new LessonListModel(reader["Lesson_Title"].ToString(), reader["Lesson_Parameter"].ToString()));
          }
          reader.NextResult();
        }
        //zastanawia mnie czy nie lepiej byłoby przerobić tą funkcję na List<LessonListmModel> Obtain_Lesson_List
      }
    }
    public void Obtain_Pars(List<string> pars, string Language)
    {
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Parameter from [Lesson] where Id_Course in (Select Id_Course from [Course] where [Course_Name] = @language)";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            pars.Add(reader["Lesson_Parameter"].ToString());
          }
          reader.NextResult();
        }
      }
    }
    public void Obtain_Lessons(List<LessonContentModel> Lessons, int Id)
    {
      using(var connection = GetConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection= connection;
        command.CommandText = "SELECT Lesson_Text, Lesson_Image FROM [Lesson_Content] WHERE Id_Lesson_Title = (SELECT Id_Lesson FROM [Lesson_Title] WHERE Id_Lesson = (SELECT Id_Lesson FROM [Lesson] WHERE Id_Lesson=@Id))";
        command.Parameters.Add("@Id", SqlDbType.Int).Value=Id;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            LessonContentModel model = new LessonContentModel();
            model.LessonText = reader["Lesson_Text"].ToString();
            if(reader["Lesson_Image"]!=System.DBNull.Value)
              model.LessonImage = (byte[])reader["Lesson_Image"];
            else
              model.LessonImage = null;
            Lessons.Add(model);
          }
          reader.NextResult();
        }
      }
    }
    public List<string> Obtain_Lesson_Names(string Country, string Language, decimal Level)
    {
      if (Country == "None")
        Country = null;
      if (Language == "None")
        Language = null;
      Nullable<decimal> DecimalLevel = Level;
      if (DecimalLevel == 0)
        DecimalLevel = null;
      List<string> ts = new List<string>();
      ts.Add("None");
      using(var connection = GetConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Title from [Lesson_Title] where Id_Lesson in (select Id_Lesson from Lesson where Lesson_Level = Coalesce(@level,Lesson_Level) and Id_Course in (select Id_Course from [Course] where [Course_Name] = Coalesce(@country,[Course_Name]) and Course_Level = Coalesce(@level,Course_Level))) and Lesson_Language = Coalesce(@language,Lesson_Language)";
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country ?? (object)DBNull.Value;
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language ?? (object)DBNull.Value;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = DecimalLevel ?? (object)DBNull.Value;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            ts.Add(reader["Lesson_Title"].ToString());
          }
        }
      }
      return ts;
    }
    public ParamModel Obtain_Lesson_Parameters(string LName)
    {
      ParamModel result = null;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select C.[Course_Name], L.Lesson_Level, LT.Lesson_Language, LT.Lesson_Title From[Course] C Join[Lesson] L on C.Id_Course = L.Id_Course Join[Lesson_Title] LT on L.Id_Lesson = LT.Id_Lesson Where LT.Lesson_Title=@title";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = LName;
        using(var reader = command.ExecuteReader())
        {
          if(reader.Read())
          {
            result = new ParamModel()
            {
              country = reader["Course_Name"].ToString(),
              language = reader["Lesson_Language"].ToString(),
              title = reader["Lesson_Title"].ToString(),
              level = (decimal)reader["Lesson_Level"]
            };
          }
        }
      }
      return result;
    }
    public ObservableCollection<LessonData> Obtain_Lesson_Content(string Lesson)
    {
      /*using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select C.[Course_Name], L.Lesson_Level, LT.Lesson_Language, LT.Lesson_Title From[Course] C Join[Lesson] L on C.Id_Course = L.Id_Course Join[Lesson_Title] LT on L.Id_Lesson = LT.Id_Lesson Where LT.Lesson_Title=@title";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Lesson;
        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            ParamModel.Instance.country = reader["Course_Name"].ToString();
            ParamModel.Instance.language = reader["Lesson_Language"].ToString();
            ParamModel.Instance.title = reader["Lesson_Title"].ToString();
            ParamModel.Instance.level = (decimal)reader["Lesson_Level"];
          }
        }
      }*/
      ObservableCollection<LessonData> lc = new ObservableCollection<LessonData>();
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "SELECT Id_Lesson_Content, Lesson_Text, Lesson_Image FROM [Lesson_Content] WHERE Id_Lesson_Title = (SELECT Id_Lesson FROM [Lesson_Title] WHERE Lesson_Title = @title)";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Lesson;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonData model = new LessonData();
            model.LessonID = (int)reader["Id_Lesson_Content"];
            model.LessonText = reader["Lesson_Text"].ToString();
            if (reader["Lesson_Image"] != System.DBNull.Value)
              model.LessonImage = (byte[])reader["Lesson_Image"];
            else
              model.LessonImage = null;
            lc.Add(model);
          }
          reader.NextResult();
        }
        ///foreach (LessonData p in lc) { Console.WriteLine(p.LessonID, p.LessonText, p.LessonImage); }
        return lc;
      }
    }

  }
}
