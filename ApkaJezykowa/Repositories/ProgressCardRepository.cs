using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  internal class ProgressCardRepository : BaseRepository, IProgressCardRepository
  {
    public bool IsUserSignedIn(string username, string language, string country)
    {
      bool IsUserSigned;
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        //command.CommandText = "select uc.Id_Course from [User_Course] uc where uc.Id_User=(select u.Id_User from [User] u where u.[Username]=@username) and uc.Id_Course in (select c.Id_Course from [Course] c where c.Course_Name=@country)";
        command.CommandText = "select Id_Progress_Card from [Progress_Card] where Lang_Course=@country, Lang=@language, Id_User = (select Id_User from [User] where [Username] =@username)";
        command.Parameters.AddWithValue("@country", SqlDbType.NVarChar).Value = country;
        command.Parameters.AddWithValue("@language", SqlDbType.NVarChar).Value = language;
        command.Parameters.AddWithValue("@username", SqlDbType.NVarChar).Value = username;
        
        IsUserSigned = command.ExecuteScalar() == null ? false : true;
      }
      Console.WriteLine("Chyba pykło");
      return IsUserSigned;
    }
    public void Add(string username, string language, string country)
    {
      using (var connection = GetUserConnection())
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
      }
    }
  }
}
