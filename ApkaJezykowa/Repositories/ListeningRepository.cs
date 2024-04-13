using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  public class ListeningRepository : BaseRepository, IListeningRepository
  {
    public void GetPhrases(ObservableCollection<TTS> phrases, int id, string Language)
    {
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
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
      }
    }
    public void GetTestPhrases(ObservableCollection<TTS> phrases, int id, string Language)
    {
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
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
      }
    }
    public void GetAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang)
    {
      int id_choose;
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open(); 
        command.Connection = connection;
        command.CommandText = "select top 2 Id_Choose_Right_Phrase, Situation_Description, TTS_Phrase, Answer from [Choose_Right_Phrase] where Id_Listening = @id_listening";
        command.Parameters.Add("@id_listening",SqlDbType.Int).Value = id;
        using(var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            TaskTemplate task = new TaskTemplate();
            id_choose = (int)reader["Id_Choose_Right_Phrase"];
            task._description = reader["Situation_Description"].ToString();
            task._tts_phrase = reader["TTS_Phrase"].ToString();
            task._correct_answer = reader["Answer"].ToString();
            task._answers.Add(reader["Answer"].ToString());
            command.CommandText = "select top 3 Wrong_Answer from [Wrong_Answers_List] where Id_Choose_Right_Phrase = @id2";
            command.Parameters.Add("@id2", SqlDbType.Int).Value = id_choose;
            using(var reader2 = command.ExecuteReader())
            {
              while(reader2.Read())
              {
                task._answers.Add(reader["Wrong_Answer"].ToString());
              }
              reader2.NextResult();
            }
            //task._answers.Shuffle
            var rnd = new Random();
            var result = task._answers.OrderBy(item=> rnd.Next());
            task._answers = (ObservableCollection<string>)result;
            data.Add(task);
          }
          reader.NextResult();
        }
      }
    }
    public void GetTestAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang)
    {
      int id_choose;
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 2 Id_Choose_Right_Phrase, Situation_Description, TTS_Phrase, Answer from [Choose_Right_Phrase] where Id_Listening in (select Id_Listening from Listening where Id_Vocabulary = @id)";
        command.Parameters.Add("@id_listening", SqlDbType.Int).Value = id;
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
            command.CommandText = "select top 3 Wrong_Answer from [Wrong_Answers_List] where Id_Choose_Right_Phrase = @id2";
            command.Parameters.Add("@id2", SqlDbType.Int).Value = id_choose;
            using (var reader2 = command.ExecuteReader())
            {
              while (reader2.Read())
              {
                task._answers.Add(reader["Wrong_Answer"].ToString());
              }
              reader2.NextResult();
            }
            //task._answers.Shuffle
            var rnd = new Random();
            var result = task._answers.OrderBy(item => rnd.Next());
            task._answers = (ObservableCollection<string>)result;
            data.Add(task);
          }
          reader.NextResult();
        }
      }
    }
  }
}
