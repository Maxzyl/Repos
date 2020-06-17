using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MeasurementUI
{
  public static class ListBoxExtention
  {
    public static bool GetIsRectSelectionEnabled(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsRectSelectionEnabledProperty);
    }

    public static void SetIsRectSelectionEnabled(DependencyObject obj, bool value)
    {
      obj.SetValue(IsRectSelectionEnabledProperty, value);
    }

    public static readonly DependencyProperty IsRectSelectionEnabledProperty = DependencyProperty.RegisterAttached(
      "IsRectSelectionEnabled", typeof(bool), typeof(ListBoxRectSelection), new FrameworkPropertyMetadata(OnIsRectSelectionEnabledChanged));

    private static void OnIsRectSelectionEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if(!(sender is ListBox))
      {
        throw new ArgumentException("sender", "Target should be a ListBox and its sub class instance."); ;
      }

      bool isEnabled = (bool)e.NewValue;

      if (isEnabled)
      {
        ListBoxRectSelection rectSelection = ListBoxExtention.GetRectSelection(sender) ;
        if (rectSelection == null)
        {
            rectSelection = new ListBoxRectSelection(sender as ListView);
            ListBoxExtention.SetRectSelection(sender, rectSelection);
        }
        rectSelection.IsEnabled = isEnabled;        
      }
    } 

    private static ListBoxRectSelection GetRectSelection(DependencyObject obj)
    {
      return (ListBoxRectSelection)obj.GetValue(RectSelectionProperty);
    }

    private static void SetRectSelection(DependencyObject obj, ListBoxRectSelection value)
    {
      obj.SetValue(RectSelectionProperty, value);
    }

    private static readonly DependencyProperty RectSelectionProperty =
        DependencyProperty.RegisterAttached("RectSelection", typeof(ListBoxRectSelection), typeof(ListBoxExtention));
  }
}
