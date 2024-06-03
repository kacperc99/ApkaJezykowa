using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
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
using System.Windows.Input;

namespace ApkaJezykowa.Repositories
{
  public class VocabularyRepository : BaseRepository, IVocabularyRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    IMongoCollection<VocabularyModel> vocabularyCollection;
    IMongoCollection<CourseModel> courseCollection;
    IMongoCollection<ListeningListModel> listeningCollection;
    IMongoCollection<ReadingListModel> comprehensionCollection;
    IMongoCollection<AccentModel> accentCollection;
    public VocabularyRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
      var database = SpeechServiceKey.Instance.Client.GetDatabase("CourseBase");
      vocabularyCollection = database.GetCollection<VocabularyModel>("Vocabulary");
      courseCollection = database.GetCollection<CourseModel>("Course");
      comprehensionCollection = database.GetCollection<ReadingListModel>("Comprehension");
      listeningCollection = database.GetCollection<ListeningListModel>("Listening");
      accentCollection = database.GetCollection<AccentModel>("Accent");
    }
    public void ObtainVocabList(ObservableCollection<VocabularyListModel> VocabularyList, string Country, string Language)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<CourseModel>.Filter.Eq("Course_Name", Country);
      var projection = Builders<CourseModel>.Projection.Include("_id").Exclude("Course_Name").Exclude("Image");
      var result = courseCollection.Find(filter).Project<CourseModel>(projection).FirstOrDefault();
      var filter2 = Builders<VocabularyModel>.Filter.Eq("Id_Course", result.Id);
      var result2 = vocabularyCollection.Find(filter2).ToList();
      foreach (var item in result2)
      {
        VocabularyListModel model= new VocabularyListModel();
        model.Id_Vocabulary = item.Id;
        model.Vocabulary_Level = item.VocabularyLevel;
        model.Id_Course = null;
        var filter3 = Builders<ListeningListModel>.Filter.Eq("Id_Vocabulary", item.Id);
        var result3 = listeningCollection.Find(filter3).ToList();
        model.ListeningList = new ObservableCollection<ListeningListModel>(result3);
        var filter4 = Builders<ReadingListModel>.Filter.Eq("Id_Vocabulary", item.Id);
        var result4 = comprehensionCollection.Find(filter4).ToList();
        model.ReadingList = new ObservableCollection<ReadingListModel>(result4);
        VocabularyList.Add(model);
      }
      //Console.WriteLine("Fetching Vocabulary Exercises List. Start!");
      /*using (var connection = GetCourseConnection())
      using(var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select * from Vocabulary where Id_Course = (select Id_Course from [Course] where Course_Name = @country)";
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
        using(var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            VocabularyListModel VocabModel = new VocabularyListModel();
            VocabModel.Id_Vocabulary = (int)reader["Id_Vocabulary"];
            VocabModel.Vocabulary_Level = (decimal)reader["Vocabulary_Level"];
            //VocabModel.Vocabulary_Parameter = reader["Vocabulary_Parameter"].ToString();
            VocabModel.Id_Course = null;
            ObservableCollection<ListeningListModel> listeningListModels = new ObservableCollection<ListeningListModel>();
            using(var command_3 = new SqlCommand())
            {
              command_3.Connection = connection;
              command_3.CommandText = "select * from [Listening] where Id_Vocabulary = @id";
              command_3.Parameters.Add("@id",SqlDbType.Int).Value = VocabModel.Id_Vocabulary;
              using(var reader3 = command_3.ExecuteReader())
              {
                while (reader3.Read())
                {
                  ListeningListModel ListeningModel = new ListeningListModel();
                  ListeningModel.Id_Listening = (int)reader3[0];
                  ListeningModel.Listening_Language = null;
                  ListeningModel.Listening_Title = reader3[2].ToString();
                  ListeningModel.Id_Vocabulary = null;
                  listeningListModels.Add(ListeningModel);
                }
                reader3.NextResult();
              }
            }
            VocabModel.ListeningList = listeningListModels;
            ObservableCollection<ReadingListModel> readingListModels = new ObservableCollection<ReadingListModel>();
            using (var command_2 = new SqlCommand())
            {
              command_2.Connection = connection;
              command_2.CommandText = "select * from [Comprehension] where Id_Vocabulary = @id";
              command_2.Parameters.Add("@id", SqlDbType.Int).Value = VocabModel.Id_Vocabulary;
              using (var reader2 = command_2.ExecuteReader())
              {
                while (reader2.Read())
                {
                  ReadingListModel ReadingModel = new ReadingListModel();
                  ReadingModel.Id_Reading = (int)reader2[0];
                  ReadingModel.Reading_Language = null;
                  ReadingModel.Reading_Title = reader2[2].ToString();
                  ReadingModel.Id_Vocabulary = null;
                  readingListModels.Add(ReadingModel);
                }
                reader2.NextResult();
              }
            }
            VocabModel.ReadingList = readingListModels;
            VocabularyList.Add(VocabModel);
          }
          reader.NextResult();
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Vocabulary Exercises List", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
    public AccentModel GetAccent(string Lang)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<CourseModel>.Filter.Eq("Course_Name", Lang);
      var projection = Builders<CourseModel>.Projection.Expression(item=>item.Id);//Include("_id");
      var result = courseCollection.Find(filter).Project(projection).FirstOrDefault();
      var filter2 = Builders<AccentModel>.Filter.Eq("Id_Course", result);
      var result2 = accentCollection.Find(filter2).FirstOrDefault();
      
      //Console.WriteLine("Fetching Accent Data. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Accent, Lang, Voice from [Accent] where Id_Course = (select Id_Course from [Course] where Course_Name = @country)";
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = Lang;
        AccentModel accent = new AccentModel();
        using (var reader = command.ExecuteReader()) 
        {
          if(reader.Read())
          {
            accent.Accent = reader[0].ToString();
            accent.Lang = reader[1].ToString();
            accent.Voice = reader[2].ToString();
          }
        }*/
      stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Accent Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return result2;
      //}
    }
  }
}
