﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class SettingsViewModel : BaseViewModel
  {
    string _filename;

    public string Filename { get { return _filename; } set { _filename = value; OnPropertyChanged(nameof(Filename)); } }
  }
}
