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
  }
}
