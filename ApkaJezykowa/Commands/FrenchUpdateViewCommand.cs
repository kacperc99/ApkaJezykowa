﻿using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class FrenchUpdateViewCommand : ICommand
  {
    private FrenchViewModel viewModel;

    public FrenchUpdateViewCommand(FrenchViewModel viewModel)
    {
      this.viewModel = viewModel;
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "FrenchLesson")
      {
        //viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "FrenchReturn")
      {
        viewModel.SelectedViewModel = new LessonsViewModel();
      }
    }
  }
}

