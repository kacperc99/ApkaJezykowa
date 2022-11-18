﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class FrenchViewModel : BaseViewModel
  {

    private BaseViewModel _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));

      }
    }
    public ICommand FrenchUpdateViewCommand { get; set; }
    public FrenchViewModel()
    {
      FrenchUpdateViewCommand = new FrenchUpdateViewCommand(this);
    }
  }
}
