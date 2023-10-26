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
    public void AddLesson(string Country, string Language, ObservableCollection<LessonData>EditedLessons, string Title, decimal Level)
    {
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Course from [Course] where Course_Level=@level and Course_Language=@country";
        int? CourseID = System.Convert.ToInt32(command.ExecuteScalar());
        if(CourseID != null)
        {
          command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
          int? LessonID = System.Convert.ToInt32(command.ExecuteScalar());
          if (LessonID !=null)
          {
            command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@Language and Id_Lesson=@id";
            command.Parameters.Add("@language", SqlDbType.Decimal).Value = Language;
            command.Parameters.Add("@id", SqlDbType.NVarChar).Value = LessonID;
            bool DoesLessonExist = command.ExecuteScalar() == null ? false : true;
            if (DoesLessonExist)
            {
              decimal levl = 0;
              command.CommandText = "select MAX(Lesson_Level) from [Lesson]";
              using (var reader = command.ExecuteReader())
                if (reader.Read())
                  levl = (decimal)reader["Lesson_Level"];
              command.CommandText = "select Id_Course from [Course] where Course_Level=@level and Course_Language=@country";
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Language;
              if (command.ExecuteScalar() == null)
              {
                command.CommandText = "insert into [Course] values(@country, @level)";
                command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
                command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
                command.ExecuteNonQuery();
              }
              while (levl != Level)
              {
                command.CommandText = "update [Lesson] set Lesson_Level=@levelup, Lesson_Parameter=@param, Id_Course = (select Id_Course from [Course] where Course_Level=@levelup and Course_Language=@country) where Lesson_Language=@level and Id_Course=(select Id_Course from [Course] where Course_Level=@level and Course_Language=@country)";
                command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl + 1;
                command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (levl + 1).ToString();
                command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl;
                command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
                command.ExecuteNonQuery();
                levl--;
              }
              command.CommandText = "insert into [Lesson] (Lesson_Level, Lesson_Parameter, Id_Course) select @level, @param, Course.Id_Course from [Course] where Course_Level=@level and Course_Language=@country";
              command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + Level.ToString();
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
              command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, Lesson.Id_Lesson from [Lesson] where Id_Course=(select Id_Course from [Course] where Course_Level=@level and Course_Language=@country)";
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
              command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Id_Lesson = (select Id_Lesson from [Lesson] where Id_Course=(select Id_Course from [Course] where Course_Level=@level and Course_Language=@country))";
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              int Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
              foreach (var x in EditedLessons)
              {
                command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
                command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
                command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
                command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
                command.ExecuteNonQuery();
              }
            }
            if (!DoesLessonExist)
            {
              command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, @id";
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
              command.Parameters.Add("@id", SqlDbType.Int).Value = LessonID;
              command.ExecuteNonQuery();
              command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Id_Lesson = @id";
              command.Parameters.Add("@id", SqlDbType.Int).Value = LessonID;
              int Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
              foreach (var x in EditedLessons)
              {
                command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
                command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
                command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
                command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
                command.ExecuteNonQuery();
              }
            }
          }
          if (LessonID==null)
          {
            command.CommandText = "insert into [Lesson] (Lesson_Level, Lesson_Parameter, Id_Course) select @level, @param, @id";
            command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + Level.ToString();
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            command.ExecuteNonQuery();
            command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, Lesson.Id_Lesson from [Lesson] where Id_Course=@id";
            command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
            command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            command.ExecuteNonQuery();
            command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Id_Lesson = (select Id_Lesson from [Lesson] where Id_Course=@id)";
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            int Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
            foreach (var x in EditedLessons)
            {
              command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
              command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
              command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
              command.ExecuteNonQuery();
            }
          }
        }
        if(CourseID==null)
        {
          command.CommandText = "insert into [Course] values(@country, @level)";
          command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.ExecuteNonQuery();
          command.CommandText = "select Id_Course from [Course] where Course_Level=@level and Course_Language=@country";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Language;
          CourseID = System.Convert.ToInt32(command.ExecuteScalar());
          command.CommandText = "insert into [Lesson] (Lesson_Level, Lesson_Parameter, Id_Course) select @level, @param, @id";
          command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + Level.ToString();
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          command.ExecuteNonQuery();
          command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, Lesson.Id_Lesson from [Lesson] where Id_Course=@id";
          command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
          command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          command.ExecuteNonQuery();
          command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Id_Lesson = (select Id_Lesson from [Lesson] where Id_Course=@id)";
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          int Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
          foreach (var x in EditedLessons)
          {
            command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
            command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
            command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
            command.ExecuteNonQuery();
          }
        }
        /*command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=(select Id_Course from [Course] where Course_Level=@level and Course_Language=@country)";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Language;
        DoesLessonExist = command.ExecuteScalar() == null ? false : true;
        if(DoesLessonExist)
        {
          decimal levl=0;
          command.CommandText = "select MAX(Lesson_Level) from [Lesson]";
          using (var reader = command.ExecuteReader())
            if (reader.Read())
              levl = (decimal)reader["Lesson_Level"];
          command.CommandText = "select Id_Course from [Course] where Course_Level=@level and Course_Language=@country";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl+1;
          command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Language;
          if (command.ExecuteScalar() == null)
          {
            command.CommandText = "insert into [Course] values(@country, @level)";
            command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
            command.ExecuteNonQuery();
          }
          while (levl!=Level)
          {
            command.CommandText = "update [Lesson] set Lesson_Level=@levelup, Lesson_Parameter=@param, Id_Course = (select Id_Course from [Course] where Course_Level=@levelup and Course_Language=@country) where Lesson_Language=@level and Id_Course=(select Id_Course from [Course] where Course_Level=@level and Course_Language=@country)";
            command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl + 1;
            command.Parameters.Add("@param",SqlDbType.NVarChar).Value = Country.ToLower() + (levl+1).ToString();
            command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl;
            command.ExecuteNonQuery();
            levl--;
          }
          command.CommandText = "insert into [Lesson] values()";
        }
        if (!DoesLessonExist)
        {

        }*/

        /*while (true)
        {
          int id = 0;
          command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Language=@level";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl;
          using(var reader = command.ExecuteReader())
          {
            while(reader.Read())
              id = (int)reader["Id_Lesson"];
          }
          if (id!=0)
          {
            command.CommandText = "";
            levl++;
          }
          if (id==0)
          {
            command.CommandText = "";
            break;
          }
        }*/
      }
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "";
      }
    }
    public void UpdateLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string Title, decimal Level)
    {

    }
  }
}
