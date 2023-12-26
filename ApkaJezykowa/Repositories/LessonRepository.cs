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
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using static ApkaJezykowa.MVVM.ViewModel.LessonImagesData;

namespace ApkaJezykowa.Repositories
{
  internal class LessonRepository : BaseRepository, ILessonRepository
  {
    public LessonModel Display(int Level, string Language, string Lesson_Language)
    {
      LessonModel lesson = null;
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select L.Id_Lesson, L.Lesson_Level, LT.Lesson_Title, L.Id_Course from [Lesson_Title] LT join [Lesson] L on LT.Id_Lesson = L.Id_Lesson where Lesson_Level=@level and Lesson_Language=@lessonlang and Id_Course =(Select Id_Course from [Course] where [Course_Name] = @language)";
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
      using (var connection = GetCourseConnection())
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
      using (var connection = GetCourseConnection())
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
      using(var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection= connection;
        command.CommandText = "SELECT Id_Lesson_Content, Lesson_Text FROM [Lesson_Content] WHERE Id_Lesson_Title = (SELECT Id_Lesson_Title FROM [Lesson_Title] WHERE Lesson_Title = @title and Lesson_Language = @lessonlang)";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value=Title;
        command.Parameters.Add("@lessonlang", SqlDbType.NVarChar).Value = Lesson_Language;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            LessonContentModel model = new LessonContentModel();
            int Id = (int)reader["Id_Lesson_Content"];
            model.LessonText = reader["Lesson_Text"].ToString();
            List<LessonImageModel> Images = new List<LessonImageModel>();
            using (var command2 = new SqlCommand())
            {
              command2.Connection = connection;
              command2.CommandText = "SELECT Lesson_Image, Image_Desc from Lesson_Images where Id_Lesson_Content = @id";
              command2.Parameters.Add("@id", SqlDbType.Int).Value = Id;
              using(var reader2 = command2.ExecuteReader())
              {
                while (reader2.Read())
                {   
                  LessonImageModel image = new LessonImageModel();
                  if (reader2["Lesson_Image"] != System.DBNull.Value)
                    image.Image = (byte[])reader2["Lesson_Image"];
                  else
                    image.Image = null;
                  image.Description = reader2["Image_Desc"].ToString();
                  Images.Add(image);
                }
              }
            }
            model.LessonImages = Images;
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
      using(var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Title from [Lesson_Title] where Id_Lesson in (select Id_Lesson from Lesson where Lesson_Level = Coalesce(@level,Lesson_Level) and Id_Course in (select Id_Course from [Course] where [Course_Name] = Coalesce(@country,[Course_Name]))) and Lesson_Language = Coalesce(@language,Lesson_Language) order by Id_Lesson";
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
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select C.Id_Course, C.[Course_Name], L.Lesson_Level, LT.Lesson_Language, L.Id_Lesson, LT.Id_Lesson_Title, LT.Lesson_Title From[Course] C Join[Lesson] L on C.Id_Course = L.Id_Course Join[Lesson_Title] LT on L.Id_Lesson = LT.Id_Lesson Where LT.Lesson_Title=@title";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = LName;
        using(var reader = command.ExecuteReader())
        {
          if(reader.Read())
          {
            result = new ParamModel()
            {
              CourseID = (int)reader["Id_Course"],
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
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "SELECT Id_Lesson_Content, Lesson_Text FROM [Lesson_Content] WHERE Id_Lesson_Title = (SELECT Id_Lesson_Title FROM [Lesson_Title] WHERE Lesson_Title = @title)";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Lesson;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonData model = new LessonData();
            model.LessonID = (int)reader["Id_Lesson_Content"];
            model.LessonText = reader["Lesson_Text"].ToString();
            ObservableCollection<LessonImagesData> Images = new ObservableCollection<LessonImagesData>();
            using (var command2 = new SqlCommand())
            {
              command2.Connection = connection;
              command2.CommandText = "SELECT Id_Lesson_Images, Lesson_Image, Image_Desc from Lesson_Images where Id_Lesson_Content = @id";
              command2.Parameters.Add("@id", SqlDbType.Int).Value = model.LessonID;
              
              using (var reader2 = command2.ExecuteReader())
              {
                while (reader2.Read())
                {
                  LessonImagesData image = new LessonImagesData();
                  image.ImageID = (int)reader2["Id_Lesson_Images"];
                  if (reader2["Lesson_Image"] != System.DBNull.Value)
                    image.Image = (byte[])reader2["Lesson_Image"];
                  else
                    image.Image = null;
                  image.Description = reader2["Image_Desc"].ToString();
                  Images.Add(image);
                }
              }
            }
            model.LessonImage = Images;
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
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select [Course_Name], [Image] from [Course]";
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Clicker button = new Clicker();
            button.Language = reader["Course_Name"].ToString();
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
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select [Image] from [Course] where [Course_Name] = @lang";
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
    public void AddLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string Title, decimal Level)
    {
      bool MaxLevel;
      int? CourseID, LessonID, Lesson_TitleID;
      decimal MaxLevelInt = 0;
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        //znajdowanie ID kursu
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Course from [Course] where [Course_Name]=@country";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
        CourseID = System.Convert.ToInt32(command.ExecuteScalar());
        //szukanie lekcji w danym języku z największym możliwym przypisanym poziomem dla danego kursu
        command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = (select max(Lesson_Level) from [Lesson] where Id_Course = @id) and Id_Course=@id)";
        command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
        MaxLevel = command.ExecuteScalar() == null ? false : true;
      }
      if (MaxLevel)
      {

        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          //znajdź maksymalny poziom
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select max(Lesson_Level) from [Lesson] where Id_Course = @id";
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          MaxLevelInt = System.Convert.ToInt32(command.ExecuteScalar());
          //stwórz instancję lekcji o poziomie wyższym o jeden
          command.CommandText = "insert into [Lesson] values(@level, @parameter, @id_course)";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = MaxLevelInt + 1;
          command.Parameters.Add("@parameter", SqlDbType.NVarChar).Value = Country.ToLower() + (MaxLevelInt + 1).ToString();
          command.Parameters.Add("@id_course", SqlDbType.Int).Value = CourseID;
          command.ExecuteScalar();
        }
      }
      //sprawdzanie, czy istnieje już lekcja przypisana do danego poziomu
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
        LessonID = System.Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language = @language and Id_Lesson = @lesson_id";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@lesson_id", SqlDbType.Int).Value = LessonID;
        Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
      }
      if (Lesson_TitleID != 0)
      {
        //przeniesienie lekcji o poziom wyżej, jeśli wybrany poziom jest zajęty
        while (Level <= MaxLevelInt)
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "update [Lesson_Title] set Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @levelup and Id_Course = @id) where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id)";
            command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = MaxLevelInt + 1;
            command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
            command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = MaxLevelInt;
            command.ExecuteScalar();
            MaxLevelInt--;
          }
        }
      }
      //dodaj nową lekcję w podanym języku
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "insert into [Lesson_Title] values (@language, @title, (select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id))";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
        command.ExecuteScalar();
        command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language = @language2 and Id_Lesson =(select Id_Lesson from [Lesson] where Lesson_Level=@level2 and Id_Course=@id2)";
        command.Parameters.Add("@language2", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@level2", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id2", SqlDbType.NVarChar).Value = CourseID;
        Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
      }
      //dodanie kontentu do lekcji
      foreach (var x in EditedLessons)
      {
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Lesson_Content] values (@text, @id)";
          command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
          command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
          command.ExecuteNonQuery();
        }
        int LessonContentId = 0;
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select Id_Lesson_Content from [Lesson_Content] where Lesson_Text = @text and Id_Lesson_Title = @id";
          command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
          command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
          LessonContentId = System.Convert.ToInt32(command.ExecuteScalar());
        }
        //dodanie obrazów do danej instancji kontentu
        foreach (var y in x.LessonImage)
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Images] values (@id, @desc, @image)";
            command.Parameters.Add("@id", SqlDbType.Int).Value = LessonContentId;
            command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = y.Description;
            command.Parameters.Add("@image", SqlDbType.VarBinary).Value = y.Image;
            command.ExecuteNonQuery();
          }
        }
      }
    }
      
    public void UpdateLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string OldTitle, string Title, decimal Level, int CourseID, int Lesson_Id, int Lesson_Title_Id)
    {
      decimal levl = 0;
      string currenttitle = null;
      using (var connection = GetCourseConnection())
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
      decimal supportlevl = levl;
      if (levl != Level)
      {

        if (Level > levl)
        {
          while (levl < Level)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update [Lesson_Title] set Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @leveldown and Id_Course = @id) where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id)";
              command.Parameters.Add("@leveldown", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
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
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update [Lesson_Title] set Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @levelup and Id_Course = @id) where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id)";
              command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl - 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
            }
            levl--;
          }
        }

      }
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "update [Lesson_Title] set Lesson_Title = @title, Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id) where Lesson_Title=@oldtitle and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @oldlevel and Id_Course = @id)";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
        command.Parameters.Add("@oldtitle", SqlDbType.NVarChar).Value = OldTitle;
        command.Parameters.Add("@oldlevel", SqlDbType.Decimal).Value = supportlevl;
        command.ExecuteNonQuery();
      }
      ObservableCollection<LessonData> data = new ObservableCollection<LessonData>();
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Lesson_Content, Lesson_Text from Lesson_Content where Id_Lesson_Title = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Title_Id;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonData content = new LessonData();
            content.LessonID = (int)reader["Id_Lesson_Content"];
            content.LessonText = reader["Lesson_Text"].ToString();
            ObservableCollection<LessonImagesData> Images = new ObservableCollection<LessonImagesData>();
            using (var command2 = new SqlCommand())
            {
              command2.Connection = connection;
              command2.CommandText = "SELECT Id_Lesson_Images, Lesson_Image, Image_Desc from Lesson_Images where Id_Lesson_Content = @id";
              command2.Parameters.Add("@id", SqlDbType.Int).Value = content.LessonID;

              using (var reader2 = command2.ExecuteReader())
              {
                while (reader2.Read())
                {
                  LessonImagesData image = new LessonImagesData();
                  image.ImageID = (int)reader2["Id_Lesson_Images"];
                  if (reader2["Lesson_Image"] != System.DBNull.Value)
                    image.Image = (byte[])reader2["Lesson_Image"];
                  else
                    image.Image = null;
                  image.Description = reader2["Image_Desc"].ToString();
                  Images.Add(image);
                }
                reader2.NextResult();
              }
            }
            if(Images!=null)
              content.LessonImage = Images;
            else 
              content.LessonImage = null;

            data.Add(content);
          }
          reader.NextResult();
        }
      }
      int counter;
      if (EditedLessons.Count() > data.Count())
        counter = data.Count();
      else
        counter = EditedLessons.Count();
      for (int i = 0; i < counter; i++)
      {
        if (EditedLessons[i].LessonText != data[i].LessonText)
        {
          using (var connection = GetCourseConnection())
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
        int imageCounter;
        if (EditedLessons[i].LessonImage.Count() < data[i].LessonImage.Count())
          imageCounter = EditedLessons[i].LessonImage.Count();
        else
          imageCounter = data[i].LessonImage.Count();

        for (int j = 0; j < imageCounter; j++)
        {
          if (EditedLessons[i].LessonImage[j].Description != data[i].LessonImage[j].Description && EditedLessons[i].LessonImage[j].Image.SequenceEqual(data[i].LessonImage[j].Image) == false)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update Lesson_Images set Image_Desc = @desc, Lesson_Image = @image where Image_Desc = @olddesc and Lesson_Image=@oldimage and Id_Lesson_Images=@id";

              command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = EditedLessons[i].LessonImage[j].Description;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage[j].Image;
              command.Parameters.Add("@olddesc", SqlDbType.NVarChar).Value = data[i].LessonImage[j].Description;
              command.Parameters.Add("@oldimage", SqlDbType.VarBinary).Value = data[i].LessonImage[j].Image;
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
          else if (EditedLessons[i].LessonImage[j].Description != data[i].LessonImage[j].Description)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update Lesson_Images set Image_Desc = @desc where Image_Desc = @olddesc and Id_Lesson_Images=@id";

              command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = EditedLessons[i].LessonImage[j].Description;
              command.Parameters.Add("@olddesc", SqlDbType.NVarChar).Value = data[i].LessonImage[j].Description;
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
          else if (!EditedLessons[i].LessonImage[j].Image.SequenceEqual(data[i].LessonImage[j].Image))
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update Lesson_Images set Lesson_Image=@image where Lesson_Image=@oldimage and Id_Lesson_Images=@id";

              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage[j].Image;
              command.Parameters.Add("@oldimage", SqlDbType.VarBinary).Value = data[i].LessonImage[j].Image;
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
        }
        if (EditedLessons[i].LessonImage.Count() > data[i].LessonImage.Count())
        {
          for (int j = imageCounter; j < EditedLessons[i].LessonImage.Count(); j++)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into Lesson_Images values (@id, @desc, @image)";
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
              command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = EditedLessons[i].LessonImage[j].Description;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage[j].Image;
              command.ExecuteNonQuery();
            }
          }
        }
        else if (EditedLessons[i].LessonImage.Count() < data[i].LessonImage.Count())
        {
          for (int j = imageCounter; j < data[i].LessonImage.Count(); j++)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "delete from Lesson_Images where Id_Lesson_Images=@id";
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
        }
      }
      if (EditedLessons.Count() > data.Count())
      {
        for (int i = counter; i < EditedLessons.Count(); i++)
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Content] (Lesson_Text, Id_Lesson_Title) select @text, @titleid";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@titleid", SqlDbType.Int).Value = Lesson_Title_Id;
            command.ExecuteNonQuery();
          }
          int LessonContentId = 0;
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select Id_Lesson_Content from [Lesson_Content] where Lesson_Text = @text and Id_Lesson_Title = @id";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Title_Id;
            LessonContentId = System.Convert.ToInt32(command.ExecuteScalar());
          }
          foreach (var y in EditedLessons[i].LessonImage)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into [Lesson_Images] values (@id, @desc, @image)";
              command.Parameters.Add("@id", SqlDbType.Int).Value = LessonContentId;
              command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = y.Description;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = y.Image;
              command.ExecuteNonQuery();
            }
          }
        }
      }
      else if (EditedLessons.Count() < data.Count())
      {
        for (int i = counter; i < data.Count(); i++)
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "delete from Lesson_Images where Id_Lesson_Content = @id";
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
          using (var connection = GetCourseConnection())
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
      /*decimal levl = 0;
      string currenttitle = null;
      using (var connection = GetCourseConnection())
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
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update [Lesson] set Lesson_Level = @leveldown, Lesson_Parameter = @param where Lesson_Level = @level and Id_Course = (select Id_Course from[Course] where [Course_Name] = @country)";
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
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update [Lesson] set Lesson_Level = @levelup, Lesson_Parameter = @param where Lesson_Level = @level and Id_Course = (select Id_Course from[Course] where [Course_Name] = @country)";
              command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (levl).ToString();
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl - 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
            }
            levl--;
          }
        }
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;

          command.CommandText = "update [Lesson] set Lesson_Level = @level, Lesson_Parameter = @param where Id_Lesson = @Id";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country.ToLower() + (Level).ToString();
          command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Id;
          command.ExecuteNonQuery();
        }
      }
      using (var connection = GetCourseConnection())
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
      if (currenttitle != Title)
      {
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "update [Lesson_Title] set Lesson_Title = @title where Lesson_Title=@oldtitle";
          command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
          command.Parameters.Add("@oldtitle", SqlDbType.NVarChar).Value = currenttitle;
          command.ExecuteNonQuery();
        }
;
      }
      ObservableCollection<LessonData> data = new ObservableCollection<LessonData>();
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Lesson_Content, Lesson_Text from Lesson_Content where Id_Lesson_Title = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Title_Id;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonData content = new LessonData();
            content.LessonID = (int)reader["Id_Lesson_Content"];
            content.LessonText = reader["Lesson_Text"].ToString();
            ObservableCollection<LessonImagesData> Images = new ObservableCollection<LessonImagesData>();
            using (var command2 = new SqlCommand())
            {
              command2.Connection = connection;
              command2.CommandText = "SELECT Id_Lesson_Images, Lesson_Image, Image_Desc from Lesson_Images where Id_Lesson_Images = @id";
              command2.Parameters.Add("@id", SqlDbType.Int).Value = content.LessonID;

              using (var reader2 = command2.ExecuteReader())
              {
                while (reader2.Read())
                {
                  LessonImagesData image = new LessonImagesData();
                  image.ImageID = (int)reader2["Id_Lesson_Images"];
                  if (reader2["Lesson_Image"] != System.DBNull.Value)
                    image.Image = (byte[])reader2["Lesson_Image"];
                  else
                    image.Image = null;
                  image.Description = reader2["Image_Desc"].ToString();
                  Images.Add(image);
                }
              }
            }
            content.LessonImage = Images;

            data.Add(content);
          }
          reader.NextResult();
        }
      }
      int counter;
      if (EditedLessons.Count() > data.Count())
        counter = data.Count();
      else
        counter = EditedLessons.Count();
      for (int i = 0; i < counter; i++)
      {
        if (EditedLessons[i].LessonText != data[i].LessonText)
        {
          using (var connection = GetCourseConnection())
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
        int imageCounter;
        if (EditedLessons[i].LessonImage.Count() > data[i].LessonImage.Count())
          imageCounter = EditedLessons[i].LessonImage.Count();
        else
          imageCounter = data[i].LessonImage.Count();

        for (int j = 0; j < counter; j++)
        {
          if (EditedLessons[i].LessonImage[j].Description != data[i].LessonImage[j].Description && EditedLessons[i].LessonImage[j].Image != data[i].LessonImage[j].Image)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update Lesson_Images set Image_Desc = @desc, Lesson_Image = @image where Image_Desc = @olddesc, Lesson_Image=@oldimage and Id_Lesson_Images=@id";

        command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = EditedLessons[i].LessonImage[j].Description;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage[j].Image;
              command.Parameters.Add("@olddesc", SqlDbType.NVarChar).Value = data[i].LessonImage[j].Description;
              command.Parameters.Add("@oldimage", SqlDbType.VarBinary).Value = data[i].LessonImage[j].Image;
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
          else if (EditedLessons[i].LessonImage[j].Description != data[i].LessonImage[j].Description)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update Lesson_Images set Image_Desc = @desc where Image_Desc = @olddesc and Id_Lesson_Images=@id";

        command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = EditedLessons[i].LessonImage[j].Description;
              command.Parameters.Add("@olddesc", SqlDbType.NVarChar).Value = data[i].LessonImage[j].Description;
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
          else if (EditedLessons[i].LessonImage[j].Image != data[i].LessonImage[j].Image)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "update Lesson_Images set Lesson_Image=@image where Lesson_Image=@oldimage and Id_Lesson_Images=@id";

        command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage[j].Image;
              command.Parameters.Add("@oldimage", SqlDbType.VarBinary).Value = data[i].LessonImage[j].Image;
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
        }
        if (EditedLessons[i].LessonImage.Count() > data[i].LessonImage.Count())
        {
          for (int j = imageCounter; j < EditedLessons[i].LessonImage.Count(); j++)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into Lesson_Images values (@id, @desc, @image)";
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
              command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = EditedLessons[i].LessonImage[j].Description;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = EditedLessons[i].LessonImage[j].Image;
              command.ExecuteNonQuery();
            }
          }
        }
        else if (EditedLessons[i].LessonImage.Count() < data[i].LessonImage.Count())
        {
          for (int j = imageCounter; j < data[i].LessonImage.Count(); j++)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "delete from Lesson_Images where Id_Lesson_Images=@id";
              command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonImage[j].ImageID;
              command.ExecuteNonQuery();
            }
          }
        }
      }
      if (EditedLessons.Count() > data.Count())
      {
        for (int i = counter; i < EditedLessons.Count(); i++)
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Content] (Lesson_Text, Id_Lesson_Title) select @text, @titleid";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@titleid", SqlDbType.Int).Value = Lesson_Title_Id;
            command.ExecuteNonQuery();
          }
          int LessonContentId = 0;
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select Id_Lesson_Content from [Lesson_Content] where Lesson_Text = @text and Id_Lesson_Title = @id";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Title_Id;
            LessonContentId = System.Convert.ToInt32(command.ExecuteScalar());
          }
          foreach (var y in EditedLessons[i].LessonImage)
          {
            using (var connection = GetCourseConnection())
            using (var command = new SqlCommand())
            {
              connection.Open();
              command.Connection = connection;
              command.CommandText = "insert into [Lesson_Images] values (@id, @desc, @image)";
              command.Parameters.Add("@id", SqlDbType.Int).Value = LessonContentId;
              command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = y.Description;
              command.Parameters.Add("@image", SqlDbType.VarBinary).Value = y.Image;
              command.ExecuteNonQuery();
            }
          }
        }
      }
      else if (EditedLessons.Count() < data.Count())
      {
        for (int i = counter; i < data.Count(); i++)
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "delete from Lesson_Content where Id_Lesson_Content = @id";
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "delete from Lesson_Images where Id_Lesson_Content = @id";
            command.Parameters.Add("@id", SqlDbType.Int).Value = data[i].LessonID;
            command.ExecuteNonQuery();
          }
        }
      }*/
    }
  }
}
