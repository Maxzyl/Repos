using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MeasurementUI
{
  public class RecangleSelectionEventArgs : EventArgs
  {
    public Rect SelectionRect { get; private set; }

    public RecangleSelectionEventArgs(Rect selectionRect)
    {
      SelectionRect = selectionRect;
    }
  }
}
