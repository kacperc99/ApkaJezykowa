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
  internal class User_CourseRepository : BaseRepository, IUser_CourseRepository
  {
    public bool IsUserSignedIn(string username, string country)
    {
      bool IsUserSigned;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select uc.Id_Course from [User_Course] uc where uc.Id_User=(select u.Id from [User] u where u.[Username]=@username) and uc.Id_Course in (select c.Id from [Course] c where c.Course_Name=@country)";
        command.Parameters.AddWithValue("@username", SqlDbType.NVarChar).Value = username;
        command.Parameters.AddWithValue("@country", SqlDbType.NVarChar).Value = country;
        IsUserSigned = command.ExecuteScalar() == null ? false : true;
      }
      Console.WriteLine("Chyba pykło");
      return IsUserSigned;
    }
    public void Add(string username, string country)
    {
      int UserID;
      int CourseID;
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id from [User] where [Username]=@username";
        command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
        UserID = (int)command.ExecuteScalar();
        Console.WriteLine("Eins!");
      }
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select Id from [Course] where [Course_Name]=@country and Course_Level=1";
        command.Parameters.Add("@country", SqlDbType.NVarChar).Value = country;
        CourseID = (int)command.ExecuteScalar();
        Console.WriteLine("Zwei!");
      }
      using (var connection = GetConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection= connection;
        command.CommandText = "insert into [User_Course] values (@Id_User, @Id_Course)";
        command.Parameters.Add("@Id_User",SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@Id_Course",SqlDbType.Int).Value = CourseID;
        command.ExecuteNonQuery();
        Console.WriteLine("Drei!");
      }
    }
  }
}
