using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  internal class UserRepository : BaseRepository, IUserRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    public UserRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
    }
    public bool FindUser(NetworkCredential credential)
    {
      bool newUser;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Searching for the User. Start!");
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection=connection;
        command.CommandText = "select *from [User] where [Username]=@username or [Email]=@email";
        command.Parameters.Add("@username",SqlDbType.NVarChar).Value = credential.UserName;
        command.Parameters.Add("@email",SqlDbType.NVarChar).Value=credential.Password;
        newUser = command.ExecuteScalar() == null ? false : true;
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Searching for the user", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return newUser;
    }
    public void Add(string Username, SecureString Password, string Email, string Country)
    {
      string Passwort = new NetworkCredential("",Password).Password;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Adding new User. Start!");
      using (var connection = GetUserConnection())
      {
        connection.Open();
        string sql = "insert into [User] values (@username, @password, @email, @country)";
        using(SqlCommand cmd = new SqlCommand(sql,connection))
        {
          cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = Username;
          cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = Passwort;
          cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = Email;
          cmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
          cmd.CommandType= CommandType.Text;
          cmd.ExecuteNonQuery();
          /*well, maybe before I forget, I will shortly describe what's the thing with user perms, 
           * since they're gone from this part of the database.
           basically I've turned it into a separate table, which contains all the permission details, including
          type of permission (editing, editing + approving, chat moderating, administrators and stuff
          new system is going to allow for much more precise permission assigning*/
        }
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Adding new user", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }

    public bool AuthenticateUser(NetworkCredential credential)
    {
      bool validUser;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Authenticating User. Start!");
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select *from [User] where [Username]=@username and [Password]=@password";
        command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
        validUser = command.ExecuteScalar() == null ? false : true;
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Authenticating User", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return validUser;
    }

    public void Edit(UserModel userModel)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<UserModel> GetByAll()
    {
      throw new NotImplementedException();
    }

    public UserModel GetById(int id)
    {
      throw new NotImplementedException();
    }

    public UserModel GetByUsername(string username)
    {
      UserModel user = null;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching User Data. Start!");
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select *from [User] where [Username]=@username";
        command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
        using (var reader = command.ExecuteReader())
        {
          if(reader.Read())
          {
            user = new UserModel()
            {
              Id = reader[0].ToString(),
              Username = reader[1].ToString(),
              Password = String.Empty,
              Email = reader[3].ToString(),
              Country = reader[4].ToString()
            };
          }
        }
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching User Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return user;
      }
    }

    public void Remove(int id)
    {
      throw new NotImplementedException();
    }
  }
}
