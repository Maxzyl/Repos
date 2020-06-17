using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoBase;
using DevExpress.Xpf.Docking;

namespace MeasurementUI
{
    public class DockingDemoModule : DemoModule
    {
        #region static
        public static readonly DependencyProperty DockLayoutManagerProperty;
        static DockingDemoModule()
        {
            DockLayoutManagerProperty = DependencyProperty.Register(
                "DockLayoutManager", typeof(DockLayoutManager), typeof(DockingDemoModule), new FrameworkPropertyMetadata(null));
        }
        #endregion
        public DockingDemoModule()
        {

        }
        protected override bool CanLeave()
        {
            if (Container != null) HideFloatGroups(Container);
            return true;
        }
        List<DockLayoutManager> DisposeList;
        protected override void RaiseBeforeModuleDisappear()
        {
            base.RaiseBeforeModuleDisappear();
            DisposeList = new List<DockLayoutManager>();
            VisualTreeEnumerator e = new VisualTreeEnumerator(this);
            while (e.MoveNext())
            {
                DockLayoutManager manager = e.Current as DockLayoutManager;
                if (manager != null)
                {
                    DisposeList.Add(manager);
                    manager.Visibility = Visibility.Collapsed;
                    foreach (FloatGroup fGroup  in manager.FloatGroups)
                    {
                        fGroup.IsOpen = false;
                    }
                }
            }
        }
        protected override void RaiseAfterModuleDisappear()
        {
            base.RaiseAfterModuleDisappear();
            if (DisposeList != null)
            {
                foreach (DockLayoutManager manager in DisposeList)
                    manager.Dispose();
                DisposeList.Clear();
                DisposeList = null;
            }
        }
        protected void NavigateHomePage()
        {
            System.Diagnostics.Process.Start("http://www.devexpress.com");
        }
        protected void ShowAbout()
        {
        }
        DockLayoutManager container;
        protected virtual DockLayoutManager Container
        {
            get
            {
                if (container == null) container = FindDockLayoutManager();
                return container;
            }
        }
        public DockLayoutManager FindDockLayoutManager()
        {
            VisualTreeEnumerator e = new VisualTreeEnumerator(this);
            while (e.MoveNext())
            {
                DockLayoutManager manager = e.Current as DockLayoutManager;
                if (manager != null) return manager;
            }
            return null;
        }
        protected override void RaiseIsPopupContentInvisibleChanged(DependencyPropertyChangedEventArgs e)
        {
            base.RaiseIsPopupContentInvisibleChanged(e);
            if (Container == null) return;
            if (object.Equals(e.NewValue, true))
                HideFloatGroups(Container);
            else
                ShowFloatGroups(Container);
        }
        void ShowFloatGroups(DockLayoutManager dockLayoutManager)
        {
            foreach (FloatGroup floatGroup in dockLayoutManager.FloatGroups)
            {
                if (floatGroup.ShouldRestoreOnActivate)
                {
                    floatGroup.ShouldRestoreOnActivate = false;
                    if (!floatGroup.IsOpen) floatGroup.IsOpen = true;
                }
            }
        }
        void HideFloatGroups(DockLayoutManager dockLayoutManager)
        {
            foreach (FloatGroup floatGroup in dockLayoutManager.FloatGroups)
            {
                if (floatGroup.IsOpen)
                {
                    floatGroup.ShouldRestoreOnActivate = true;
                    if (floatGroup.IsOpen) floatGroup.IsOpen = false;
                }
            }
        }
    }
}
