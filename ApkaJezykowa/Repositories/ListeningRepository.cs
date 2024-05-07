using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  public class ListeningRepository : BaseRepository, IListeningRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    public ListeningRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
    }
    public ObservableCollection<TTS> GetPhrases(int id, string Language)
    {
      ObservableCollection<TTS> phrases = new ObservableCollection<TTS>();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Phrases. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        var rnd = new Random();
        command.CommandText = "select top 8 p.Phrase, t.Phrase_Translated from TTS_Phrase p join TTS_Translation t on p.Id_TTS_Phrase=t.Id_TTS_Phrase where Id_Listening = @id";
        command.Parameters.Add("@id",SqlDbType.Int).Value = id;
        using(var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TTS data = new TTS();
            data._phrase = reader["Phrase"].ToString();
            data._phrase_Translated = reader["Phrase_Translated"].ToString();
            phrases.Add(data);
          }
          reader.NextResult();
        }
        var result = phrases.OrderBy(item => rnd.Next()).ToList();
        phrases = new ObservableCollection<TTS>(result);
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return phrases;
    }
    public ObservableCollection<TTS> GetTestPhrases(int id, string Language)
    {
      ObservableCollection<TTS> phrases = new ObservableCollection<TTS>();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Test Phrases. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        var rnd = new Random();
        command.CommandText = "select top 8 p.Phrase, t.Phrase_Translated from TTS_Phrase p join TTS_Translation t on p.Id_TTS_Phrase=t.Id_TTS_Phrase where t.Id_Listening in (select Id_Listening from Listening where Id_Vocabulary = @id)";
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TTS data = new TTS();
            data._phrase = reader["Phrase"].ToString();
            data._phrase_Translated = reader["Phrase_Translated"].ToString();
            phrases.Add(data);
          }
          reader.NextResult();
        }
        var result = phrases.OrderBy(item => rnd.Next()).ToList();
        phrases = new ObservableCollection<TTS>(result);
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
      return phrases;
    }
    public void GetAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang)
    {
      int id_choose;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Questions with Answers. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open(); 
        command.Connection = connection;
        command.CommandText = "select top 2 Id_Choose_Right_Phrase, Situation_Description, TTS_Phrase, Answer from [Choose_Right_Phrase] where Id_Listening = @id_listening";
        command.Parameters.Add("@id_listening",SqlDbType.Int).Value = id;
        var rnd = new Random();
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TaskTemplate task = new TaskTemplate();
            id_choose = (int)reader["Id_Choose_Right_Phrase"];
            task._description = reader["Situation_Description"].ToString();
            task._tts_phrase = reader["TTS_Phrase"].ToString();
            task._correct_answer = reader["Answer"].ToString();
            task._answers.Add(reader["Answer"].ToString());
            using (var command2 = new SqlCommand())
            {
              command2.Connection = connection;
              command2.CommandText = "select top 3 Wrong_Answer from [Wrong_Answers_List] where Id_Choose_Right_Phrase = @id2";
              command2.Parameters.Add("@id2", SqlDbType.Int).Value = id_choose;
              using (var reader2 = command2.ExecuteReader())
              {
                while (reader2.Read())
                {
                  task._answers.Add(reader2[0].ToString());
                }
                reader2.NextResult();
              }
            }
            //task._answers.Shuffle
            var result = task._answers.OrderBy(item=> rnd.Next());
            task._answers = new ObservableCollection<string>(result);
            data.Add(task);
          }
          reader.NextResult();
        }
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
    public void GetTestAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang)
    {
      int id_choose;
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Test Questions with Answers. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 2 Id_Choose_Right_Phrase, Situation_Description, TTS_Phrase, Answer from [Choose_Right_Phrase] where Id_Listening in (select Id_Listening from Listening where Id_Vocabulary = @id)";
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var rnd = new Random();
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TaskTemplate task = new TaskTemplate();
            id_choose = (int)reader["Id_Choose_Right_Phrase"];
            task._description = reader["Situation_Description"].ToString();
            task._tts_phrase = reader["TTS_Phrase"].ToString();
            task._correct_answer = reader["Answer"].ToString();
            task._answers.Add(reader["Answer"].ToString());
            using (var command2 = new SqlCommand())
            {
              command2.Connection = connection;
              command2.CommandText = "select top 3 Wrong_Answer from [Wrong_Answers_List] where Id_Choose_Right_Phrase = @id2";
              command2.Parameters.Add("@id2", SqlDbType.Int).Value = id_choose;
              using (var reader2 = command2.ExecuteReader())
              {
                while (reader2.Read())
                {
                  task._answers.Add(reader2[0].ToString());
                }
                reader2.NextResult();
              }
            }
            //task._answers.Shuffle
            var result = task._answers.OrderBy(item=> rnd.Next());
            task._answers = new ObservableCollection<string>(result);
            data.Add(task);
          }
          reader.NextResult();
        }
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
  }
}
