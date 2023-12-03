using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;

namespace ApkaJezykowa.Commands
{
    internal class LessonUpdateViewComand : ICommand
    {
    private ILessonRepository lessonRepository;
            private LessonsViewModel viewModel;

            public LessonUpdateViewComand(LessonsViewModel viewModel)
            {
      lessonRepository = new LessonRepository();
                this.viewModel = viewModel;
            }


            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }
    public void Execute(object parameter)
    {
        string Lang = parameter.ToString();
        viewModel.SelectedViewModel = new ModuleMenuViewModel(Lang, lessonRepository.GetIcon(Lang));
        //ExerciseLevelModel.Instance.Language = "French";
    }
    }
}
