using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.ViewModel
{
    public class BaseViewModel :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    public bool _isViewVisibleLogin;
    public bool _isViewVisibleMain;
    public bool IsViewVisibleLogin { get { return _isViewVisibleLogin; } set { value = _isViewVisibleLogin; OnPropertyChanged(nameof(IsViewVisibleLogin)); } }
    public bool IsViewVisibleMain { get { return _isViewVisibleMain; } set { value = _isViewVisibleMain; OnPropertyChanged(nameof(IsViewVisibleMain)); } }
  }
}
