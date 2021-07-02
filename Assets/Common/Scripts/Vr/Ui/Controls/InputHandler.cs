
namespace Common.Vr.Ui.Controls
{
  public class InputHandler
  {
    public InputHandler() {}

    public void PointerDown(bool isDown)
    {
      if (isDown == _isDown) { return; }
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

    public void PointerHovered(Control focusControl)
    {
      if (focusControl == _hoveredControl) { return; }
      if (_hoveredControl) { Control.OnControlUnhovered?.Invoke(_hoveredControl); }
      if (_downControl != null && focusControl != _downControl) { focusControl = null; }
      _hoveredControl = focusControl;
      if (_hoveredControl) { Control.OnControlHovered?.Invoke(_hoveredControl); }
    }

    private Control _hoveredControl;
    private Control _downControl;
    private bool _isDown;
  }
}
