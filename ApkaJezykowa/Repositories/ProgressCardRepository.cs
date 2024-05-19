using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
using MongoDB.Bson;
using MongoDB.Driver;
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
    IMongoCollection<UserModel> userCollection;
    IMongoCollection<ProgressCardModel> progressCardCollection;
    public ProgressCardRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
      var database = SpeechServiceKey.Instance.Client.GetDatabase("UserBase");
      userCollection = database.GetCollection<UserModel>("User");
      progressCardCollection = database.GetCollection<ProgressCardModel>("Progress_Card");
    }
    public bool IsUserSignedIn(string username, string language, string country)
    {
      bool IsUserSigned;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<UserModel>.Filter.Eq("Username",username);
      var projection = Builders<UserModel>.Projection.Include("_id").Exclude("Username").Exclude("Password").Exclude("Email").Exclude("Country");
      var result = userCollection.Find(filter).Project<UserModel>(projection).First();
      var filterBuilder2 = Builders<ProgressCardModel>.Filter;
      var filter2 = filterBuilder2.Empty;
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Lang_Course",country));
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Lang", language));
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Id_User", result.Id));
      var projection2 = Builders<ProgressCardModel>.Projection.Include("_id").Exclude("Lang_Course").Exclude("Lang").Exclude("Exercise_User_Level").Exclude("Listening_User_Level").Exclude("Text_User_Level").Exclude("Id_User");
      IsUserSigned = progressCardCollection.Find(filter2).Project<ProgressCardModel>(projection2).FirstOrDefault() == null ? false: true;
      //Console.WriteLine("Checking Level Roles. Start!");
      /*using (var connection = GetUserConnection())
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
      }*/
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
      var filter = Builders<UserModel>.Filter.Eq("Username", username);
      var projection = Builders<UserModel>.Projection.Include("_id").Exclude("Username").Exclude("Password").Exclude("Email").Exclude("Country");
      var result = userCollection.Find(filter).Project<UserModel>(projection).First();
      var sort2 = Builders<ProgressCardModel>.Sort.Descending("_id");
      var projection2 = Builders<ProgressCardModel>.Projection.Include("_id").Exclude("Lang_Course").Exclude("Lang").Exclude("Exercise_User_Level").Exclude("Listening_User_Level").Exclude("Text_User_Level").Exclude("Id_User");
      var result2 = progressCardCollection.Find(new BsonDocument()).Sort(sort2).Project<ProgressCardModel>(projection2).FirstOrDefault();
      var progress = new ProgressCardModel
      {
        Id = result2.Id + 1,
        LangCourse = country,
        Lang = language,
        ExerciseUserLevel = 1,
        ListeningUserLevel = 1,
        TextUserLevel = 1,
        IdUser = result.Id
      };
      //Console.WriteLine("Applying Level Roles. Start!");
      /*using (var connection = GetUserConnection())
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
      }*/
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
