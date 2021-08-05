using System.Collections.Generic;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class ScrolledList : Control
  {
    public uint ShownCount = 10;
    public Scrollbar MyScrollbar;
    public Transform Items;

    protected List<GameObject> _list = new List<GameObject>();
    protected float _itemHeight;
    protected uint _scrollIndex = 0;

    protected virtual void Awake()
    {
      MyScrollbar.OnScrollValueChanged += OnScrollValueChangedEventListener;
    }

    protected void OnScrollValueChangedEventListener(uint value)
    {
      OnScrollValueChangedEventListener(value, false);
    }
    protected void OnScrollValueChangedEventListener(uint value, bool force)
    {
      if (force || value != _scrollIndex)
      {
        var t = Items.localPosition;
        t.y = value * _itemHeight;
        Items.localPosition = t;
        for (var i = 0; i < ShownCount; i++)
        {
          _list[(int)(i + _scrollIndex)].gameObject.SetActive(false);
        }
        _scrollIndex = value;
        for (var i = 0; i < ShownCount; i++)
        {
          _list[(int)(i + _scrollIndex)].gameObject.SetActive(true);
        }
      }
    }
  }
}
