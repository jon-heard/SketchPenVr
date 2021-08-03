using System;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Scrolled : Control
  {
    public Scrollbar MyScrollbar;

    protected virtual void OnHoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        MyScrollbar.IsListeningForThumbstick = true;
      }
    }

    protected virtual void OnUnhoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        MyScrollbar.IsListeningForThumbstick = false;
      }
    }

    protected virtual void OnEnable()
    {
      Control.OnControlHovered += OnHoveredEventListener;
      Control.OnControlUnhovered += OnUnhoveredEventListener;
    }

    protected virtual void OnDisable()
    {
      Control.OnControlHovered -= OnHoveredEventListener;
      Control.OnControlUnhovered -= OnUnhoveredEventListener;
    }
  }
}
