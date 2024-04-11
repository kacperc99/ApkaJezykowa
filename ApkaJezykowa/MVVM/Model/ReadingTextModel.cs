using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ReadingTextModel
  {
    public int Id_Reading_Text { get; set; }
    public bool? Only_Test_Mode { get; set; }
    public string Text_Title { get; set; }
    public string TTS_Text { get; set; }
    public byte[] Illustration { get; set; }
  }
}
