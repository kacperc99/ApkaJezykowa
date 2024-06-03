using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
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
    IMongoCollection<TTSPhraseModel> tTSPhraseCollection;
    IMongoCollection<TTSTranslationModel> tTSTranslationCollection;
    IMongoCollection<ChooseRightPhraseModel> chooseRightPhraseCollection;
    IMongoCollection<WrongAnswersListModel> wrongAnswersListCollection;
    IMongoCollection<ListeningListModel> listeningCollection;
    public ListeningRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
      var database = SpeechServiceKey.Instance.Client.GetDatabase("CourseBase");
      tTSPhraseCollection = database.GetCollection<TTSPhraseModel>("TTS_Phrase");
      tTSTranslationCollection = database.GetCollection<TTSTranslationModel>("TTS_Translation");
      chooseRightPhraseCollection = database.GetCollection<ChooseRightPhraseModel>("Choose_Right_Phrase");
      wrongAnswersListCollection = database.GetCollection<WrongAnswersListModel>("Wrong_Answers_List");
      listeningCollection = database.GetCollection<ListeningListModel>("Listening");
    }
    public ObservableCollection<TTS> GetPhrases(int id, string Language)
    {
      ObservableCollection<TTS> phrases; 
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      var rnd = new Random();
      measurement.Start();
      stopwatch.Start();
      //var user = tTSPhraseCollection.Find(filter).Aggregate().Lookup<TTSPhraseModel, TTSTranslationModel, TTS>(tTSTranslationCollection, x => x.Id, y => y.IdTTSPhrase, x => x._phrase).ToList();
      //var result = tTSPhraseCollection.Aggregate([{ $lookup: { } }])
      //Console.WriteLine("Fetching Phrases. Start!");
      //var query = (from p in tTSPhraseCollection.AsQueryable() join t in tTSTranslationCollection.AsQueryable().Where(x => x.IdListening == id) on p.Id equals t.IdTTSPhrase select new { p.Phrase, t.PhraseTranslated }).ToList();
      //var query = tTSPhraseCollection.AsQueryable().Join(tTSTranslationCollection, p=> p.Id, t=>t.IdTTSPhrase, (p, t) => new TTS{_phrase=p.Phrase, _phrase_Translated=t.PhraseTranslated }).Where
      var query = (from p in tTSPhraseCollection.AsQueryable()
                   join t in tTSTranslationCollection on p.Id equals t.IdTTSPhrase
                   where (t.IdListening == id)
                   select new TTS
                   {
                     _phrase = p.Phrase,
                     _phrase_Translated = t.PhraseTranslated
                   }).Sample(8).ToList();
      phrases = new ObservableCollection<TTS>(query);
      var result = phrases.OrderBy(item => rnd.Next()).ToList();
      phrases = new ObservableCollection<TTS>(result);
      //var result = query.Select(x => new { }).ToList();
      /*using (var connection = GetCourseConnection())
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
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Phrases", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return phrases;
    }
    public ObservableCollection<TTS> GetTestPhrases(int id, string Language)
    {
      ObservableCollection<TTS> phrases;
      var rnd = new Random();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<ListeningListModel>.Filter.Eq("Id_Vocabulary", id);
      var projection = Builders<ListeningListModel>.Projection.Expression(item=>item.Id_Listening);
      var result = listeningCollection.Find(filter).Project(projection).ToList();
      var query = (from p in tTSPhraseCollection.AsQueryable()
                   join t in tTSTranslationCollection on p.Id equals t.IdTTSPhrase
                   where (result.Contains(t.IdListening))
                   select new TTS
                   {
                     _phrase = p.Phrase,
                     _phrase_Translated = t.PhraseTranslated
                   }).Sample(8).ToList();
      phrases = new ObservableCollection<TTS>(query);
      var result2 = phrases.OrderBy(item => rnd.Next()).ToList();
      phrases = new ObservableCollection<TTS>(result2);
      //Console.WriteLine("Fetching Test Phrases. Start!");
      /*using (var connection = GetCourseConnection())
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
      stopwatch.Stop();*/
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Test Phrases", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return phrases;
    }
    public void GetAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang)
    {
      //int id_choose;
      var rnd = new Random();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<ChooseRightPhraseModel>.Filter.Eq("Id_Listening", id);
      var result = chooseRightPhraseCollection.Aggregate().Match(filter).AppendStage<ChooseRightPhraseModel>("{ $sample: { size: 2 } }").ToList();
      foreach(var x in result)
      {
        TaskTemplate task = new TaskTemplate();
        task._description = x.SituationDescription;
        task._tts_phrase = x.TTSPhrase;
        task._correct_answer = x.Answer;
        task._answers.Add(x.Answer);
        var filter2 = Builders<WrongAnswersListModel>.Filter.Eq("Id_Choose_Right_Phrase", x.Id);
        var projection = Builders<WrongAnswersListModel>.Projection.Expression(item=>item.WrongAnswer);
        var result2 = wrongAnswersListCollection.Aggregate().Match(filter2).AppendStage<WrongAnswersListModel>($@"{{ $sample: {{ size: {3} }} }}").Project(projection).ToList();
        foreach(var y in result2)
        {
          task._answers.Add(y);
        }
        var result3 = task._answers.OrderBy(item => rnd.Next());
        task._answers = new ObservableCollection<string>(result3);
        data.Add(task);
      }
      //Console.WriteLine("Fetching Questions with Answers. Start!");
      /*using (var connection = GetCourseConnection())
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
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Questions with Answers", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
    public void GetTestAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang)
    {
      //int id_choose;
      var rnd = new Random();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<ListeningListModel>.Filter.Eq("Id_Vocabulary", id);
      var projection = Builders<ListeningListModel>.Projection.Expression(item=>item.Id_Listening);
      var result = listeningCollection.Find(filter).Project(projection).ToList();
      var filter2 = Builders<ChooseRightPhraseModel>.Filter.In("Id_Listening", result);
      var result2 = chooseRightPhraseCollection.Aggregate().Match(filter2).AppendStage<ChooseRightPhraseModel>($@"{{ $sample: {{ size: {2} }} }}").ToList();
      foreach (var x in result2)
      {
        TaskTemplate task = new TaskTemplate();
        task._description = x.SituationDescription;
        task._tts_phrase = x.TTSPhrase;
        task._correct_answer = x.Answer;
        task._answers.Add(x.Answer);
        var filter3 = Builders<WrongAnswersListModel>.Filter.Eq("Id_Choose_Right_Phrase", x.Id);
        var projection2 = Builders<WrongAnswersListModel>.Projection.Expression(item=>item.WrongAnswer);
        var result3 = wrongAnswersListCollection.Aggregate().Match(filter3).AppendStage<WrongAnswersListModel>($@"{{ $sample: {{ size: {3} }} }}").Project(projection2).ToList();
        foreach (var y in result3)
        {
          task._answers.Add(y);
        }
        var result4 = task._answers.OrderBy(item => rnd.Next());
        task._answers = new ObservableCollection<string>(result4);
        data.Add(task);
      }
      //Console.WriteLine("Fetching Test Questions with Answers. Start!");
      /*using (var connection = GetCourseConnection())
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
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Test Questions with Answers", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
  }
}
