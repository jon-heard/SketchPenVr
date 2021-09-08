using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr.Ui.Controls
{
  public class Dropdown : Control
  {
    [SerializeField] private UnityEvent OnItemSet;
    [SerializeField] private bool ItemReselectedTriggersItemSetEvent;
    [SerializeField] private List<string> Options;
    [Header("Wiring")]
    [SerializeField] private TextMesh Label;
    [SerializeField] private Transform Geometry;
    [SerializeField] private GameObject DroppedUi;
    [SerializeField] private Transform Items;
    [SerializeField] private DropdownItem ItemPrefab;
    [SerializeField] private Button DownButton;
    [SerializeField] private Scrollbar Scroll;

    [NonSerialized] public bool ItemSelected = false;

    public uint Index
    {
      get { return _index; }
      set
      {
        if (value != _index || (ItemReselectedTriggersItemSetEvent && ItemSelected))
        {
          _index = value;
          ItemSelected = false;
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
      _const_visibleListSize = App_Details.Instance.MyCommonDetails.SCROLLBAR_VISIBLE_LIST_SIZE;
      _listItems.Clear();
      _scrollIndex = 0;
      for (uint i = 0; i < src.Count; i++)
      {
        var newListItem = Instantiate(ItemPrefab, Items);
        _listItems.Add(newListItem);
        newListItem.Initialize(src[(int)i], this, i);
        newListItem.gameObject.SetActive(i < _const_visibleListSize);
      }
      Scroll.SetRange(_const_visibleListSize, (uint)src.Count);
    }

    public void ToggleListVisibility()
    {
      DroppedUi.SetActive(!DroppedUi.activeSelf);
    }
    public void SetListVisibility(bool isShown)
    {
      DroppedUi.SetActive(isShown);
    }

    private List<DropdownItem> _listItems = new List<DropdownItem>();
    private float _itemSize;
    private uint _scrollIndex;
    private uint _const_visibleListSize;

    protected void OnClickedEventListener(Control focus)
    {
      if (focus == this || focus == DownButton)
      {
        ToggleListVisibility();
      }
      else if (!Scroll.IsPartOfScroll(focus) && focus?.TakesFocus == true)
      {
        SetListVisibility(false);
      }
    }

    private void Start()
    {
      Scroll.OnScrollValueChanged += OnScrollValueChangedEventListener;
      _itemSize = ItemPrefab.Height;
      if (Options.Count > 0)
      {
        SetList(Options);
      }
    }

    private void OnEnable()
    {
      Control.OnControlClicked += OnClickedEventListener;
    }

    private void OnDisable()
    {
      Control.OnControlClicked -= OnClickedEventListener;
      DroppedUi.SetActive(false);
    }

    private void OnScrollValueChangedEventListener(uint value)
    {
      if (value != _scrollIndex)
      {
        var t = Items.localPosition;
        t.y = value * _itemSize;
        Items.localPosition = t;
        for (var i = 0; i < _const_visibleListSize; i++)
        {
          _listItems[(int)(i + _scrollIndex)].gameObject.SetActive(false);
        }
        _scrollIndex = value;
        for (var i = 0; i < _const_visibleListSize; i++)
        {
          _listItems[(int)(i + _scrollIndex)].gameObject.SetActive(true);
        }
      }
    }
  }
}
