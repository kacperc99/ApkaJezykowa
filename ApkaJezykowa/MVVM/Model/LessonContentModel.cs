using ApkaJezykowa.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonContentModel
  {
    public int Id {  get; set; }
   public string LessonText { get; set; }
   public List<LessonImageModel> LessonImages { get; set; }
  }
}
