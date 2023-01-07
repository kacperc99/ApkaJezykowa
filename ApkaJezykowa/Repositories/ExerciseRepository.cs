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
  internal class ExerciseRepository : BaseRepository, IExerciseRepository
  {
    public void Display(int Level, string Language)
    {
      List<string> Tasktmp = new List<string>();
      List<string> Answertmp = new List<string>();
      List<string> Tiptmp = new List<string>();
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 10 * from [Exercise] where Exercise_Level=@level and Id_Course =(Select Id from [Course] where [Course_Name] = @language and Course_Level = @level) order by NEWID()";
        command.Parameters.Add("@level",SqlDbType.Decimal).Value = Level;
        command.Parameters.Add("@language", SqlDbType.NVarChar).Value = Language;
        using (var reader = command.ExecuteReader())
        {
          while(reader.Read())
          {
              ExerciseModel.Instance.Id = reader["Id"].ToString();
              ExerciseModel.Instance.ExerciseLevel = (decimal)reader["Exercise_Level"];
              ExerciseModel.Instance.TaskText = reader["Task_Text"].ToString();
              Tasktmp.Add(reader["Task"].ToString());
              Answertmp.Add(reader["Answer"].ToString());
              Tiptmp.Add(reader["Tip"].ToString());
              ExerciseModel.Instance.Id_Course = reader["Id_Course"].ToString();
          }
          reader.NextResult();
          ExerciseModel.Instance.Exercise = Tasktmp;
          ExerciseModel.Instance.Answer = Answertmp;
          ExerciseModel.Instance.Tip = Tiptmp;
        }
      }
    }
  }
}
