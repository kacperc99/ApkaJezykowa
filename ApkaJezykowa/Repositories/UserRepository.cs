﻿using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  internal class UserRepository : BaseRepository, IUserRepository
  {
    public bool FindUser(NetworkCredential credential)
    {
      bool newUser;
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection=connection;
        command.CommandText = "select *from [User] where [Username]=@username or [Email]=@email";
        command.Parameters.Add("@username",SqlDbType.NVarChar).Value = credential.UserName;
        command.Parameters.Add("@email",SqlDbType.NVarChar).Value=credential.Password;
        newUser = command.ExecuteScalar() == null ? false : true;
      }
      return newUser;
    }
    public void Add(string Username, SecureString Password, string Email, string Country)
    {
      string Passwort = new NetworkCredential("",Password).Password;
      using(var connection = GetUserConnection())
      {
        connection.Open();
        string sql = "insert into [User] values (@username, @password, @email, @country)";
        using(SqlCommand cmd = new SqlCommand(sql,connection))
        {
          cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = Username;
          cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = Passwort;
          cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = Email;
          cmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = Country;
          cmd.CommandType= CommandType.Text;
          cmd.ExecuteNonQuery();
          /*well, maybe before I forget, I will shortly describe what's the thing with user perms, 
           * since they're gone from this part of the database.
           basically I've turned it into a separate table, which contains all the permission details, including
          type of permission (editing, editing + approving, chat moderating, administrators and stuff
          new system is going to allow for much more precise permission assigning*/
        }
      }
    }

    public bool AuthenticateUser(NetworkCredential credential)
    {
      bool validUser;
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select *from [User] where [Username]=@username and [Password]=@password";
        command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
        validUser = command.ExecuteScalar() == null ? false : true;
      }
      return validUser;
    }

    public void Edit(UserModel userModel)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<UserModel> GetByAll()
    {
      throw new NotImplementedException();
    }

    public UserModel GetById(int id)
    {
      throw new NotImplementedException();
    }

    public UserModel GetByUsername(string username)
    {
      UserModel user = null;
      using (var connection = GetUserConnection())
      using (var command = new SqlCommand())
      {
        connection.Open();
        command.Connection = connection;
        command.CommandText = "select *from [User] where [Username]=@username";
        command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
        using (var reader = command.ExecuteReader())
        {
          if(reader.Read())
          {
            user = new UserModel()
            {
              Id = reader[0].ToString(),
              Username = reader[1].ToString(),
              Password = String.Empty,
              Email = reader[3].ToString(),
              Country = reader[4].ToString()
            };
          }
        }
        return user;
      }
    }

    public void Remove(int id)
    {
      throw new NotImplementedException();
    }
  }
}
