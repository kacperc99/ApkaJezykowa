using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ApkaJezykowa
{
    public class DrawerControl : Control
    {


        public bool IsOpen
        {
            get { return (bool)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(DrawerControl), new PropertyMetadata(0));


        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(DrawerControl), new PropertyMetadata(0));



        static DrawerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawerControl), new FrameworkPropertyMetadata(typeof(DrawerControl)));
        }
    }
}
