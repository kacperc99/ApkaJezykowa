using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  internal class ProgressCardRepository : BaseRepository, IProgressCardRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    public ProgressCardRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
    }
    public bool IsUserSignedIn(string username, string language, string country)
    {
      bool IsUserSigned;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Checking Level Roles. Start!");
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        //command.CommandText = "select uc.Id_Course from [User_Course] uc where uc.Id_User=(select u.Id_User from [User] u where u.[Username]=@username) and uc.Id_Course in (select c.Id_Course from [Course] c where c.Course_Name=@country)";
        command.CommandText = "select Id_Progress_Card from [Progress_Card] where Lang_Course=@country and Lang=@language and Id_User = (select Id_User from [User] where [Username] = @username)";
        command.Parameters.AddWithValue("@country", SqlDbType.NVarChar).Value = country;
        command.Parameters.AddWithValue("@language", SqlDbType.NVarChar).Value = language;
        command.Parameters.AddWithValue("@username", SqlDbType.NVarChar).Value = username;
        
        IsUserSigned = command.ExecuteScalar() == null ? false : true;
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Checking Level Roles", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return IsUserSigned;
    }
    public void Add(string username, string language, string country)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Applying Level Roles. Start!");
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection= connection;
        command.CommandText = "insert into [Progress_Card] values (@country, @language, 1,1,1, (select Id_User from [User] where [Username]=@username))";
        command.Parameters.AddWithValue("@country", SqlDbType.NVarChar).Value = country;
        command.Parameters.AddWithValue("@language", SqlDbType.NVarChar).Value = language;
        command.Parameters.AddWithValue("@username", SqlDbType.NVarChar).Value = username;
        command.ExecuteNonQuery();
        Console.WriteLine("p?!");
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Applying Level Roles", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
  }
}
