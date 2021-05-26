using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Component
{
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				InitializeSingleton();
			}
			return _instance;
		}
	}

	protected static void InitializeSingleton()
	{
		if (_instance) { return; }
		var objs = FindObjectsOfType(typeof(T)) as T[];
		if (objs.Length == 0)
		{
			GameObject obj = new GameObject();
			obj.hideFlags = HideFlags.HideAndDontSave;
			_instance = obj.AddComponent<T>();
			Debug.LogError("No instances of singleton " + typeof(T).Name + ".  New instance created.");
		}
		else if (objs.Length == 1)
		{
			_instance = objs[0];
		}
		else
		{
			_instance = objs[0];
			Debug.LogError("Multiple instances of singleton " + typeof(T).Name + ".  Choosing First.");
		}
	}

	private static T _instance;
}
