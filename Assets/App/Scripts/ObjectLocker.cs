using System.Collections.Generic;

public class ObjectLocker
{
  public System.Action<bool> LockStateChanged;

  public bool IsLocked
  {
    get
    {
      foreach (var oneLock in _locks)
      {
        if (oneLock.Value)
        {
          return true;
        }
      }
      return false;
    }
  }

  public bool GetLock(string name)
  {
    if (!_locks.ContainsKey(name)) { return false; }
    return _locks[name];
  }

  public void SetLock(string name, bool locked)
  {
    if (_locks.ContainsKey(name) && locked == _locks[name]) { return; }
    var preIsLocked = IsLocked;
    _locks[name] = locked;
    if (IsLocked != preIsLocked)
    {
      LockStateChanged?.Invoke(!preIsLocked);
    }
  }

  private Dictionary<string, bool> _locks = new Dictionary<string, bool>();
}
