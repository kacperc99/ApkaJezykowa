using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.Keys
{
  public class SpeechServiceKey
  {
    //public string Key = "9e828889c417490791c07ae4a30ab961";
    //public string Region = "germanywestcentral";
    public string Key { get; set; }
    public string Region {  get; set; }
    private SpeechServiceKey() { }
    public static readonly SpeechServiceKey Instance = new SpeechServiceKey();
  }
}
