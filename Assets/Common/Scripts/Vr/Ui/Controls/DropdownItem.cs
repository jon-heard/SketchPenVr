
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class DropdownItem : Scrolled
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

    protected override void Awake()
    {
      base.Awake();
      _idleMaterial = _geometry.GetComponent<Renderer>().material;
    }

    private void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        _owner.ItemSelected = true;
        _owner.Index = _index;
      }
    }

    protected override void OnHoveredEventListener(Control focus)
    {
      base.OnHoveredEventListener(focus);
      if (focus == this)
      {
        _geometry.GetComponent<Renderer>().material =
          App_Resources.Instance.MyCommonResources.DropdownItemHoveredMaterial;
      }
    }

    protected override void OnUnhoveredEventListener(Control focus)
    {
      base.OnUnhoveredEventListener(focus);
      if (focus == this)
      {
        _geometry.GetComponent<Renderer>().material = _idleMaterial;
      }
    }

    protected override void OnEnable()
    {
      Control.OnControlClicked += OnClickedEventListener;
      base.OnEnable();
    }

    protected override void OnDisable()
    {
      Control.OnControlClicked -= OnClickedEventListener;
      base.OnDisable();
    }
  }
}
