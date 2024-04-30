using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class AccentModel
  {
    public int Id_Accent { get; set; }
    public string Accent { get; set; }
    public string Lang { get; set; }
    public string Voice { get; set; }
    public string Id_Course { get; set; }
  }
}
