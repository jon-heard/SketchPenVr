using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Console : ScrolledList
  {
    public TextMesh PrefabLabel;

    //DEV: Console "Print" test
    //public string ToPrint;
    //public bool RunPrint;
    //private void Update()
    //{
    //  if (RunPrint)
    //  {
    //    RunPrint = false;
    //    Print(ToPrint);
    //    ToPrint = "";
    //  }
    //}

    public void Print(string text)
    {
      PrefabLabel.text = text;
      var newItem = Instantiate(PrefabLabel.transform.parent, Items);
      var place = (int)(_list.Count - _placeholderCount);
      var t = newItem.localPosition;
      t.y = - _itemHeight * place;
      newItem.localPosition = t;
      if (_placeholderCount > 0)
      {
        Destroy(_list[place]);
        _list.RemoveAt(place);
        _placeholderCount--;
      }
      _list.Insert(place, newItem.gameObject);
      if (_list.Count <= ShownCount)
      {
        newItem.gameObject.SetActive(true);
      }
      else
      {
        var isAtBottom = MyScrollbar.IsAtBottom;
        var currentScrollIndex = _scrollIndex;
        MyScrollbar.SetRange(ShownCount, (uint)_list.Count);
        MyScrollbar.ScrollPosition =
          isAtBottom ? (uint)(_list.Count - ShownCount) : currentScrollIndex;
        MyScrollbar.UpdateDraggerPosition();
      }
    }

    public void OnClearButton()
    {
      Clear();
    }

    public void OnCloseButton()
    {
      gameObject.SetActive(false);
    }

    private uint _placeholderCount;

    protected override void Awake()
    {
      base.Awake();
      _itemHeight = PrefabLabel.transform.parent.Find("Geometry").localScale.y;
      PrefabLabel.transform.parent.gameObject.SetActive(false);
      Clear();
      gameObject.SetActive(false);
    }

    private void Clear()
    {
      for (var i = 0; i < _list.Count; i++)
      {
        Destroy(_list[i]);
      }
      _list.Clear();
      _placeholderCount = 0;
      for (var i = 0; i < ShownCount; i++)
      {
        Print("");
      }
      _placeholderCount = ShownCount;
      MyScrollbar.SetRange(ShownCount, ShownCount);
    }
  }
}
