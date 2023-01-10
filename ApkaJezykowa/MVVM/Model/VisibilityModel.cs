using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class VisibilityModel
  {
    public bool IsViewVisibleMain { get; set; }
    public bool IsViewVisibleLogin { get; set; }
    public bool IsViewVisible { get; set; }
    private VisibilityModel() { }
    public static readonly VisibilityModel Instance = new VisibilityModel();
  }
  

}
