using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr.Ui.Controls
{
  public class Dropdown : Control
  {
    [SerializeField] private UnityEvent OnItemSet;
    [Header("Wiring")]
    [SerializeField] private TextMesh Label;
    [SerializeField] private Transform Geometry;
    [SerializeField] private GameObject List;
    [SerializeField] private DropdownItem ItemPrefab;

    public uint Index
    {
      get { return _index; }
      set
      {
        if (value != _index)
        {
          _index = value;
          OnItemSet.Invoke();
        }
        if (_index < _listItems.Count)
        {
          Label.text = _listItems[(int)_index].Text;
        }
      }
    }
    private uint _index = Global.NullUint;

    public void SetList(List<string> src)
    {
      foreach (var listItem in _listItems)
      {
        Destroy(listItem);
      }
      _listItems.Clear();
      var parent = List.transform;
      for (uint i = 0; i < src.Count; i++)
      {
        var newListItem = Instantiate(ItemPrefab, parent);
        _listItems.Add(newListItem);
        newListItem.Initialize(src[(int)i], this, i);
        newListItem.gameObject.SetActive(true);
      }
    }

    public void ToggleList()
    {
      List.SetActive(!List.activeSelf);
    }

    private List<DropdownItem> _listItems = new List<DropdownItem>();

    protected override void DoClickInternal()
    {
      ToggleList();
    }

    private void OnDisable()
    {
      List.SetActive(false);
    }
  }
}
