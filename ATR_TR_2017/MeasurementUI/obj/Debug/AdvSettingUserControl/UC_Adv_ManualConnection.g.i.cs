﻿#pragma checksum "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A5B847CC6E8898FBFFECD7E847443E9ED89804ED"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using DataUtils;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.ExpressionEditor;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Editors.RangeControl;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors.Settings.Extension;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Grid.TreeList;
using MeasurementUI;
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


namespace MeasurementUI {
    
    
    /// <summary>
    /// UC_Adv_ManualConnection
    /// </summary>
    public partial class UC_Adv_ManualConnection : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 94 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExportIMG;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnImportIMG;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDeleteIMG;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img;
        
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
            System.Uri resourceLocater = new System.Uri("/MeasurementUI;component/advsettingusercontrol/uc_adv_manualconnection.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
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
            this.btnExportIMG = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
            this.btnExportIMG.Click += new System.Windows.RoutedEventHandler(this.btnExportIMG_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnImportIMG = ((System.Windows.Controls.Button)(target));
            
            #line 95 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
            this.btnImportIMG.Click += new System.Windows.RoutedEventHandler(this.btnImportIMG_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnDeleteIMG = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
            this.btnDeleteIMG.Click += new System.Windows.RoutedEventHandler(this.btnDeleteIMG_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.img = ((System.Windows.Controls.Image)(target));
            
            #line 100 "..\..\..\AdvSettingUserControl\UC_Adv_ManualConnection.xaml"
            this.img.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.img_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

