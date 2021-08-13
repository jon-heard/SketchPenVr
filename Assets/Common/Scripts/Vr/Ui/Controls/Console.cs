using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Console : ScrolledList
  {
    public Renderer PrefabGeometry;
    public TextMesh PrefabLabel;
    public Material DividerMaterial;

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

    public static void Print(string text)
    {
      Debug.Log("CONSOLE >> " + text);
      _instance.PrintInternal(text);
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
    private static Console _instance;
    private static Material _mainPrefabGeometryMaterial;

    protected override void Awake()
    {
      base.Awake();
      _instance = this;
      _itemHeight = PrefabLabel.transform.parent.Find("Geometry").localScale.y;
      _mainPrefabGeometryMaterial = PrefabGeometry.material;
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
        PrintInternal("");
      }
      _placeholderCount = ShownCount;
      MyScrollbar.SetRange(ShownCount, ShownCount);
    }

    private void PrintInternal(string text)
    {
      PrefabLabel.text = text;
      Transform newItem = null;
      var place = (int)(_list.Count - _placeholderCount);
      if (place % 3 == 2)
      {
        PrefabGeometry.material = DividerMaterial;
        newItem = Instantiate(PrefabLabel.transform.parent, Items);
        PrefabGeometry.material = _mainPrefabGeometryMaterial;
      }
      else
      {
        newItem = Instantiate(PrefabLabel.transform.parent, Items);
      }
      var t = newItem.localPosition;
      t.y = -_itemHeight * place;
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
  }
}
