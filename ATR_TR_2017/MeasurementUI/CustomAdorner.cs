using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MeasurementUI
{
  public class CustomAdorner : Adorner
  {
    public CustomAdorner(UIElement adornedElement, UIElement adornerElement)
      : base(adornedElement)
    {
      _VisualChildren = new VisualCollection(this);
      _Presenter = new ContentPresenter();
      _Presenter.Content = adornerElement;
      _VisualChildren.Add(_Presenter);
    }

    private VisualCollection _VisualChildren;
    private ContentPresenter _Presenter;

    protected override Visual GetVisualChild(int index)
    {
      return _VisualChildren[index];
    }

    protected override int VisualChildrenCount
    {
      get
      {
        return _VisualChildren.Count;
      }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      _Presenter.Arrange(new Rect(finalSize));
      return _Presenter.RenderSize;
    }
  }
}
