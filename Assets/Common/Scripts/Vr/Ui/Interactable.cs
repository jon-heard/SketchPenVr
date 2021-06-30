using UnityEngine;

namespace Common.Vr.Ui
{
  public class Interactable : MonoBehaviour
  {
    public Interactable DragInteractable
    {
      get
      {
        var parentInteractable = transform.parent?.GetComponent<Interactable>();
        return parentInteractable ? parentInteractable.DragInteractable : this;
      }
    }

    public void SetTemporaryParent(Transform parent, Transform oldParentConfirm = null)
    {
      if (parent)
      {
        if (!_primaryParentIsSet)
        {
          _primaryParent = transform.parent;
          _primaryParentIsSet = true;
        }
        transform.parent = parent;
      }
      else if (oldParentConfirm == transform.parent)
      {
        transform.parent = _primaryParent;
        _primaryParentIsSet = false;
      }
    }
    private bool _primaryParentIsSet = false;
    private Transform _primaryParent = null;
  }
}
