using NUnit.Framework;
using System;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;
using System.Net;
using System.Data.SqlClient;


namespace ApkaJezykowaTest
{
  internal class FrenchLessonViewModelTests : FrenchLessonViewModel
  {

    [SetUp]
    public void Setup()
    {
      
    }

    [Test]
    public void LessonsTestTest()
    {
      ExerciseLevelModel.Instance.Language = "Francuski";
      ExerciseLevelModel.Instance.Level = 1;
      var FrenchLessonViewModel = new FrenchLessonViewModel();

     FrenchLessonViewModel.LoadLesson();
     Assert.AreNotEqual(FrenchLessonViewModel.LessonTitle, null, "Funkcja powinna pozyskać string");
    }
  }
}
