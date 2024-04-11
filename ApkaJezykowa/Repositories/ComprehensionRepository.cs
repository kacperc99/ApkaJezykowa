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

namespace ApkaJezykowa.Repositories
{
  public class ComprehensionRepository : BaseRepository, IComprehensionRepository
  {
    public ReadingTextModel Obtain_Text(int Id_Comprehension)
    {
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "Select * from [Reading_Text] where Id_Reading_Text = (select Id_Reading_Text from [Translated_Text] where Id_Comprehension = @id)";
        command.Parameters.Add("@id",SqlDbType.Int).Value = Id_Comprehension;
        var reader = command.ExecuteReader();
        ReadingTextModel result = new ReadingTextModel();
        result.Id_Reading_Text = (int)reader["Id_Reading_Text"];
        result.Only_Test_Mode = null;
        result.Text_Title = reader["Text_Title"].ToString();
        result.TTS_Text = reader["TTS_Text"].ToString();
        result.Illustration = (byte[])reader["Illustration"];
        reader.Close();
        return result;
      }
    }
    public void Obtain_Dictionary(int Id_Reading_Text, ObservableCollection<TextWordbookModel> TextWordBook)
    {
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
    }
    public string Obtain_Translation(int Id_Comprehension)
    {
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Translation from [Translated_Text] where Id_Comprehension = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Comprehension;
        var reader = command.ExecuteReader();
        string Text_Translated = reader["Translation"].ToString();
        reader.Close();
        return Text_Translated;
      }
    }
    public ObservableCollection<TextQuestionTestModel> Obtain_Questions(int Id_Reading_Text, ObservableCollection<string> correctAnswers)
    {
      ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
      using (var connection = GetCourseConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select top 10 * from [Text_Question] where Id_Reading_Text=@id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = Id_Reading_Text;
        int counter = 1;
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
            var rnd = new Random();
            Answers = Answers.OrderBy(item => rnd.Next()).ToList();
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
        return textQuestions;
      }
    }
  }
}
