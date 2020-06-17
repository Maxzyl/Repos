using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace MeasurementUI
{
  public class ListBoxRectSelection
  {
    private ListBox _TargetList;
    private RectangleSelectionGesture _RectangleSelectionGesture;
    
    public bool IsEnabled
    {
      get { return _RectangleSelectionGesture.IsEnabled; }
      set { _RectangleSelectionGesture.IsEnabled = value; }
    }

    public ListBoxRectSelection(ListBox target)
    {
      _TargetList = target;
      _RectangleSelectionGesture = new RectangleSelectionGesture(target);
      _RectangleSelectionGesture.RectangleSelectionChanging += RectangleSelectionChanging;
      _RectangleSelectionGesture.RectangleSelectionStarted += RectangleSelectionStarted;
    }

    private void RectangleSelectionStarted(object sender, RecangleSelectionEventArgs e)
    {
      _TargetList.Focus();
    }    

    private void RectangleSelectionChanging(object sender, RecangleSelectionEventArgs e)
    {
      _TargetList.SelectedItems.Clear();
      VisualTreeHelper.HitTest(_TargetList, ItemHitTestFilter, ItemHitTestCallback, new GeometryHitTestParameters(new RectangleGeometry(e.SelectionRect)));
    }

    private HitTestFilterBehavior ItemHitTestFilter(DependencyObject visual)
    {
      if (visual is ListBoxItem)
      {
        ((ListBoxItem)visual).IsSelected = true;
        return HitTestFilterBehavior.ContinueSkipChildren;
      }

      return HitTestFilterBehavior.Continue;
    }

    private HitTestResultBehavior ItemHitTestCallback(HitTestResult result)
    {
      return HitTestResultBehavior.Continue;
    }
  }
}
