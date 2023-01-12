using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface IUserRepository
  {
    bool AuthenticateUser(NetworkCredential credential);
    bool FindUser(NetworkCredential credential);
    void Add(string Username, SecureString Password, string Email, string Country);
    void Edit(UserModel userModel);
    void Remove(int id);
    UserModel GetById(int id);
    void GetByUsername(string username);
    IEnumerable<UserModel> GetByAll();
  }
}
