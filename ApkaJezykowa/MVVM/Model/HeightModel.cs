using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class HeightModel
  {
    public double Height { get; set; }
    public double Width { get; set; }
    private HeightModel() { }
    public static readonly HeightModel Instance = new HeightModel();
  }
}
