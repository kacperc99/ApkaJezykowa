﻿#pragma checksum "..\..\..\..\MVVM\View\LessonView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "11FAF347B6BD426E5C12DEE70A2A3A4E747C26E94A47C71B73070D5C0EF264E8"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using ApkaJezykowa.MVVM.View;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ApkaJezykowa.MVVM.View {
    
    
    /// <summary>
    /// LessonView
    /// </summary>
    public partial class LessonView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer SV;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView LV2;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid nav_pnl;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel st_pnl;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton Tg_btn;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.Storyboard HideStackPanel;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.Storyboard ShowStackPanel;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\MVVM\View\LessonView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView LV;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ApkaJezykowa;component/mvvm/view/lessonview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MVVM\View\LessonView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SV = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 16 "..\..\..\..\MVVM\View\LessonView.xaml"
            this.SV.SizeChanged += new System.Windows.SizeChangedEventHandler(this.SV_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LV2 = ((System.Windows.Controls.ListView)(target));
            return;
            case 3:
            this.nav_pnl = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.st_pnl = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.Tg_btn = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 6:
            this.HideStackPanel = ((System.Windows.Media.Animation.Storyboard)(target));
            return;
            case 7:
            this.ShowStackPanel = ((System.Windows.Media.Animation.Storyboard)(target));
            return;
            case 8:
            this.LV = ((System.Windows.Controls.ListView)(target));
            
            #line 106 "..\..\..\..\MVVM\View\LessonView.xaml"
            this.LV.SizeChanged += new System.Windows.SizeChangedEventHandler(this.LV_SizeChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

