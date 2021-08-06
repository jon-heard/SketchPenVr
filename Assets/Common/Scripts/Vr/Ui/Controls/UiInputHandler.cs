using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class UiInputHandler
  {
    public void UpdatePointer(Control hoveredControl, bool isDown, Vector3 point, bool isPrimaryController)
    {
      // Hovering
      if (hoveredControl != _hoveredControl)
      {
        if (_hoveredControl)
        {
          Control.WasPrimaryController = isPrimaryController;
          Control.OnControlUnhovered?.Invoke(_hoveredControl);
        }
        if (_downControl != null && hoveredControl != _downControl) { hoveredControl = null; }
        _hoveredControl = hoveredControl;
        if (_hoveredControl)
        {
          Control.WasPrimaryController = isPrimaryController;
          Control.OnControlHovered?.Invoke(_hoveredControl);
        }
      }

      // Mouse down / up
      if (isDown != _isDown)
      {
        _isDown = isDown;
        if (isDown)
        {
          if (_hoveredControl)
          {
            _downControl = _hoveredControl;
            Control.WasPrimaryController = isPrimaryController;
            Control.OnControlDown?.Invoke(_downControl);
          }
        }
        else
        {
          if (_downControl)
          {
            Control.WasPrimaryController = isPrimaryController;
            Control.OnControlUp?.Invoke(_downControl);
            if (_downControl == _hoveredControl)
            {
              Control.WasPrimaryController = isPrimaryController;
              Control.OnControlClicked?.Invoke(_downControl);
            }
            _downControl = null;
          }
        }
      }

      // uv
      if (hoveredControl && point != _point)
      {
        Control.WasPrimaryController = isPrimaryController;
        Control.OnPointerMoved?.Invoke(point);
        _point = point;
      }
    }

    private Vector3 _point;
    private Control _hoveredControl;
    private Control _downControl;
    private bool _isDown;
  }
}
