﻿#pragma checksum "..\..\User.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7C86ECDC7418F333962C68F46377D2DA3943C061"
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
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Grid.Themes;
using DevExpress.Xpf.Grid.TreeList;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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
using UserMaintenance;


namespace UserMaintenance {
    
    
    /// <summary>
    /// User
    /// </summary>
    public partial class User : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DockLayoutManager dockLayoutManager1;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.GridControl gridControl1;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Grid.TableView tableView1;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem btnRefresh;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem btnClear;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem btnDelete;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem btUpdate;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Bars.BarButtonItem btnSave;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DockLayoutManager dockLayoutManager2;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentGroup tabbedGroup2;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel dpnormal;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ebUserID;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ebUserName;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox ebPwd;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox ebPwdcfm;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ebEmail;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ebPhone;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ebDepat;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbxEnable;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Docking.DocumentPanel dpUserGroup;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Grid9085;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\User.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ListBoxEdit lbeUserGroup;
        
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
            System.Uri resourceLocater = new System.Uri("/UserMaintenance;component/user.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\User.xaml"
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
            
            #line 14 "..\..\User.xaml"
            ((UserMaintenance.User)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            
            #line 14 "..\..\User.xaml"
            ((UserMaintenance.User)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UserControl_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dockLayoutManager1 = ((DevExpress.Xpf.Docking.DockLayoutManager)(target));
            return;
            case 3:
            this.gridControl1 = ((DevExpress.Xpf.Grid.GridControl)(target));
            
            #line 20 "..\..\User.xaml"
            this.gridControl1.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.gridControl1_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tableView1 = ((DevExpress.Xpf.Grid.TableView)(target));
            return;
            case 5:
            this.btnRefresh = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 47 "..\..\User.xaml"
            this.btnRefresh.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnClear = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 48 "..\..\User.xaml"
            this.btnClear.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.btnClear_ItemClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnDelete = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 49 "..\..\User.xaml"
            this.btnDelete.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.btnDelete_ItemClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btUpdate = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 50 "..\..\User.xaml"
            this.btUpdate.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.btUpdate_ItemClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnSave = ((DevExpress.Xpf.Bars.BarButtonItem)(target));
            
            #line 51 "..\..\User.xaml"
            this.btnSave.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(this.btnSave_ItemClick);
            
            #line default
            #line hidden
            return;
            case 10:
            this.dockLayoutManager2 = ((DevExpress.Xpf.Docking.DockLayoutManager)(target));
            return;
            case 11:
            this.tabbedGroup2 = ((DevExpress.Xpf.Docking.DocumentGroup)(target));
            return;
            case 12:
            this.dpnormal = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 13:
            this.ebUserID = ((System.Windows.Controls.TextBox)(target));
            return;
            case 14:
            this.ebUserName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.ebPwd = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 16:
            this.ebPwdcfm = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 17:
            this.ebEmail = ((System.Windows.Controls.TextBox)(target));
            return;
            case 18:
            this.ebPhone = ((System.Windows.Controls.TextBox)(target));
            return;
            case 19:
            this.ebDepat = ((System.Windows.Controls.TextBox)(target));
            return;
            case 20:
            this.cbxEnable = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 21:
            this.dpUserGroup = ((DevExpress.Xpf.Docking.DocumentPanel)(target));
            return;
            case 22:
            this.Grid9085 = ((System.Windows.Controls.Grid)(target));
            
            #line 96 "..\..\User.xaml"
            this.Grid9085.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Grid9085_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 23:
            this.lbeUserGroup = ((DevExpress.Xpf.Editors.ListBoxEdit)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

