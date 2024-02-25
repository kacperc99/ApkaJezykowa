using ApkaJezykowa.Commands;
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
  internal class ExerciseRepository : BaseRepository, IExerciseRepository
  {
    public void Display(ObservableCollection<ExerciseModel> Exercises, int Id)
    {
      using (var connection = GetCourseConnection())
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
            ExerciseModel model = new ExerciseModel();
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
      }
    }
    public void Display_Exercise_List(List<ExerciseListModel> ExerciseList, string Language, string Country)
    {
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Exercise_Level, Exercise_Title, Exercise_Parameter, Task_text from [Exercise] where Exercise_Language=@country and Id_Course in(Select Id_Course from [Course] where [Course_Name] = @language) order by Exercise_Level ASC";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@country",SqlDbType.NVarChar).Value = Country;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
            ExerciseListModel list = new ExerciseListModel();
            list.Exercise_Level = (decimal)reader["Exercise_Level"];
            list.Exercise_Title = reader["Exercise_Title"].ToString();
            list.Exercise_Parameter = reader["Exercise_Parameter"].ToString();
            list.Task_Text = reader["Task_text"].ToString();
            ExerciseList.Add(list);
          }
          reader.NextResult();
        }
      }
    }
    public void Obtain_Pars(List<Pars> pars, string Language)
    {
      using(var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Exercise_Parameter, Id_Exercise, Task_text from [Exercise] where Id_Course in(Select Id_Course from [Course] where [Course_Name] = @language) order by Exercise_Level ASC";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Pars par = new Pars();
            par.par = reader["Exercise_Parameter"].ToString();
            par.id = (int)reader["Id_Exercise"];
            par.text = reader["Task_text"].ToString();
            pars.Add(par);
          }
          reader.NextResult();
        }
      }
    }
    public void Enter_Test_Mode(int Id, string Language, ObservableCollection<TestData> TestingData)
    {
      //List<int> ids = new List<int>();
      //List<string> tasks = new List<string>();
      //List<bool> check = new List<bool>();
      using (var connection = GetCourseConnection())
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
      }
    }
    public List<string> Obtain_Exercise_Names(string Country, string Language, decimal Level)
    {
      if (Country == "None")
        Country = null;
      if (Language == "None")
        Language = null;
      Nullable<decimal> DecimalLevel = Level;
      if (DecimalLevel == 0)
        DecimalLevel = null;
      List<string> ex_nam = new List<string>();
      ex_nam.Add("None");
      using(var connection = GetCourseConnection())
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
      }
      return ex_nam;
    }
    public ObservableCollection<ExerciseData> Obtain_Exercise_Content(string Exercise)
    {
      ObservableCollection<ExerciseData> ec = new ObservableCollection<ExerciseData>();
      using(var connection = GetCourseConnection())
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
        }
        return ec;
      }
    }
    public ExerciseParamModel Obtain_Exercise_Parameters(string Exercise)
    {
      ExerciseParamModel result = null;
      using (var connection = GetCourseConnection())
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
      }
      return result;
    }
    public bool DoesLessonExist(string Country, string Language, decimal Level)
    {
      using (var connection = GetCourseConnection())
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
        return command.ExecuteScalar() == null ? false : true;
      }
    }
    public void AddExercise(string Country, string Language, ObservableCollection<ExerciseData> EditedExercises, string Title, decimal Level, string TaskText)
    {
      int id;
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
        command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + Level.ToString() + "%";
        int count = System.Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "insert into [Exercise] values (@language, @level, @title, @parameter, @tasktext, (select Id_Course from [Course] where [Course_Name] = @country))";
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
        command.Parameters.Add("@parameter", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
        command.Parameters.Add("@tasktext", SqlDbType.NVarChar).Value = TaskText;
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
        command.ExecuteNonQuery();
        command.CommandText = "select Id_Exercise from [Exercise] where Exercise_Parameter = @param2";
        command.Parameters.Add("@param2", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
        id = System.Convert.ToInt32(command.ExecuteScalar());
      }
    
        foreach (var x in EditedExercises)
        {
          using (var connection = GetCourseConnection())
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
          }
        }
     }
    
    public void EditExercise(string Country, string Language, ObservableCollection<ExerciseData> EditedExercises, string TaskText, string OldTitle, string Title, decimal Level, int CourseID, int Exercise_Id)
    {
      ObservableCollection<ExerciseData> data = new ObservableCollection<ExerciseData>();
      //few changes and improvements will be needed, certain actions are being initiaied unecessarily
      //everything is going to be moved to the dabase itself as a procedure
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select COUNT(*) from [Exercise] where Exercise_Parameter Like (@param)";
        command.Parameters.Add("@param", SqlDbType.NVarChar).Value = Country + Level.ToString() + "%";
        int count = System.Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "update [Exercise] set Exercise_Level = @level, Exercise_Parameter = @parameter, Task_text=@tasktext where Id_Exercise=@id";
        command.Parameters.Add("@level", SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@parameter", SqlDbType.NVarChar).Value = Country + Level.ToString() + (count + 1).ToString();
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
      }
      foreach (var x in data)
      {
        using (var connection = GetCourseConnection())
        using (var command = new SqlCommand())
        {
          connection.Open();
          command.Connection = connection;
          if (EditedExercises.Any(e => e.Exercise_Content_Id == x.Exercise_Content_Id))
          {
            if (EditedExercises.Any(e => e.Exercise_Content_Id == x.Exercise_Content_Id && (e.Answer1 != x.Answer1 || e.Answer2 != x.Answer2 || e.Answer3 != x.Answer3 || e.Task != x.Task || e.Tip != x.Tip)))
            {
              command.CommandText = "update [Exercise_Content] set Task = @task, Answer = @answer1, Answer2 = @answer2, Answer3 = @answer3, Tip = @tip where Id_Exercise_Content = @id2";
              command.Parameters.Add("@task", SqlDbType.NVarChar).Value = x.Task;
              command.Parameters.Add("@answer1", SqlDbType.NVarChar).Value = x.Answer1;
              command.Parameters.Add("@answer2", SqlDbType.NVarChar).Value = x.Answer2 ?? (object)DBNull.Value;
              command.Parameters.Add("@answer3", SqlDbType.NVarChar).Value = x.Answer3 ?? (object)DBNull.Value;
              command.Parameters.Add("@tip", SqlDbType.NVarChar).Value = x.Tip;
              command.Parameters.Add("@id2", SqlDbType.Int).Value = x.Exercise_Content_Id;
              command.ExecuteNonQuery();
            }
            var ToRemove = EditedExercises.Where(e => e.Exercise_Content_Id == x.Exercise_Content_Id).First();
            EditedExercises.Remove(ToRemove);
          }
          else
          {
            command.CommandText = "delete from [Exercise_Content] where Id_Exercise_Content = @id2";
            command.Parameters.Add("@id2", SqlDbType.Int).Value = x.Exercise_Content_Id;
            command.ExecuteNonQuery();
          }
        }
      }
        if(EditedExercises.Count > 0)
        {
          foreach(var p in EditedExercises)
          {
          using (var connection = GetCourseConnection())
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
          
          }
        }
      }
    }
  }
}
