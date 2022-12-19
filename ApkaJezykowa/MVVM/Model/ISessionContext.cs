using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public interface ISessionContext: INotifyPropertyChanged
  {
    bool IsViewVisible { get; set; }
  }
}
