using ApkaJezykowa.MVVM.Model;
using NUnit.Framework;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowaTest
{
  internal class FrenchExerciseViewModelTests
  {
    [SetUp]
    public void SetUp()
    {

    }
    [Test]
    public void LoadDataTest()
    {
      ExerciseLevelModel.Instance.Level = 1;
      ExerciseLevelModel.Instance.Language = "Francuski";
      var FrenchExerciseViewModel = new FrenchExerciseViewModel();
      Assert.IsNotNull(FrenchExerciseViewModel.Test, "nie powinno być nullem");
      Assert.IsNotNull(FrenchExerciseViewModel.Exercise, "nie powinno być nullem");
      FrenchExerciseViewModel.CheckAnswers.Execute(null);
    }
  }
}
