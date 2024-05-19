using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using Microsoft.CognitiveServices.Speech.Diagnostics.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using static ApkaJezykowa.MVVM.ViewModel.LessonImagesData;

namespace ApkaJezykowa.Repositories
{
  internal class LessonRepository : BaseRepository, ILessonRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    public LessonRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
    }
    public LessonModel Display(int Level, string Language, string Lesson_Language)
    {
      LessonModel lesson = null;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Display Data. Start!");
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Display Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return lesson;
    }
    /*public string GetTitle(int Id, string Language)
    {
      string read = null;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Title. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Title from [Lesson_Title] where Id_Lesson_Title = @id and Lesson_Language = @lang";
        command.Parameters.Add("@lang", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Id;
        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            read = reader["Lesson_Title"].ToString();
          }
        }
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Title", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return read;
    }*/
    public void Obtain_Lesson_List(List<LessonListModel> LessonsList, string Language, string Lesson_Language)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Lessons List. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select LT.Id_Lesson_Title, LT.Lesson_Title, L.Lesson_Level from [Lesson_Title] LT join [Lesson] L on LT.Id_Lesson=L.Id_Lesson where LT.Lesson_Language = @lessonlang and L.Id_Course in (Select Id_Course from [Course] where [Course_Name] = @language) order by L.Lesson_Level";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@lessonlang", SqlDbType.NVarChar).Value = Lesson_Language;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            LessonsList.Add(new LessonListModel((int)reader["Id_Lesson_Title"], reader["Lesson_Title"].ToString(), (decimal)reader["Lesson_Level"]));
          }
          reader.NextResult();
        }
        //zastanawia mnie czy nie lepiej byłoby przerobić tą funkcję na List<LessonListmModel> Obtain_Lesson_List
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Lessons List", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
    /*public decimal Obtain_Level(int Id, string Language)
    {
      decimal dec = 0; ;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Level. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Lesson_Level from [Lesson] where Id_Lesson = @id";// in (Select Id_Course from [Course] where [Course_Name] = @language) order by Lesson_Parameter"; tu skończyłeś btw, rozważałeś sens tj, jak i następnych funkcji
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id;
        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            dec = (decimal)reader["Lesson_Level"];
          }
          //reader.NextResult();
        }
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Display Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return dec;
    }*/
    public void Obtain_Lessons(List<LessonContentModel> Lessons, int TitleId)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Lesson Content. Start!");
      using (var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection= connection;
        command.CommandText = "SELECT Id_Lesson_Content, Lesson_Text FROM [Lesson_Content] WHERE Id_Lesson_Title =@titleid"; //(SELECT Id_Lesson_Title FROM [Lesson_Title] WHERE Lesson_Title = @title and Lesson_Language = @lessonlang)";
        command.Parameters.Add("@titleid", SqlDbType.Int).Value=TitleId;
        //command.Parameters.Add("@lessonlang", SqlDbType.NVarChar).Value = Lesson_Language;
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Lesson Content", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
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
      List<string> ts = new List<string>
      {
        "None"
      };
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Lesson Names. Start!");
      using (var connection = GetCourseConnection())
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Lesson Names", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return ts;
    }
    public LessonParamModel Obtain_Lesson_Parameters(string LName)
    {
      LessonParamModel result = null;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Lesson Parameters. Start!");
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
            result = new LessonParamModel()
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Lesson Parameters", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return result;
    }
    public ObservableCollection<LessonData> Obtain_Lesson_Content(string Lesson)
    {
      ObservableCollection<LessonData> lc = new ObservableCollection<LessonData>();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Lesson Content. Start!");
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
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Display Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return lc;
      }
    }
    public void GetButtons(ObservableCollection<Clicker> Buttons)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Languages. Start!");
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Display Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
    public byte[] GetIcon(string Lang)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Icons. Start!");
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
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Display Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return Icon;
      }
    }
    public void AddLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string Title, decimal Level)
    {
      bool MaxLevel;
      int? CourseID, LessonID = 0, Lesson_TitleID;
      decimal MaxLevelInt = 0, MaxLevelIntLang=0;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Adding Lesson. Start!");
      
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
        //zmniejszenie poziomu lekcji do najwyższego możliwego +1 w celu uniknięcia dezorganizacji lekcji
        command.CommandText = "select max(L.Lesson_Level) from [Lesson] L join [Lesson_Title] LT on L.Id_Lesson = LT.Id_Lesson where LT.Lesson_Language = @language and L.Id_Course = @id";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
        var p = command.ExecuteScalar();
        if (p==DBNull.Value)
          Level = 1;
        else
        {
          MaxLevelIntLang = (decimal)p;
          if (MaxLevelIntLang + 1 < Level)
            Level = MaxLevelIntLang + 1;
        }
        //ten cały fragment poniżej musi zostać zrekonstruowany. Jego celem ma być znalezienie, czy isnieje rekord w tabeli Lessons odpowiadający danej lekcji
        //szukanie lekcji w danym języku z największym możliwym przypisanym poziomem dla danego kursu
        //command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = (select max(Lesson_Level) from [Lesson] where Id_Course = @id) and Id_Course=@id)";
        //command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level = (select max(Lesson_Level) from [Lesson] where Id_Course = @id) and Id_Course=@id";
        //command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        //command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
        command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id";
        //command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        //command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
        MaxLevel = command.ExecuteScalar() == null ? false : true;
      }
      if (MaxLevel)
      {
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id)";
          command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
          Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());
        }
        if (Lesson_TitleID !=0)
        {
          /*using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            //znajdź maksymalny poziom
            connection.Open();
            command.Connection = connection;
            command.CommandText = "";
          }
          if()
          { */
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            //znajdź maksymalny poziom
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select max(Lesson_Level) from [Lesson] where Id_Course = @id";
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            MaxLevelInt = System.Convert.ToInt32(command.ExecuteScalar());
            if (MaxLevelInt == MaxLevelIntLang)
            {
              //stwórz instancję lekcji o poziomie wyższym o jeden
              command.CommandText = "insert into [Lesson] (Lesson_Level, Id_Course) output inserted.Id_Lesson values(@level, @id_course)";
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = MaxLevelInt + 1;
              //command.Parameters.Add("@parameter", SqlDbType.NVarChar).Value = Country.ToLower() + (MaxLevelInt + 1).ToString();
              command.Parameters.Add("@id_course", SqlDbType.Int).Value = CourseID;
              LessonID = (int)command.ExecuteScalar();
            }

            //}
          }
        }
        else
        {
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id";
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            LessonID = System.Convert.ToInt32(command.ExecuteScalar());
          }
        }
      }
      else
      {
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          //stwórz nową lekcję na odpowiednim poziomie
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Lesson] (Lesson_Level, Id_Course) output inserted.Id_Lesson values(@level, @id_course)";
          command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
          command.Parameters.Add("@id_course", SqlDbType.Int).Value = CourseID;
          LessonID = (int)command.ExecuteScalar();
        }
      }
      //sprawdzanie, czy istnieje już lekcja przypisana do danego poziomu
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        /*command.CommandText = "select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
        LessonID = System.Convert.ToInt32(command.ExecuteScalar());*/
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
            command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = MaxLevelIntLang + 1;
            command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
            command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
            command.Parameters.Add("@level", SqlDbType.Decimal).Value = MaxLevelIntLang;
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
        command.CommandText = "insert into [Lesson_Title] (Lesson_Language, Lesson_Title, Id_Lesson) output inserted.Id_Lesson_Title values (@language, @title, (select Id_Lesson from [Lesson] where Lesson_Level=@level and Id_Course=@id))";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id", SqlDbType.Int).Value = CourseID;
        Lesson_TitleID = (int)command.ExecuteScalar();
        /*command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language = @language2 and Id_Lesson =(select Id_Lesson from [Lesson] where Lesson_Level=@level2 and Id_Course=@id2)";
        command.Parameters.Add("@language2", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@level2", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@id2", SqlDbType.NVarChar).Value = CourseID;
        Lesson_TitleID = System.Convert.ToInt32(command.ExecuteScalar());*/
      }
      //dodanie kontentu do lekcji
      foreach (var x in EditedLessons)
      {
        int LessonContentId = 0;
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Lesson_Content] (Lesson_Text, Id_Lesson_Title) output inserted.Id_Lesson_Content values (@text, @id)";
          command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
          command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
          LessonContentId = (int)command.ExecuteScalar();
        }
        /*int LessonContentId = 0;
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "select Id_Lesson_Content from [Lesson_Content] where Lesson_Text = @text and Id_Lesson_Title = @id";
          command.Parameters.Add("@text", SqlDbType.NVarChar).Value = x.LessonText;
          command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_TitleID;
          LessonContentId = System.Convert.ToInt32(command.ExecuteScalar());
        }*/
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Adding Lesson:", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }

    public void UpdateLesson(string Country, string Language, ObservableCollection<LessonData> EditedLessons, string OldTitle, string Title, decimal Level, int CourseID, int Lesson_Id, int Lesson_Title_Id)
    {
      decimal levl = 0;
      string currenttitle = null;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Editing Lesson. Start!");
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
              command.CommandText = "update [Lesson_Title] set Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @leveldown and Id_Course = @id) " +
                "where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id)";
              command.Parameters.Add("@leveldown", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl + 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
              //command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
              //command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + levl.ToString() + "%";
              //int count = System.Convert.ToInt32(command.ExecuteScalar());
              command.CommandText = "update [Exercise] set Exercise_Level = @leveldown where Exercise_Language=@language and Exercise_Level=@level and Id_Course=@id";
              //command.Parameters.Add("@param2", SqlDbType.NVarChar).Value = Country + levl.ToString() + (count + 1).ToString();
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
              command.CommandText = "update [Lesson_Title] set Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @levelup and Id_Course = @id) " +
                "where Lesson_Language = @language and Id_Lesson = (select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course = @id)";
              command.Parameters.Add("@levelup", SqlDbType.Decimal).Value = levl;
              command.Parameters.Add("@id", SqlDbType.NVarChar).Value = CourseID;
              command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
              command.Parameters.Add("@level", SqlDbType.Decimal).Value = levl - 1;
              command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
              command.ExecuteNonQuery();
              //command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
              //command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + levl.ToString() + "%";
              //int count = System.Convert.ToInt32(command.ExecuteScalar());
              command.CommandText = "update [Exercise] set Exercise_Level = @levelup where Exercise_Language=@language and Exercise_Level=@level and Id_Course=@id";
              //command.Parameters.Add("@param2", SqlDbType.NVarChar).Value = Country + levl.ToString() + (count + 1).ToString();
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
        //command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
        //command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + Level.ToString() + "%";
        //int count = System.Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "update [Exercise] set Exercise_Level = @level where Exercise_Level = @oldlevel and Exercise_Language=@language and Id_Course = @id";
        //command.Parameters.Add("@param2", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
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
          int LessonContentId = 0;
          using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Lesson_Content] (Lesson_Text, Id_Lesson_Title) output inserted.Id_Lesson_Content values(@text, @titleid)";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@titleid", SqlDbType.Int).Value = Lesson_Title_Id;
            LessonContentId = (int)command.ExecuteScalar();
          }
          /*using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select Id_Lesson_Content from [Lesson_Content] where Lesson_Text = @text and Id_Lesson_Title = @id";
            command.Parameters.Add("@text", SqlDbType.NVarChar).Value = EditedLessons[i].LessonText;
            command.Parameters.Add("@id", SqlDbType.Int).Value = Lesson_Title_Id;
            LessonContentId = System.Convert.ToInt32(command.ExecuteScalar());
          }*/
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
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Editing Lesson", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      //Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
  }
}
