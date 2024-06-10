using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.Keys
{
  public class SpeechServiceKey
  {
    public string Key { get; set; }
    public string Region { get; set; }
    public string UserBaseConnection { get; set; }
    public string CourseBaseConnection { get; set; }
    private SpeechServiceKey() { }
    public static readonly SpeechServiceKey Instance = new SpeechServiceKey();
  }
}
