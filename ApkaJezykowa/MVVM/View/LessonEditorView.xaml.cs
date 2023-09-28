using ApkaJezykowa.MVVM.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApkaJezykowa.MVVM.View
{
  /// <summary>
  /// Logika interakcji dla klasy LessonEditorView.xaml
  /// </summary>
  public partial class LessonEditorView : UserControl
  {
    public LessonEditorView()
    {
      InitializeComponent();
    }
    private void ChooseImage_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog
      {
        InitialDirectory = @"D:\",
        Title = "Wybierz obraz",

        CheckFileExists = true,
        CheckPathExists = true,
        DefaultExt = "png",
        Filter = "Pliki obrazów (*.jpg;*.png;)|*.jpg;*.png",
        FilterIndex = 2,
        RestoreDirectory = true,

        ReadOnlyChecked = true,
        ShowReadOnly = true,
      };

      if (openFileDialog1.ShowDialog() == true)
      {
        BitmapImage img = new BitmapImage(new Uri(openFileDialog1.FileName));
        MemoryStream memStream = new MemoryStream();
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(img));
        encoder.Save(memStream);
        byte[] bytes = memStream.ToArray();
        (this.DataContext as LessonEditorViewModel)?.SetPath(bytes);
      }
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (Content.IsFocused && Content.Text == "Write lesson content here")
        Content.Clear();
    }
  }
}
