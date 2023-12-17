using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;
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
  }
}
