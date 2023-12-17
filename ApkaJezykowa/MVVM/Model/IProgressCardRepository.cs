using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal interface IProgressCardRepository
  {
    bool IsUserSignedIn(string username, string language, string country);
    void Add(string username, string language, string country);
  }
}
