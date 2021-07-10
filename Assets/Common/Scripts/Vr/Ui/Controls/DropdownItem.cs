
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class DropdownItem : Control
  {
    [SerializeField] private TextMesh _label;
    [SerializeField] private Transform _geometry;

    public string Text { get; private set; }

    public float Height
    {
      get { return _geometry.transform.localScale.y; }
    }

    public void Initialize(string value, Dropdown owner, uint index)
    {
      Text = value;
      _owner = owner;
      _index = index;

      _label.text = value;
      var t = transform.localPosition;
      t.y -= Height * index;
      transform.localPosition = t;
    }

    private Dropdown _owner;
    private uint _index;

    protected void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        _owner.Index = _index;
      }
    }

    private void OnEnable()
    {
      Control.OnControlClicked += OnClickedEventListener;
    }

    private void OnDisable()
    {
      Control.OnControlClicked -= OnClickedEventListener;
    }
  }
}
