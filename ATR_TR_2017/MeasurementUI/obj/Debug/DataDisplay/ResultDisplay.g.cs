﻿#pragma checksum "..\..\..\DataDisplay\ResultDisplay.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3664FBFD6374ADD6132DA0CDB9A905E28ADC0B58"
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
using DevExpress.Core;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.DemoBase;
using DevExpress.Xpf.DemoBase.DataClasses;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.Docking;
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
using DevExpress.Xpf.Editors.Themes;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Grid.TreeList;
using MeasurementUI;
using SymtChartLib;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
using ViewModelBaseLib;


namespace MeasurementUI {
    
    
    /// <summary>
    /// ResultDisplay
    /// </summary>
    public partial class ResultDisplay : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 181 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid;
        
        #line default
        #line hidden
        
        
        #line 183 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DockLayoutManager dockLayoutManager1;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentGroup documentGroup2;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel inputDocumentPanel;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid inputGrid;
        
        #line default
        #line hidden
        
        
        #line 201 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel remarksPanel;
        
        #line default
        #line hidden
        
        
        #line 206 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel flowDocumentPanel;
        
        #line default
        #line hidden
        
        
        #line 209 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.TabControl tabcontrol;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentGroup documentGroup1;
        
        #line default
        #line hidden
        
        
        #line 219 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel documentPanle2;
        
        #line default
        #line hidden
        
        
        #line 220 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl loopGridControl;
        
        #line default
        #line hidden
        
        
        #line 226 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel dpChart2;
        
        #line default
        #line hidden
        
        
        #line 228 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SymtChartLib.WindowChart InterfaceChart2;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentGroup documentGroup3;
        
        #line default
        #line hidden
        
        
        #line 245 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel passFailDocumentPanel;
        
        #line default
        #line hidden
        
        
        #line 246 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid passFailGrid;
        
        #line default
        #line hidden
        
        
        #line 247 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox listBoxStep;
        
        #line default
        #line hidden
        
        
        #line 272 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentGroup documentGroup4;
        
        #line default
        #line hidden
        
        
        #line 273 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel documentPanel1;
        
        #line default
        #line hidden
        
        
        #line 282 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl contentControl;
        
        #line default
        #line hidden
        
        
        #line 285 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Start;
        
        #line default
        #line hidden
        
        
        #line 290 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel documentPanel2;
        
        #line default
        #line hidden
        
        
        #line 296 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox listBoxStep2;
        
        #line default
        #line hidden
        
        
        #line 318 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPause;
        
        #line default
        #line hidden
        
        
        #line 321 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStop;
        
        #line default
        #line hidden
        
        
        #line 324 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBack;
        
        #line default
        #line hidden
        
        
        #line 328 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFinish;
        
        #line default
        #line hidden
        
        
        #line 336 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.LayoutPanel navigateDocumentPanel;
        
        #line default
        #line hidden
        
        
        #line 357 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ComboBoxEdit combTestSpec;
        
        #line default
        #line hidden
        
        
        #line 366 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxSearch;
        
        #line default
        #line hidden
        
        
        #line 367 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 370 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView treeView;
        
        #line default
        #line hidden
        
        
        #line 372 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem treeItem;
        
        #line default
        #line hidden
        
        
        #line 390 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem expandAllItem;
        
        #line default
        #line hidden
        
        
        #line 391 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem UnexpandAllItem;
        
        #line default
        #line hidden
        
        
        #line 392 "..\..\..\DataDisplay\ResultDisplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem IsAddToChart;
        
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
            System.Uri resourceLocater = new System.Uri("/MeasurementUI;component/datadisplay/resultdisplay.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DataDisplay\ResultDisplay.xaml"
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
            
            #line 24 "..\..\..\DataDisplay\ResultDisplay.xaml"
            ((MeasurementUI.ResultDisplay)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.DockingDemoModule_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.dockLayoutManager1 = ((DevExpress.Xpf.Docking.DockLayoutManager)(target));
            
            #line 183 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.dockLayoutManager1.DockOperationCompleted += new DevExpress.Xpf.Docking.Base.DockOperationCompletedEventHandler(this.dockLayoutManager1_DockOperationCompleted);
            
            #line default
            #line hidden
            return;
            case 4:
            this.documentGroup2 = ((DevExpress.Xpf.Docking.DocumentGroup)(target));
            return;
            case 5:
            this.inputDocumentPanel = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 6:
            this.inputGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.remarksPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.flowDocumentPanel = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 9:
            this.tabcontrol = ((System.Windows.Forms.TabControl)(target));
            return;
            case 10:
            this.documentGroup1 = ((DevExpress.Xpf.Docking.DocumentGroup)(target));
            return;
            case 11:
            this.documentPanle2 = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 12:
            this.loopGridControl = ((DevExpress.Xpf.Grid.GridControl)(target));
            
            #line 220 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.loopGridControl.CustomColumnDisplayText += new DevExpress.Xpf.Grid.CustomColumnDisplayTextEventHandler(this.loopGridControl_CustomColumnDisplayText);
            
            #line default
            #line hidden
            return;
            case 13:
            this.dpChart2 = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 14:
            this.InterfaceChart2 = ((SymtChartLib.WindowChart)(target));
            return;
            case 15:
            this.documentGroup3 = ((DevExpress.Xpf.Docking.DocumentGroup)(target));
            return;
            case 16:
            this.passFailDocumentPanel = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 17:
            this.passFailGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 18:
            this.listBoxStep = ((System.Windows.Controls.ListBox)(target));
            return;
            case 19:
            this.documentGroup4 = ((DevExpress.Xpf.Docking.DocumentGroup)(target));
            return;
            case 20:
            this.documentPanel1 = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 21:
            this.contentControl = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 22:
            this.Start = ((System.Windows.Controls.Button)(target));
            
            #line 285 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.Start.Click += new System.Windows.RoutedEventHandler(this.Start_Click);
            
            #line default
            #line hidden
            return;
            case 23:
            this.documentPanel2 = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 24:
            this.listBoxStep2 = ((System.Windows.Controls.ListBox)(target));
            
            #line 297 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.listBoxStep2.KeyDown += new System.Windows.Input.KeyEventHandler(this.listBoxStep2_KeyDown);
            
            #line default
            #line hidden
            return;
            case 26:
            this.btnPause = ((System.Windows.Controls.Button)(target));
            
            #line 318 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.btnPause.Click += new System.Windows.RoutedEventHandler(this.btnPause_Click);
            
            #line default
            #line hidden
            return;
            case 27:
            this.btnStop = ((System.Windows.Controls.Button)(target));
            
            #line 321 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.btnStop.Click += new System.Windows.RoutedEventHandler(this.btnStop_Click);
            
            #line default
            #line hidden
            return;
            case 28:
            this.btnBack = ((System.Windows.Controls.Button)(target));
            
            #line 324 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.btnBack.Click += new System.Windows.RoutedEventHandler(this.btnBack_Click);
            
            #line default
            #line hidden
            return;
            case 29:
            this.btnFinish = ((System.Windows.Controls.Button)(target));
            
            #line 328 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.btnFinish.Click += new System.Windows.RoutedEventHandler(this.btnFinish_Click);
            
            #line default
            #line hidden
            return;
            case 30:
            this.navigateDocumentPanel = ((DevExpress.Xpf.Docking.LayoutPanel)(target));
            return;
            case 31:
            this.combTestSpec = ((DevExpress.Xpf.Editors.ComboBoxEdit)(target));
            
            #line 357 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.combTestSpec.SelectedIndexChanged += new System.Windows.RoutedEventHandler(this.combTestSpec_SelectedIndexChanged);
            
            #line default
            #line hidden
            return;
            case 32:
            this.txtBoxSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 366 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.txtBoxSearch.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtBoxSearch_KeyDown);
            
            #line default
            #line hidden
            return;
            case 33:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 367 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 34:
            this.treeView = ((System.Windows.Controls.TreeView)(target));
            
            #line 370 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.treeView.AddHandler(System.Windows.Controls.TreeViewItem.SelectedEvent, new System.Windows.RoutedEventHandler(this.treeView_Selected));
            
            #line default
            #line hidden
            return;
            case 35:
            this.treeItem = ((System.Windows.Controls.TreeViewItem)(target));
            return;
            case 36:
            this.expandAllItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 390 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.expandAllItem.Click += new System.Windows.RoutedEventHandler(this.expandAllItem_Click);
            
            #line default
            #line hidden
            return;
            case 37:
            this.UnexpandAllItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 391 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.UnexpandAllItem.Click += new System.Windows.RoutedEventHandler(this.UnexpandAllItem_Click);
            
            #line default
            #line hidden
            return;
            case 38:
            this.IsAddToChart = ((System.Windows.Controls.MenuItem)(target));
            
            #line 392 "..\..\..\DataDisplay\ResultDisplay.xaml"
            this.IsAddToChart.Click += new System.Windows.RoutedEventHandler(this.IsAddToChart_Click);
            
            #line default
            #line hidden
            return;
            case 39:
            
            #line 403 "..\..\..\DataDisplay\ResultDisplay.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 25:
            
            #line 311 "..\..\..\DataDisplay\ResultDisplay.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnConnect_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

