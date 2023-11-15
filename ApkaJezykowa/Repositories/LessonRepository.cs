using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Repositories
{
  internal class LessonRepository : BaseRepository, ILessonRepository
  {
    public LessonModel Display(int Level, string Language, string Lesson_Language)
    {
      LessonModel lesson = null;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select L.Id_Lesson, L.Lesson_Level, LT.Lesson_Title, L.Id_Course from [Lesson_Title] LT join [Lesson] L on LT.Id_Lesson = L.Id_Lesson where Lesson_Level=@level and Lesson_Language=@lessonlang and Id_Course =(Select Id_Course from [Course] where [Course_Name] = @language and Course_Level = @level)";
        command.Parameters.Add("@level", SqlDbType.Int).Value = Level;
        command.Parameters.Add("@lessonlang", SqlDbType.NVarChar).Value = Lesson_Language;
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
    public void Obtain_Lesson_List(List<LessonListModel> LessonsList, string Language, string Lesson_Language)
    {
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select LT.Lesson_Title, L.Lesson_Parameter from [Lesson_Title] LT join [Lesson] L on LT.Id_Lesson=L.Id_Lesson where LT.Lesson_Language = @lessonlang and L.Id_Course in (Select Id_Course from [Course] where [Course_Name] = @language) order by L.Lesson_Parameter";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@lessonlang", SqlDbType.NVarChar).Value = Lesson_Language;
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
        command.CommandText = "select Lesson_Parameter from [Lesson] where Id_Course in (Select Id_Course from [Course] where [Course_Name] = @language) order by Lesson_Parameter";
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
    public void Obtain_Lessons(List<LessonContentModel> Lessons, string Title, string Lesson_Language)
    {
      using(var connection = GetConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection= connection;
        command.CommandText = "SELECT Lesson_Text, Lesson_Image FROM [Lesson_Content] WHERE Id_Lesson_Title = (SELECT Id_Lesson_Title FROM [Lesson_Title] WHERE Lesson_Title = @title and Lesson_Language = @lessonlang)";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value=Title;
        command.Parameters.Add("@lessonlang", SqlDbType.NVarChar).Value = Lesson_Language;
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
        command.CommandText = "Select C.[Course_Name], L.Lesson_Level, LT.Lesson_Language, L.Id_Lesson, LT.Id_Lesson_Title, LT.Lesson_Title From[Course] C Join[Lesson] L on C.Id_Course = L.Id_Course Join[Lesson_Title] LT on L.Id_Lesson = LT.Id_Lesson Where LT.Lesson_Title=@title";
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
              level = (decimal)reader["Lesson_Level"],
              Id = (int)reader["Id_Lesson"],
              TitleId = (int)reader["Id_Lesson_Title"]
            };
          }
        }
      }
      return result;
    }
    public ObservableCollection<LessonData> Obtain_Lesson_Content(string Lesson)
    {
      ObservableCollection<LessonData> lc = new ObservableCollection<LessonData>();
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "SELECT Id_Lesson_Content, Lesson_Text, Lesson_Image FROM [Lesson_Content] WHERE Id_Lesson_Title = (SELECT Id_Lesson_Title FROM [Lesson_Title] WHERE Lesson_Title = @title)";
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
    public void GetButtons(ObservableCollection<Clicker> Buttons)
    {
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Language, Image from Icon";
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Clicker button = new Clicker();
            button.Language = reader["Language"].ToString();
            button.Icon = (byte[])reader["Image"];
            Buttons.Add(button);
          }
          reader.NextResult();
        }
      }
    }
    public byte[] GetIcon(string Lang)
    {
      byte[] Icon=null;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Image from Icon where Language = @lang";
        command.Parameters.Add("@lang",SqlDbType.NVarChar).Value = Lang;
        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            Icon = (byte[])reader["Image"];
          }
        }
        return Icon;
      }
    }
    public void AddLesson(string Country, string Language, ObservableCollection<LessonData>EditedLessons, string Title, decimal Level)
    {
      int? CourseID, LessonID, Lesson_TitleID;
      bool DoesLessonExist, DoesCourseExist;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
        CourseID = System.Convert.ToInt32(command.ExecuteScalar());
      }
        if(CourseID != 0)
        {
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
          LessonID = System.Convert.ToInt32(command.ExecuteScalar());
        }
          if (LessonID !=0)
          {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@Language and Id_Lesson=@id";
            command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
            command.Parameters.Add("@id", SqlDbType.Int).Value = LessonID;
            DoesLessonExist = command.ExecuteScalar() == null ? false : true;
          }
            if (DoesLessonExist)
            {
              decimal levl = 0;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "select MAX(Lesson_Level) from [Lesson]";
              using (var reader = command.ExecuteReader())
                if (reader.Read())
                  levl = (decimal)reader[0];
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "select Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country";
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              DoesCourseExist = command.ExecuteScalar() == null ? false : true;
            }
              if (!DoesCourseExist)
              {
              using (var connection = GetConnection())
              using (var command = new SqlCommand())
              {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into [Course] values(@country, @level, (select Id_Icon from Icon where [Language]=@country))";
                command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
                command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
                command.ExecuteNonQuery();
              }
              }
              while (levl >= Level)
              {
              using (var connection = GetConnection())
              using (var command = new SqlCommand())
              {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "update [Lesson] set Lesson_Level=@levelup, Lesson_Parameter=@param, Id_Course = (select Id_Course from [Course] where Course_Level=@levelup and [Course_Name]=@country) where Lesson_Level=@level and Id_Course=(select Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country)";
                command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl + 1;
                command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (levl + 1).ToString();
                command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl;
                command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
                command.ExecuteNonQuery();
              }
                levl--;
              }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into [Lesson] (Lesson_Level, Lesson_Parameter, Id_Course) select @level, @param, Course.Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country";
              command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + Level.ToString();
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, Lesson.Id_Lesson from [Lesson] where Id_Course=(select Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country)";
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@language and Id_Lesson = (select Id_Lesson from [Lesson] where Id_Course=(select Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country))";
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
            }
              foreach (var x in EditedLessons)
              {
              using (var connection = GetConnection())
              using (var command = new SqlCommand())
              {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
                command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
                command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
                command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
                command.ExecuteNonQuery();
              }
              }
            }
            if (!DoesLessonExist)
            {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, @id";
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
              command.Parameters.Add("@id", SqlDbType.Int).Value = LessonID;
              command.ExecuteNonQuery();
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@language and Id_Lesson = @id";
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@id", SqlDbType.Int).Value = LessonID;
              Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
            }
              foreach (var x in EditedLessons)
              {
              using (var connection = GetConnection())
              using (var command = new SqlCommand())
              {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
                command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
                command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
                command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
                command.ExecuteNonQuery();
              }
              }
            }
          }
          if (LessonID==0)
          {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson] (Lesson_Level, Lesson_Parameter, Id_Course) select @level, @param, @id";
            command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + Level.ToString();
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            command.ExecuteNonQuery();
          }
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, Lesson.Id_Lesson from [Lesson] where Id_Course=@id";
            command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
            command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            command.ExecuteNonQuery();
          }
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@language and Id_Lesson = (select Id_Lesson from [Lesson] where Id_Course=@id)";
            command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
          }
            foreach (var x in EditedLessons)
            {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
              command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
              command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
              command.ExecuteNonQuery();
            }
            }
          }
        }
        if(CourseID==0)
        {
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Course] values(@country, @level, (select Id_Icon from Icon where [Language]=@country))";
          command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.ExecuteNonQuery();
        }
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select Id_Course from [Course] where Course_Level=@level and [Course_Name]=@country";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
          CourseID = System.Convert.ToInt32(command.ExecuteScalar());
        }
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Lesson] (Lesson_Level, Lesson_Parameter, Id_Course) select @level, @param, @id";
          command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + Level.ToString();
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          command.ExecuteNonQuery();
        }
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) select @language, @title, Lesson.Id_Lesson from [Lesson] where Id_Course=@id";
          command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
          command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          command.ExecuteNonQuery();
        }
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@language and Id_Lesson = (select Id_Lesson from [Lesson] where Id_Course=@id)";
          command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
        }
          foreach (var x in EditedLessons)
          {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Content] values (@text, @id, @image)";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
            command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
            command.Parameters.Add("@image", SqlDbType.VarBinary).Value = x.LessonImage;
            command.ExecuteNonQuery();
          }
          }
        }
    }
    public void UpdateLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string Title, decimal Level, int Lesson_Id, int Lesson_Title_Id)
    {
      decimal levl = 0;
      string currenttitle = null;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Level from [Lesson] where Id_Lesson = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Id;
        using (var reader = command.ExecuteReader())
          if (reader.Read())
            levl = (decimal)reader[0];
      }
      if (levl != Level)
      {

        if (Level > levl)
        {
          while (levl < Level)
          {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update [Lesson] set Lesson_Level = @leveldown, Lesson_Parameter = @param, Id_Course = (select Id_Course from[Course] where Course_Level = @leveldown and[Course_Name] = @country) where Lesson_Level = @level and Id_Course = (select Id_Course from[Course] where Course_Level = @level and[Course_Name] = @country)";
              command.Parameters.Add("@leveldown", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (levl).ToString();
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
            }
            levl++;
          }
        }
        else if (Level < levl)
        {
          while (levl > Level)
          {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update [Lesson] set Lesson_Level = @levelup, Lesson_Parameter = @param, Id_Course = (select Id_Course from[Course] where Course_Level = @levelup and[Course_Name] = @country) where Lesson_Level = @level and Id_Course = (select Id_Course from[Course] where Course_Level = @level and[Course_Name] = @country)";
              command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (levl).ToString();
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl-1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
            }
            levl--;
          }
        }
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;

          command.CommandText = "update [Lesson] set Lesson_Level = @level, Lesson_Parameter = @param, Id_Course = (select Id_Course from[Course] where Course_Level = @level and[Course_Name] = @country) where Id_Lesson = @Id";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (Level).ToString();
          command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
          command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Id;
          command.ExecuteNonQuery();
        }
      }
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Title from [Lesson_Title] where Id_Lesson_Title=@IdTitle";
        command.Parameters.Add("@IdTitle", SqlDbType.Int).Value = Lesson_Title_Id;
        var reader = command.ExecuteReader();
        if (reader.Read())
          currenttitle = reader[0].ToString();
      }
        if(currenttitle != Title)
        {
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "update [Lesson_Title] set Lesson_Title = @title where Lesson_Title=@oldtitle";
          command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
          command.Parameters.Add("@oldtitle", SqlDbType.NVarChar).Value = currenttitle;
          command.ExecuteNonQuery();
        }
;       }
        ObservableCollection<LessonData> data = new ObservableCollection<LessonData>();
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Lesson_Content, Lesson_Text, Lesson_Image from Lesson_Content where Id_Lesson_Title = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Title_Id;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonData content = new LessonData();
            content.LessonID = (int)reader["Id_Lesson_Content"];
            content.LessonText = reader["Lesson_Text"].ToString();
            if (reader["Lesson_Image"] != System.DBNull.Value)
              content.LessonImage = (byte[])reader["Lesson_Image"];
            else
              content.LessonImage = null;
            data.Add(content);  
          }
          reader.NextResult();
        }
      }
      int counter;
      if(EditedLessons.Count()>data.Count())
        counter = data.Count();
      else
        counter = EditedLessons.Count();
      for (int i=0; i<counter; i++)
      {
        if(EditedLessons[i].LessonText != data[i].LessonText && EditedLessons[i].LessonImage != data[i].LessonImage)
        {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "update [Lesson_Content] set Lesson_Text = @text, Lesson_Image=@image where Lesson_Text=@oldtext and Lesson_Image=@oldimage and Id_Lesson_Content=@id";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@image",SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage;
            command.Parameters.Add("@oldtext", SqlDbType.NVarChar).Value = data[i].LessonText;
            command.Parameters.Add("@oldimage", SqlDbType.VarBinary).Value = data[i].LessonImage;
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
        }
        else if (EditedLessons[i].LessonText != data[i].LessonText)
        {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "update Lesson_Content set Lesson_Text = @text where Lesson_Text=@oldtext and Id_Lesson_Content=@id";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@oldtext", SqlDbType.NVarChar).Value = data[i].LessonText;
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
        }
        else if(EditedLessons[i].LessonImage != data[i].LessonImage)
        {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "update Lesson_Content set Lesson_Image=@image where Lesson_Image=@oldimage and Id_Lesson_Content=@id";
            command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage;
            command.Parameters.Add("@oldimage", SqlDbType.VarBinary).Value = data[i].LessonImage;
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
        }
      }
      if(EditedLessons.Count() > data.Count())
      {
        for (int i = counter; i<EditedLessons.Count(); i++)
        {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Content] (Lesson_Text, Id_Lesson_Title, Lesson_Image) select @text, @titleid, @image";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@titleid", SqlDbType.Int).Value = Lesson_Title_Id;
            command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage;
            command.ExecuteNonQuery();
          }
        }
      }
      else if (EditedLessons.Count() < data.Count())
      {
        for (int i = counter; i < data.Count(); i++)
        {
          using (var connection = GetConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "delete from Lesson_Content where Id_Lesson_Content = @id";
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
        }
      }
    }
  }
}
