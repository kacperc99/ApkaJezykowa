using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApkaJezykowa.MVVM.ViewModel;

namespace ApkaJezykowa.Commands
{
    internal class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
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
            if(parameter.ToString() == "Lessons")
            {
                viewModel.SelectedViewModel = new LessonsViewModel();
            }
            if (parameter.ToString() == "Dictionary")
            {
                viewModel.SelectedViewModel = new DictionaryViewModel();
            }
            if (parameter.ToString() == "Home")
            {
                viewModel.SelectedViewModel = new MainViewModel();
            }
        }
    }
}
