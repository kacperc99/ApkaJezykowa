using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class UserModel
  {
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public UserModel() { }
    public static readonly UserModel Instance = new UserModel();

  }
}
