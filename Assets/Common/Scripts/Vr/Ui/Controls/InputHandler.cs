using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class InputHandler
  {
    public void UpdatePointer(Control hoveredControl, bool isDown, Vector3 point)
    {
      // Hovering
      if (hoveredControl != _hoveredControl)
      {
        if (_hoveredControl) { Control.OnControlUnhovered?.Invoke(_hoveredControl); }
        if (_downControl != null && hoveredControl != _downControl) { hoveredControl = null; }
        _hoveredControl = hoveredControl;
        if (_hoveredControl) { Control.OnControlHovered?.Invoke(_hoveredControl); }
      }

      // Mouse down
      if (isDown != _isDown)
      {
        _isDown = isDown;
        if (isDown)
        {
          if (_hoveredControl)
          {
            _downControl = _hoveredControl;
            Control.OnControlDown?.Invoke(_downControl);
          }
        }
        else
        {
          if (_downControl)
          {
            Control.OnControlUp?.Invoke(_downControl);
            if (_downControl == _hoveredControl)
            {
              Control.OnControlClicked(_downControl);
            }
            _downControl = null;
          }
        }
      }

      // uv
      if (hoveredControl && point != _point)
      {
        Control.OnPointMoved?.Invoke(point);
        _point = point;
      }
    }

    private Control _hoveredControl;
    private Control _downControl;
    private bool _isDown;
    private Vector3 _point;
  }
}
