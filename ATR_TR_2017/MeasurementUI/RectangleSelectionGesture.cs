using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;

namespace MeasurementUI
{
  public class RectangleSelectionGesture
  {
    #region Static Members

    public static ComponentResourceKey SelectionVisualStyleKey { get; private set; }

    private static readonly Style _DefaultSelectionVisualStyle;

    static RectangleSelectionGesture()
    {
      _DefaultSelectionVisualStyle = new Style();
      _DefaultSelectionVisualStyle.Setters.Add(new Setter(Shape.StrokeProperty, Brushes.Blue));
      _DefaultSelectionVisualStyle.Setters.Add(new Setter(Shape.FillProperty, new SolidColorBrush(Color.FromArgb(66, 0, 0, 255))));
      SelectionVisualStyleKey = new ComponentResourceKey(typeof(RectangleSelectionGesture), "SelectionVisualStyle");
    }

    #endregion

    private FrameworkElement _TargetElement;
    private RectangleGeometry _SelectionRectGeometry;
    private Path _SelectionVisual;
    private bool _Watching;
    private Point _StartPoint;
    private bool _IsEnabled;
    private CustomAdorner _CustomAdorner;
    private AdornerLayer _TargetAdornerLayer;

    /// <summary>
    /// Deffer to get AdornerLayer of _TargetElement.
    /// </summary>
    private AdornerLayer TargetAdornerLayer
    {
      get
      {
        if (_TargetAdornerLayer == null)
        {
          _TargetAdornerLayer = AdornerLayer.GetAdornerLayer(_TargetElement);
        }

        return _TargetAdornerLayer;
      }
    }    

    #region Rectangle Selection Events
   
    public event EventHandler<RecangleSelectionEventArgs> RectangleSelectionStarted;
    public event EventHandler<RecangleSelectionEventArgs> RectangleSelectionCompleted;
    public event EventHandler<RecangleSelectionEventArgs> RectangleSelectionChanging;

    protected void OnRectangleSelectionStarted(Rect rect)
    {
      if (RectangleSelectionStarted != null)
      {
        RectangleSelectionStarted(this, new RecangleSelectionEventArgs(rect));
      }
    }

    protected void OnRectangleSelectionCompleted(Rect rect)
    {
      if (RectangleSelectionCompleted != null)
      {
        RectangleSelectionCompleted(this, new RecangleSelectionEventArgs(rect));
      }
    }

    protected void OnRectangleSelectionChanging(Rect rect)
    {
      if (RectangleSelectionChanging != null)
      {
        RectangleSelectionChanging(this, new RecangleSelectionEventArgs(rect));
      }
    }

    #endregion

    #region Constructor

    public RectangleSelectionGesture(FrameworkElement targetElement, Style selectionVisualStyle)
    {
      _TargetElement = targetElement;
      IntializeSelectionVisual(selectionVisualStyle);
      _CustomAdorner = new CustomAdorner(_TargetElement, _SelectionVisual);
    }

    public RectangleSelectionGesture(FrameworkElement targetElement)
      : this(targetElement, null)
    { }

    private void IntializeSelectionVisual(Style style)
    {
      _SelectionVisual = new Path();

      if (style != null)
      {
        _SelectionVisual.Style = style;
      }
      else
      {
        style = _TargetElement.TryFindResource(SelectionVisualStyleKey) as Style;

        if (style != null)
        {
          _SelectionVisual.Style = style;
        }
        else
        {
          _SelectionVisual.Style = _DefaultSelectionVisualStyle;
        }
      }

      _SelectionRectGeometry = new RectangleGeometry(new Rect(0, 0, 0, 0));
      _SelectionVisual.Data = _SelectionRectGeometry;
    }

    #endregion
   
    private void TargetElementLostMouseCapture(object sender, MouseEventArgs e)
    {
      _Watching = false;

      Adorner[] adorners = TargetAdornerLayer.GetAdorners(_TargetElement);
      if(adorners !=null && _CustomAdorner !=null)
      {
          if (Array.IndexOf(adorners, _CustomAdorner) != -1)
          {
            TargetAdornerLayer.Remove(_CustomAdorner);
          }  
      }
    }

    private void TargetElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    { 
      _Watching = true;
      _StartPoint = e.GetPosition(_TargetElement);
      if(_TargetElement as ListView !=null)
      {
          ListView listView = _TargetElement as ListView;
          if(listView.SelectedItems.Count > 1)
          {
              listView.SelectedIndex = -1;
          }
      } 
    }

    private void TargetElementMouseMove(object sender, MouseEventArgs e)
    {
      if (_Watching)
      {
        Point curPos = e.GetPosition(_TargetElement);
        _SelectionRectGeometry.Rect = new Rect(_StartPoint, curPos);

        if (_TargetElement.IsMouseCaptured)
        {
          OnRectangleSelectionChanging(_SelectionRectGeometry.Rect);
        }
        else if (_SelectionRectGeometry.Rect.Width > SystemParameters.MinimumHorizontalDragDistance
          || _SelectionRectGeometry.Rect.Height > SystemParameters.MinimumVerticalDragDistance)
        {
          if (_TargetElement.CaptureMouse())
          {
            TargetAdornerLayer.Add(_CustomAdorner);
            OnRectangleSelectionStarted(_SelectionRectGeometry.Rect);
          }
        }
      }
    }

    private void TargetElementMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      _Watching = false;

      if (_TargetElement.IsMouseCaptured)
      {
          _TargetElement.ReleaseMouseCapture();
          Point curPos = e.GetPosition(_TargetElement);
          Rect selectionRect = new Rect(_StartPoint, curPos);
          OnRectangleSelectionCompleted(selectionRect);
          TargetAdornerLayer.Remove(_CustomAdorner);
      }
    }

    public bool IsEnabled
    {
      get
      {
        return _IsEnabled;
      }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;

          if (value)
          {
            _TargetElement.MouseLeftButtonDown += TargetElementMouseLeftButtonDown;
            _TargetElement.MouseMove += TargetElementMouseMove;
            _TargetElement.MouseLeftButtonUp += TargetElementMouseLeftButtonUp;
            _TargetElement.LostMouseCapture += TargetElementLostMouseCapture;
          }
          else
          {
            _TargetElement.MouseLeftButtonDown -= TargetElementMouseLeftButtonDown;
            _TargetElement.MouseMove -= TargetElementMouseMove;
            _TargetElement.MouseLeftButtonUp -= TargetElementMouseLeftButtonUp;
            _TargetElement.LostMouseCapture -= TargetElementLostMouseCapture;
          }
        }
      }
    }
  }
}
