
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class DropdownItem : Control
  {
    [SerializeField] private TextMesh _label;
    [SerializeField] private Transform _geometry;

    public string Text { get; private set; }

    public void Initialize(string value, Dropdown owner, uint index)
    {
      Text = value;
      _owner = owner;
      _index = index;

      _label.text = value;
      var t = transform.localPosition;
      t.y -= _geometry.transform.localScale.y * index;
      transform.localPosition = t;
    }

    private Dropdown _owner;
    private uint _index;

    protected override void DoClickInternal()
    {
      _owner.Index = _index;
      _owner.ToggleList();
    }
  }
}
