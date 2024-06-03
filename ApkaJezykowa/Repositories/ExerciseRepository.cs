using ApkaJezykowa.Commands;
using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

namespace ApkaJezykowa.Repositories
{
  internal class ExerciseRepository : BaseRepository, IExerciseRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    IMongoCollection<CourseModel> courseCollection;
    IMongoCollection<ExerciseModel> exerciseCollection;
    IMongoCollection<ExerciseContentModel> exerciseContentCollection;
    IMongoCollection<LessonModelDB> lessonCollection;
    IMongoCollection<LessonTitleModel> lessonTitleCollection;
    public ExerciseRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
      var database = SpeechServiceKey.Instance.Client.GetDatabase("CourseBase");
      courseCollection = database.GetCollection<CourseModel>("Course");
      exerciseCollection = database.GetCollection<ExerciseModel>("Exercise");
      exerciseContentCollection = database.GetCollection<ExerciseContentModel>("Exercise_Content");
      lessonCollection = database.GetCollection<LessonModelDB>("Lesson");
      lessonTitleCollection = database.GetCollection<LessonTitleModel>("Lesson_Title");
    }
    public ObservableCollection<ExerciseContentModel> Display(int Id)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var rnd = new Random();
      var filter = Builders<ExerciseContentModel>.Filter.Eq("Id_Exercise", Id);
      var result = exerciseContentCollection.Aggregate().Match(filter).AppendStage<ExerciseContentModel>($@"{{ $sample: {{ size: {10} }} }}").ToList();
      ObservableCollection<ExerciseContentModel> Exercises = new ObservableCollection<ExerciseContentModel>(result.OrderBy(item => rnd.Next()));
      //Console.WriteLine("Fetching Exercise Tasks Data. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 10 * from [Exercise_Content] where Id_Exercise = @id order by NEWID()";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            ExerciseContentModel model = new ExerciseContentModel();
            model.Id = (int)reader["Id_Exercise_Content"];
            model.Task = reader["Task"].ToString();
            model.Answer = reader["Answer"].ToString();
            model.Answer2 = reader["Answer2"].ToString();
            model.Answer3 = reader["Answer3"].ToString();
            model.Tip = reader["Tip"].ToString();
            model.Id_Exercise = (int)reader["Id_Exercise"];
            Exercises.Add(model);
          }
          reader.NextResult();
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Exercise Task Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return Exercises;
    }
    public List<ExerciseListModel> Display_Exercise_List(string Language, string Country)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<CourseModel>.Filter.Eq("Course_Name",Language);
      var projection = Builders<CourseModel>.Projection.Expression(item=>item.Id);
      var result = courseCollection.Find(filter).Project(projection).FirstOrDefault();
      var filterBuilder2 = Builders<ExerciseModel>.Filter;
      var filter2 = filterBuilder2.Empty;
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Exercise_Language", Country));
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Id_Course", result));
      var projection2 = Builders<ExerciseModel>.Projection.Expression(item=>new ExerciseListModel
      {
        Id_Exercise = item.Id,
        Exercise_Level = item.ExerciseLevel,
        Exercise_Title = item.ExerciseTitle,
        Task_Text = item.TaskText
      });
      var sort = Builders<ExerciseModel>.Sort.Ascending("Exercise_Level");
      var result2 = exerciseCollection.Find(filter2).Sort(sort).Project(projection2).ToList();
      //Console.WriteLine("Fetching Exercise List. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Exercise, Exercise_Level, Exercise_Title, Task_text from [Exercise] where Exercise_Language=@country and Id_Course in(Select Id_Course from [Course] where [Course_Name] = @language) order by Exercise_Level ASC";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@country",SqlDbType.NVarChar).Value = Country;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            ExerciseListModel list = new ExerciseListModel();
            list.Id_Exercise = (int)reader["Id_Exercise"];
            list.Exercise_Level = (decimal)reader["Exercise_Level"];
            list.Exercise_Title = reader["Exercise_Title"].ToString();
            list.Task_Text = reader["Task_text"].ToString();
            ExerciseList.Add(list);
          }
          reader.NextResult();
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Exercise List", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return new List<ExerciseListModel>(result2);
    }
    public List<Pars> Obtain_Pars(string Language)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<CourseModel>.Filter.Eq("Course_Name", Language);
      var projection = Builders<CourseModel>.Projection.Expression(item => item.Id);
      var result = courseCollection.Find(filter).Project(projection).FirstOrDefault();
      var filter2 = Builders<ExerciseModel>.Filter.Eq("Id_Course", result);
      var projection2 = Builders<ExerciseModel>.Projection.Expression(item => new Pars
      {
        title = item.ExerciseTitle,
        id = item.Id,
        text = item.TaskText
      });//Include("_id").Include("Exercise_Title").Include("Task_Text");
      var sort = Builders<ExerciseModel>.Sort.Ascending("Exercise_Level");
      var result2 = exerciseCollection.Find(filter2).Sort(sort).Project(projection2).ToList();
      //Console.WriteLine("Fetching Exercise Ids. Start!");
      /*using (var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Exercise, Exercise_Title, Task_text from [Exercise] where Id_Course in(Select Id_Course from [Course] where [Course_Name] = @language) order by Exercise_Level ASC";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Pars par = new Pars();
            par.title = reader["Exercise_Title"].ToString();
            par.id = (int)reader["Id_Exercise"];
            par.text = reader["Task_text"].ToString();
            pars.Add(par);
          }
          reader.NextResult();
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching ercise Ids", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return new List<Pars>(result2);
    }
    public ObservableCollection<TestData> Enter_Test_Mode(int Id, string Language)
    {
      //List<int> ids = new List<int>();
      //List<string> tasks = new List<string>();
      //List<bool> check = new List<bool>();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<CourseModel>.Filter.Eq("Course_Name", Language);
      var projection = Builders<CourseModel>.Projection.Expression(item => item.Id);
      var result = courseCollection.Find(filter).Project(projection).FirstOrDefault();
      var filter2 = Builders<ExerciseModel>.Filter.Eq("_id", Id);
      var projection2 = Builders<ExerciseModel>.Projection.Expression(item => item.ExerciseLevel);
      var result2 = exerciseCollection.Find(filter2).Project(projection2).FirstOrDefault();
      var filterBuilder3 = Builders<ExerciseModel>.Filter;
      var filter3 = filterBuilder3.Empty;
      filter3 = filterBuilder3.And(filter3, filterBuilder3.Eq("Id_Course", result));
      filter3 = filterBuilder3.And(filter3, filterBuilder3.Eq(item => item.ExerciseLevel, result2));
      filter3 = filterBuilder3.And(filter3, filterBuilder3.Ne("_id", Id));
      var projection3 = Builders<ExerciseModel>.Projection.Expression(item => new TestData
      {
        TestId = item.Id,
        TestTasks = item.TaskText,
        TestDone = false
      });//Include("_id").Include("Task_Text");
      var result3 = exerciseCollection.Aggregate().Match(filter3).AppendStage<ExerciseModel>($@"{{ $sample: {{ size: {3} }} }}").Project(projection3).ToList();

      //Console.WriteLine("Fetching Three Random Exercises. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select Top 3 Id_Exercise, Task_text from [Exercise] where Id_Exercise !=@id and Exercise_Level = (select Exercise_Level from [Exercise] Where Id_Exercise = @id) and Id_Course in (Select Id_Course from [Course] where [Course_Name] = @language) order by NEWID()";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id;
        command.Parameters.Add("@language",SqlDbType.NVarChar).Value = Language;
        using(var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            TestData data = new TestData();
            data.TestId = (int)reader["Id_Exercise"];
            data.TestTasks = reader["Task_text"].ToString();
            data.TestDone = false;
            TestingData.Add(data);
          }
          reader.NextResult();
          //TestModel.instance.TestId = ids;
          //TestModel.instance.TestTasks = tasks;
          //TestModel.instance.Test_Done = check;
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Three Random Exercises", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return new ObservableCollection<TestData>(result3);
    }
    public List<string> Obtain_Exercise_Names(string Country, string Language, int Level)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filterBuilder2 = Builders<ExerciseModel>.Filter;
      var filter2 = filterBuilder2.Empty;
      if (Country != "None")
      {
        var filterBuilder = Builders<CourseModel>.Filter;
        var filter = filterBuilder.Empty;
        filter = filterBuilder.And(filter, filterBuilder.Eq("Course_Name", Country));
        var projection = Builders<CourseModel>.Projection.Expression(item => item.Id);
        var result = courseCollection.Find(filter).Project(projection).FirstOrDefault();
        filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Id_Course", result));
      }
      if (Language != "None")
      {
        filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Exercise_Language",Language));
      }
      if (Level != 0)
      {
        filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Exercise_Level", Level));
      }
      var projection2 = Builders<ExerciseModel>.Projection.Expression(item => item.ExerciseTitle);
      var result2 = exerciseCollection.Find(filter2).Project(projection2).ToList();
      List<string> ex_nam = new List<string>
      {
        "None"
      };
      ex_nam.AddRange(result2);

      //Console.WriteLine("Fetching Exercise Names. Start!");
      /*using (var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Exercise_Title from [Exercise] where Exercise_Level = Coalesce(@level,Exercise_Level) and Exercise_Language = Coalesce(@language, Exercise_Language) and Id_Course in (select Id_Course from [Course] where [Course_Name] = Coalesce(@country, [Course_Name])) order by Id_Exercise";
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country ?? (object)DBNull.Value;
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language ?? (object)DBNull.Value;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = DecimalLevel ?? (object)DBNull.Value;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            ex_nam.Add(reader["Exercise_Title"].ToString());
          }
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Exercise Names", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return ex_nam;
    }
    public ObservableCollection<ExerciseData> Obtain_Exercise_Content(string Exercise)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<ExerciseModel>.Filter.Eq("Exercise_Title", Exercise);
      var projection = Builders<ExerciseModel>.Projection.Expression(item => item.Id);
      var result = exerciseCollection.Aggregate().Match(filter).Project(projection).ToList();
      var filter2 = Builders<ExerciseContentModel>.Filter.Eq("Id_Exercise",result);
      var projection2 = Builders<ExerciseContentModel>.Projection.Expression(item=>new ExerciseData
      {
        Exercise_Content_Id = item.Id,
        Task = item.Task,
        Answer1 = item.Answer,
        Answer2 = item.Answer2,
        Answer3 = item.Answer3,
        Tip = item.Tip
      });//Exclude("Id_Exercise");
      ObservableCollection<ExerciseData> ec = new ObservableCollection<ExerciseData>(exerciseContentCollection.Aggregate().Match(filter2).Project<ExerciseData>(projection2).ToList());
      //Console.WriteLine("Fetching Exercise Content. Start!");
      /*using (var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Exercise_Content, Task, Answer, Answer2, Answer3, Tip from Exercise_Content where Id_Exercise = (select Id_Exercise from [Exercise] where Exercise_Title = @title)";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Exercise;
        using(var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            ExerciseData data = new ExerciseData();
            data.Exercise_Content_Id = (int)reader["Id_Exercise_Content"];
            data.Task = reader["Task"].ToString();
            data.Answer1 = reader["Answer"].ToString();
            data.Answer2 = reader["Answer2"].ToString();
            data.Answer3 = reader["Answer3"].ToString();
            data.Tip = reader["Tip"].ToString();
            ec.Add(data);
          }
          reader.NextResult();
        }*/
      //return ec
      //}
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Exercise Content", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return ec;
    }
    public ExerciseParamModel Obtain_Exercise_Parameters(string Exercise)
    {
      ExerciseParamModel result = null;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var query = (from c in courseCollection.AsQueryable()
                   join e in exerciseCollection on c.Id equals e.IdCourse
                   where (e.ExerciseTitle==Exercise)
                   select new ExerciseParamModel
                   {
                      courseId = c.Id,
                      exerciseID = e.Id,
                      country = c.CourseName,
                      language = e.ExerciseLanguage,
                      title = e.ExerciseTitle,
                      task_Text = e.TaskText,
                      level = e.ExerciseLevel
                   }).FirstOrDefault();
      //Console.WriteLine("Obtaining Exercise Parameters. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select C.Id_Course, C.[Course_Name], E.Exercise_Level, E.Exercise_Language, E.Id_Exercise, E.Exercise_Title, E.Task_Text From [Course] C join [Exercise] E on C.Id_Course=E.Id_Course where E.Exercise_Title=@title";
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Exercise;
        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            result = new ExerciseParamModel()
            {
              courseId = (int)reader["Id_Course"],
              exerciseID = (int)reader["Id_Exercise"],
              country = reader["Course_Name"].ToString(),
              language = reader["Exercise_Language"].ToString(),
              title = reader["Exercise_Title"].ToString(),
              task_Text = reader["Task_Text"].ToString(),
              level = (decimal)reader["Exercise_Level"]
            };
          }
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Exercise Parameters", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return query;
    }
    public bool DoesLessonExist(string Country, string Language, int Level)
    {
      bool p;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<CourseModel>.Filter.Eq("Course_Name", Country);
      var projection = Builders<CourseModel>.Projection.Expression(item => item.Id);
      var result = courseCollection.Find(filter).Project(projection).FirstOrDefault();
      var filterBuilder2 = Builders<LessonModelDB>.Filter;
      var filter2 = filterBuilder2.Empty;
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Lesson_Level", Level));
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Id_Course", result));
      var projection2 = Builders<LessonModelDB>.Projection.Expression(item => item.Id);
      var result2 = lessonCollection.Find(filter2).Project(projection2).FirstOrDefault();
      var filterBuilder3 = Builders<LessonTitleModel>.Filter;
      var filter3 = filterBuilder3.Empty;
      filter3 = filterBuilder3.And(filter3, filterBuilder3.Eq("Id_Lesson", result2));
      filter3 = filterBuilder3.And(filter3, filterBuilder3.Eq("Lesson_Language", Language));
      p = lessonTitleCollection.Find(filter3).FirstOrDefault() == null ? false : true;
      //Console.WriteLine("Finding Exercise. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id_Lesson_Title from [Lesson_Title] where Lesson_Language=@language and Id_Lesson=" +
          "(select Id_Lesson from [Lesson] where Lesson_Level = @level and Id_Course=" +
          "(select Id_Course from [Course] where [Course_Name] = @country))";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
        p = command.ExecuteScalar() == null ? false : true;
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Finding Exercise", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return p;
    }
    public void AddExercise(string Country, string Language, ObservableCollection<ExerciseData> EditedExercises, string Title, int Level, string TaskText)
    {
      
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var sort = Builders<ExerciseModel>.Sort.Descending("_id");
      var projection = Builders<ExerciseModel>.Projection.Expression(item => item.Id);
      var result = exerciseCollection.Find(new BsonDocument()).Sort(sort).Project(projection).FirstOrDefault();
      var filter2 = Builders<CourseModel>.Filter.Eq("Course_Name", Country);
      var projection2 = Builders<CourseModel>.Projection.Expression(item => item.Id);
      var result2 = courseCollection.Find(filter2).Project(projection2).FirstOrDefault();
      var exercise = new ExerciseModel
      {
        Id = result+1,
        ExerciseLanguage = Language,
        ExerciseLevel = Level,
        ExerciseTitle = Title,
        TaskText = TaskText,
        IdCourse = result2,
      };
      exerciseCollection.InsertOne(exercise);
      int id=result+1;
      //Console.WriteLine("Adding Exercise. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        //command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
        //command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + Level.ToString() + "%";
        //int count = System.Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "insert into [Exercise] (Exercise_Language, Exercise_Level, Exercise_Title, Task_Text, Id_Course) output inserted.Id_Exercise values (@language, @level, @title, @tasktext, (select Id_Course from [Course] where [Course_Name] = @country))";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
        //command.Parameters.Add("@parameter", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
        command.Parameters.Add("@tasktext", SqlDbType.NVarChar).Value = TaskText;
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
        id = (int)command.ExecuteScalar();
        //command.CommandText = "select Id_Exercise from [Exercise] where Exercise_Parameter = @param2";
        //command.Parameters.Add("@param2", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
        //id = System.Convert.ToInt32(command.ExecuteScalar());
      }*/

      foreach (var x in EditedExercises)
      {
        var sort3 = Builders<ExerciseContentModel>.Sort.Descending("_id");
        var projection3 = Builders<ExerciseContentModel>.Projection.Include("_id");
        var result3 = exerciseContentCollection.Find(new BsonDocument()).Sort(sort3).Project<int>(projection3).FirstOrDefault();
        var exercisecontent = new ExerciseContentModel
        {
          Id = result3+1,
          Task = x.Task,
          Answer = x.Answer1,
          Answer2 = x.Answer2,
          Answer3 = x.Answer3,
          Tip = x.Tip,
          Id_Exercise = id
        };
        exerciseContentCollection.InsertOne(exercisecontent);
        /*using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          command.CommandText = "insert into [Exercise_Content] values (@task, @answer1, @answer2, @answer3, @tip, @id)";
          command.Parameters.Add("@task", SqlDbType.NVarChar).Value = x.Task;
          command.Parameters.Add("@answer1", SqlDbType.NVarChar).Value = x.Answer1;
          command.Parameters.Add("@answer2", SqlDbType.NVarChar).Value = x.Answer2 ?? (object)DBNull.Value;
          command.Parameters.Add("@answer3", SqlDbType.NVarChar).Value = x.Answer3 ?? (object)DBNull.Value;
          command.Parameters.Add("@tip", SqlDbType.NVarChar).Value = x.Tip;
          command.Parameters.Add("@id", SqlDbType.Int).Value = id;
          command.ExecuteNonQuery();
        }*/
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Adding Exercise", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
    
    public void EditExercise(string Country, string Language, ObservableCollection<ExerciseData> EditedExercises, string TaskText, string OldTitle, string Title, int Level, int CourseID, int Exercise_Id)
    {

      //few changes and improvements will be needed, certain actions are being initiaied unecessarily
      //everything is going to be moved to the dabase itself as a procedure
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Editing Exercise. Start!");
      var filter = Builders<ExerciseModel>.Filter.Eq("_id", Exercise_Id);
      var update = Builders<ExerciseModel>.Update.Set("Exercise_Level", Level).Set("Task_Text", TaskText);
      exerciseCollection.UpdateOne(filter, update);
      var filter2 = Builders<ExerciseContentModel>.Filter.Eq("Id_Exercise",Exercise_Id);
      var projection2 = Builders<ExerciseContentModel>.Projection.Expression(item => new ExerciseData
      {
        Exercise_Content_Id = item.Id,
        Task = item.Task,
        Answer1 = item.Answer,
        Answer2 = item.Answer2,
        Answer3 = item.Answer3,
        Tip = item.Tip
      });
      ObservableCollection<ExerciseData> data = new ObservableCollection<ExerciseData>(exerciseContentCollection.Find(filter2).Project(projection2).ToList());
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        //command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
        //command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + Level.ToString() + "%";
        //int count = System.Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "update [Exercise] set Exercise_Level = @level, Task_text=@tasktext where Id_Exercise=@id";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        //command.Parameters.Add("@parameter", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
        command.Parameters.Add("@tasktext", SqlDbType.NVarChar).Value = TaskText;
        command.Parameters.Add("@id", SqlDbType.Int).Value = Exercise_Id;
        command.ExecuteScalar();
        
        command.CommandText = "select Id_Exercise_Content, Task, Answer, Answer2, Answer3, Tip from [Exercise_Content] where Id_Exercise = @id";
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            ExerciseData content = new ExerciseData();
            content.Exercise_Content_Id = (int)reader["Id_Exercise_Content"];
            content.Task = reader["Task"].ToString();
            content.Answer1 = reader["Answer"].ToString();
            content.Answer2 = reader["Answer2"].ToString();
            content.Answer3 = reader["Answer3"].ToString();
            content.Tip = reader["Tip"].ToString();
            data.Add(content);
          }
          reader.NextResult();
        }
      }*/
      foreach (var x in data)
      {

        /*using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;*/
          if (EditedExercises.Any(e => e.Exercise_Content_Id == x.Exercise_Content_Id))
          {
            if (EditedExercises.Any(e => e.Exercise_Content_Id == x.Exercise_Content_Id && (e.Answer1 != x.Answer1 || e.Answer2 != x.Answer2 || e.Answer3 != x.Answer3 || e.Task != x.Task || e.Tip != x.Tip)))
            {
              var filter3 = Builders<ExerciseContentModel>.Filter.Eq("Id_Exercise", x.Exercise_Content_Id);
              var update3 = Builders<ExerciseContentModel>.Update.Set("Task", x.Task).Set("Answer", x.Answer1).Set("Answer2", x.Answer2).Set("Answer3", x.Answer3).Set("Tip", x.Tip);
              exerciseContentCollection.UpdateOne(filter3, update3);
              /*command.CommandText = "update [Exercise_Content] set Task = @task, Answer = @answer1, Answer2 = @answer2, Answer3 = @answer3, Tip = @tip where Id_Exercise_Content = @id2";
              command.Parameters.Add("@task", SqlDbType.NVarChar).Value = x.Task;
              command.Parameters.Add("@answer1", SqlDbType.NVarChar).Value = x.Answer1;
              command.Parameters.Add("@answer2", SqlDbType.NVarChar).Value = x.Answer2 ?? (object)DBNull.Value;
              command.Parameters.Add("@answer3", SqlDbType.NVarChar).Value = x.Answer3 ?? (object)DBNull.Value;
              command.Parameters.Add("@tip", SqlDbType.NVarChar).Value = x.Tip;
              command.Parameters.Add("@id2", SqlDbType.Int).Value = x.Exercise_Content_Id;
              command.ExecuteNonQuery();*/
            }
            var ToRemove = EditedExercises.Where(e => e.Exercise_Content_Id == x.Exercise_Content_Id).First();
            EditedExercises.Remove(ToRemove);
          }
          else
          {
            var filter3 = Builders<ExerciseContentModel>.Filter.Eq("Id_Exercise", x.Exercise_Content_Id);
            exerciseContentCollection.DeleteOne(filter3);
            /*command.CommandText = "delete from [Exercise_Content] where Id_Exercise_Content = @id2";
            command.Parameters.Add("@id2", SqlDbType.Int).Value = x.Exercise_Content_Id;
            command.ExecuteNonQuery();*/
          }
        //}
      }
        if(EditedExercises.Count > 0)
        {
          foreach(var p in EditedExercises)
          {
          var sort3 = Builders<ExerciseContentModel>.Sort.Descending("_id");
          var projection3 = Builders<ExerciseContentModel>.Projection.Expression(item=>item.Id);
          var result3 = exerciseContentCollection.Find(new BsonDocument()).Sort(sort3).Project(projection3).FirstOrDefault();
          var exercisecontent = new ExerciseContentModel
          {
            Id = result3+1,
            Task = p.Task,
            Answer = p.Answer1,
            Answer2 = p.Answer2,
            Answer3 = p.Answer3,
            Tip = p.Tip,
            Id_Exercise = Exercise_Id
          };
          exerciseContentCollection.InsertOne(exercisecontent);
          /*using (var connection = GetCourseConnection())
          using (var command = new SqlCommand())
          {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into [Exercise_Content] values (@task, @answer1, @answer2,@answer3,@tip,@id)";
            command.Parameters.Add("@task", SqlDbType.NVarChar).Value = p.Task;
            command.Parameters.Add("@answer1", SqlDbType.NVarChar).Value = p.Answer1;
            command.Parameters.Add("@answer2", SqlDbType.NVarChar).Value = p.Answer2 ?? (object)DBNull.Value;
            command.Parameters.Add("@answer3", SqlDbType.NVarChar).Value = p.Answer3 ?? (object)DBNull.Value;
            command.Parameters.Add("@tip", SqlDbType.NVarChar).Value = p.Tip;
            command.Parameters.Add("@id", SqlDbType.Int).Value = Exercise_Id;
            command.ExecuteNonQuery();
          }*/
        }
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Editing Exercise", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
  }
}
