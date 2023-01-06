using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface IUser_CourseRepository
  {
    bool IsUserSignedIn(string username, string country);
    void Add(string username, string country);
  }
}
