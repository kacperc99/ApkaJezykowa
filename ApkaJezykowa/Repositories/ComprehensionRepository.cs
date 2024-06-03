using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using MongoDB.Driver;
using ApkaJezykowa.Keys;
using System.Runtime.InteropServices;

namespace ApkaJezykowa.Repositories
{
  public class ComprehensionRepository : BaseRepository, IComprehensionRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    IMongoCollection<ReadingListModel> comprehensionCollection;
    IMongoCollection<TranslatedTextModel> translatedTextCollection;
    IMongoCollection<ReadingTextModel> readingTextCollection;
    IMongoCollection<TextWordbookModel> textWordbookCollection;
    IMongoCollection<TextQuestionModel> textQuestionCollection;
    public ComprehensionRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
      var database = SpeechServiceKey.Instance.Client.GetDatabase("CourseBase");
      comprehensionCollection = database.GetCollection<ReadingListModel>("Comprehension");
      translatedTextCollection = database.GetCollection<TranslatedTextModel>("Translated_Text");
      readingTextCollection = database.GetCollection<ReadingTextModel>("Reading_Text");
      textWordbookCollection = database.GetCollection<TextWordbookModel>("Text_Wordbook");
      textQuestionCollection = database.GetCollection<TextQuestionModel>("Text_Question");
    }
    public ReadingTextModel Obtain_Text(int Id_Comprehension)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<TranslatedTextModel>.Filter.Eq("Id_Comprehension", Id_Comprehension);
      var projection = Builders<TranslatedTextModel>.Projection.Expression(item=>item.Id_Reading_Text);
      var result = translatedTextCollection.Find(filter).Project(projection).ToList();
      var filterBuilder2 = Builders<ReadingTextModel>.Filter;
      var filter2 = filterBuilder2.Empty;
      filter2 = filterBuilder2.And(filter2, filterBuilder2.Eq("Only_Test_Mode", false));
      filter2 = filterBuilder2.And(filter2, filterBuilder2.In("Id_Reading_Text", result));
      var result2 = readingTextCollection.Aggregate().Match(filter2).AppendStage<ReadingTextModel>($@"{{ $sample: {{ size: {1} }} }}").FirstOrDefault();
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Random Text Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return result2;
      //Console.WriteLine("Fetching Random Text Data. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select top 1 * from [Reading_Text] where Only_Test_Mode = 0 and Id_Reading_Text in (select Id_Reading_Text from [Translated_Text] where Id_Comprehension = @id)";
        command.Parameters.Add("@id",SqlDbType.Int).Value = Id_Comprehension;
        var reader = command.ExecuteReader();
        ReadingTextModel result = new ReadingTextModel();
        if (reader.Read())
        {
          result.Id_Reading_Text = (int)reader[0];
          result.Only_Test_Mode = null;
          result.Text_Title = reader[2].ToString();
          result.TTS_Text = reader[3].ToString();
          result.Illustration = (byte[])reader[4];
        }
        reader.Close();
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Random Text Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return result;
      }*/
    }
    /*public int Get_Comprehension_Int(int Id_Vocabulary)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      //Console.WriteLine("Fetching Random Comprehension Id. Start!");
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 1 Id_Comprehension from Comprehension where Id_Vocabulary = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Vocabulary;
        var reader = command.ExecuteReader();
        int id = command.ExecuteNonQuery();
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Random Comprehension Id", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return id;
      }
    }*/
    public ReadingTextModel Obtain_Test_Text(int Id_Vocabulary)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<ReadingListModel>.Filter.Eq("Id_Vocabulary", Id_Vocabulary);
      var projection = Builders<ReadingListModel>.Projection.Expression(item=>item.Id_Reading);
      var result = comprehensionCollection.Find(filter).Project(projection).ToList();
      var filter2 = Builders<TranslatedTextModel>.Filter.In("Id_Comprehension",result);
      var projection2 = Builders<TranslatedTextModel>.Projection.Expression(item => item.Id_Translation);
      var result2 = translatedTextCollection.Find(filter2).Project(projection2).ToList();
      var filterBuilder3 = Builders<ReadingTextModel>.Filter;
      var filter3 = filterBuilder3.Empty;
      filter3 = filterBuilder3.And(filter3, filterBuilder3.Eq("Only_Test_Mode", true));
      filter3 = filterBuilder3.And(filter3, filterBuilder3.In("Id_Reading_Text", result2));
      var result3 = readingTextCollection.Aggregate().Match(filter3).AppendStage<ReadingTextModel>($@"{{ $sample: {{ size: {1} }} }}").FirstOrDefault();
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Random Test Text Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return result3;
      //Console.WriteLine("Fetching Random Test Text Data. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select top 1 * from [Reading_Text] where Only_Test_Mode = 1 and Id_Reading_Text in (select Id_Reading_Text from [Translated_Text] where Id_Comprehension in (select Id_Comprehension from Comprehension where Id_Vocabulary = @id))";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Vocabulary;
        var reader = command.ExecuteReader();
        ReadingTextModel result = new ReadingTextModel();
        if (reader.Read())
        {
          result.Id_Reading_Text = (int)reader[0];
          result.Only_Test_Mode = null;
          result.Text_Title = reader[2].ToString();
          result.TTS_Text = reader[3].ToString();
          result.Illustration = (byte[])reader[4];
        }
        reader.Close();
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Random Test Text Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return result;
      }*/
    }
    public void Obtain_Dictionary(int Id_Reading_Text, ObservableCollection<TextWordbookModel> TextWordBook)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<TextWordbookModel>.Filter.Eq("Id_Reading_Text", Id_Reading_Text);
      var result = textWordbookCollection.Find(filter).ToList();
      TextWordBook = new ObservableCollection<TextWordbookModel>(result);
      //Console.WriteLine("Fetching Dictionary for the Text. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select * from [Text_Wordbook] where Id_Reading_Text = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Reading_Text;
        using(var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TextWordbookModel result = new TextWordbookModel();
            result.Id_Text_Wordbook = null;
            result.Word = reader["Word"].ToString();
            result.Translated_Word = reader["Translated_Word"].ToString();
            result.Id_Reading_Text = null;
            result.Id_Comprehension = null;
            TextWordBook.Add(result);
          }
          reader.NextResult();
        }
      }*/
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Dictionary for the text", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
    }
    public string Obtain_Translation(int Id_Comprehension)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<TranslatedTextModel>.Filter.Eq("Id_Comprehension", Id_Comprehension);
      var projection = Builders<TranslatedTextModel>.Projection.Expression(item => item.Translation);
      var result = translatedTextCollection.Find(filter).Project(projection).FirstOrDefault();
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
        ("Fetching Translation Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
        stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
      MeasurementModel.Instance.CPU_Vals.Clear();
      MeasurementModel.Instance.RAM_Vals.Clear();
      return result;
      //Console.WriteLine("Fetching Translation Data. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Translation from [Translated_Text] where Id_Comprehension = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Comprehension;
        var reader = command.ExecuteReader();
        string Text_Translated = null;
        if (reader.Read())
          Text_Translated = reader["Translation"].ToString();
        reader.Close();
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Translation Data", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return Text_Translated;
      }*/
    }
    public ObservableCollection<TextQuestionTestModel> Obtain_Questions(int Id_Reading_Text, ObservableCollection<string> correctAnswers)
    {
      int counter = 0;
      var rnd = new Random();
      ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      var filter = Builders<TextQuestionModel>.Filter.Eq("Id_Reading_Text",Id_Reading_Text);
      var result = textQuestionCollection.Aggregate().AppendStage<TextQuestionModel>($@"{{ $sample: {{ size: {10} }} }}").Match(filter).ToList();
      foreach(var x in result)
      {
        TextQuestionTestModel model = new TextQuestionTestModel();
        model.Id_Text_Question = x.Id_Text_Question;
        model.Question = x.Question;
        correctAnswers.Add(x.Correct_Answer);
        List<string> Answers = new List<string>
            {
              x.Correct_Answer,
              x.Wrong_Answer,
              x.Wrong_Answer_2,
              x.Wrong_Answer_3
            };
        var result2 = Answers.OrderBy(item => rnd.Next()).ToList();
        Answers = new List<string>(result2);
        model.Answer1 = Answers[0];
        model.Answer2 = Answers[1];
        model.Answer3 = Answers[2];
        model.Answer4 = Answers[3];
        model.Answer_Tip = x.Answer_Tip;
        model.Id_Reading_Text = x.Id_Reading_Text;
        model.GroupName = counter.ToString();
        textQuestions.Add(model);
        counter++;
      }
      //Console.WriteLine("Fetching Random Questions. Start!");
      /*using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 10 * from [Text_Question] where Id_Reading_Text=@id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Reading_Text;
        int counter = 0;
        var rnd = new Random();
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TextQuestionTestModel model = new TextQuestionTestModel();
            model.Id_Text_Question = (int)reader["Id_Text_Question"];
            model.Question = reader["Question"].ToString();
            correctAnswers.Add(reader["Correct_Answer"].ToString());
            List<string> Answers = new List<string>
            {
              reader["Correct_Answer"].ToString(),
              reader["Wrong_Answer"].ToString(),
              reader["Wrong_Answer_2"].ToString(),
              reader["Wrong_Answer_3"].ToString()
            };
            var result = Answers.OrderBy(item => rnd.Next()).ToList();
            Answers = new List<string>(result);
            model.Answer1 = Answers[0];
            model.Answer2 = Answers[1];
            model.Answer3 = Answers[2];
            model.Answer4 = Answers[3];
            model.Answer_Tip = reader["Answer_Tip"].ToString();
            model.Id_Reading_Text = (int)reader["Id_Reading_Text"];
            model.GroupName = counter.ToString();
            textQuestions.Add(model);
            counter++;
          }
          reader.NextResult();
        }*/
      stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        MeasurementModel.Instance.Measurement_Results.Add(new Tuple<string, List<double>, List<float>, TimeSpan, double, float>
          ("Fetching Random Questions", new List<double>(MeasurementModel.Instance.CPU_Vals), new List<float>(MeasurementModel.Instance.RAM_Vals),
          stopwatch.Elapsed, MeasurementModel.Instance.CPU_Vals.Count > 0 ? MeasurementModel.Instance.CPU_Vals.Average() : 0.0, MeasurementModel.Instance.RAM_Vals.Count > 0 ? MeasurementModel.Instance.RAM_Vals.Average() : 0));
        MeasurementModel.Instance.CPU_Vals.Clear();
        MeasurementModel.Instance.RAM_Vals.Clear();
        return textQuestions;
    }
  }
}
