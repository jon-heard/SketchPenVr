using Common.Vr.Ui.Controls;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Vr.Ui
{
  public class Ui_Menu : MonoBehaviour
  {
    [SerializeField] private Ui_Menu _parentMenu;

    public virtual bool Show(Button source = null)
    {
      if (source)
      {
        source.State = Button.ButtonState.LockedDown;
        _source = source;
      }
      if (gameObject.activeSelf) { return true; }
      gameObject.SetActive(true);
      return true;
    }
    public virtual bool Hide()
    {
      if (!gameObject.activeSelf) { return true; }

      var allChildrenVisibilitySet = true;
      foreach (var child in _children)
      {
        if (!child.Hide())
        {
          allChildrenVisibilitySet = false;
        }
      }
      if (allChildrenVisibilitySet)
      {
        if (_source)
        {
          _source.State = Button.ButtonState.NotLockedDown;
        }
        gameObject.SetActive(false);
        if (_parentMenu) { _parentMenu.Hide(); }
      }
      return allChildrenVisibilitySet;
    }
    public bool Toggle()
    {
      return gameObject.activeSelf ? Hide() : Show();
    }

    private List<Ui_Menu> _children = new List<Ui_Menu>();
    private Button _source;

    private void Awake()
    {
      if (_parentMenu) { transform.parent = _parentMenu.transform; }
    }
    protected virtual void Start()
    {
      foreach (Transform child in transform)
      {
        var childMenu = child.GetComponent<Ui_Menu>();
        if (childMenu) { _children.Add(childMenu); }
      }
    }
  }
}
