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

namespace ApkaJezykowa.Repositories
{
  public class ComprehensionRepository : BaseRepository, IComprehensionRepository
  {
    private IPerformanceMeasurementRepository performanceMeasurementRepository;
    Thread measurement;
    Stopwatch stopwatch;
    public ComprehensionRepository()
    {
      performanceMeasurementRepository = new PerformanceMeasurementRepository();
    }
    public ReadingTextModel Obtain_Text(int Id_Comprehension)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Random Text Data. Start!");
      using (var connection = GetCourseConnection())
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
        Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return result;
      }
    }
    public int Get_Comprehension_Int(int Id_Vocabulary)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Random Comprehension Id. Start!");
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
        Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return id;
      }
    }
    public ReadingTextModel Obtain_Test_Text(int Id_Vocabulary)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Random Test Text Data. Start!");
      using (var connection = GetCourseConnection())
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
        Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return result;
      }
    }
    public void Obtain_Dictionary(int Id_Reading_Text, ObservableCollection<TextWordbookModel> TextWordBook)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Dictionary for the Text. Start!");
      using (var connection = GetCourseConnection())
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
      }
      stopwatch.Stop();
      Properties.Settings.Default.ThreadManager = false;
      Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
    }
    public string Obtain_Translation(int Id_Comprehension)
    {
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Translation Data. Start!");
      using (var connection = GetCourseConnection())
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
        Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return Text_Translated;
      }
    }
    public ObservableCollection<TextQuestionTestModel> Obtain_Questions(int Id_Reading_Text, ObservableCollection<string> correctAnswers)
    {
      ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
      measurement = new Thread(new ThreadStart(performanceMeasurementRepository.CPU_Measurement));
      stopwatch = new Stopwatch();
      Properties.Settings.Default.ThreadManager = true;
      measurement.Start();
      stopwatch.Start();
      Console.WriteLine("Fetching Random Questions. Start!");
      using (var connection = GetCourseConnection())
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
        }
        stopwatch.Stop();
        Properties.Settings.Default.ThreadManager = false;
        Console.WriteLine("Stop! Czas wykonania: " + stopwatch.Elapsed.ToString());
        return textQuestions;
      }
    }
  }
}
