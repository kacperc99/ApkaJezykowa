using ApkaJezykowa.MVVM.Model;
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
    public VocabularyRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
    }
    public void ObtainVocabList(ObservableCollection<VocabularyListModel> VocabularyList, string Country, string Language)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Vocabulary Exercises List. Start!");
      using (var connection = GetCourseConnection())
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
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
    public AccentModel GetAccent(string Lang)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Accent Data. Start!");
      using (var connection = GetCourseConnection())
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
        }
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return accent;
      }
    }
  }
}
