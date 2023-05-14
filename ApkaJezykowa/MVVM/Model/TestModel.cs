using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class TestModel
  {
    public bool TestMode { get; set; }
    public List<int> TestId { get; set; }
    public List<string> TestTasks { get; set; }
    public List<bool> Test_Done { get; set; }
    public int Test_Points { get; set; }
    public TestModel() { }
    public static readonly TestModel instance = new TestModel();
  }
}
