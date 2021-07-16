
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
    private Material _idleMaterial;

    private void Awake()
    {
      _idleMaterial = _geometry.GetComponent<Renderer>().material;
    }

    private void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        _owner.Index = _index;
      }
    }

    private void OnHoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        _geometry.GetComponent<Renderer>().material =
          App_Resources.Instance.MyCommonResources.DropdownItemHoveredMaterial;
      }
    }

    private void OnUnhoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        _geometry.GetComponent<Renderer>().material = _idleMaterial;
      }
    }

    private void OnEnable()
    {
      Control.OnControlClicked += OnClickedEventListener;
      Control.OnControlHovered += OnHoveredEventListener;
      Control.OnControlUnhovered += OnUnhoveredEventListener;
    }

    private void OnDisable()
    {
      Control.OnControlClicked -= OnClickedEventListener;
      Control.OnControlHovered -= OnHoveredEventListener;
      Control.OnControlUnhovered -= OnUnhoveredEventListener;
      OnUnhoveredEventListener(this);
    }
  }
}
