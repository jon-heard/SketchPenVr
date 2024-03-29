using UnityEngine;

namespace Common
{
	public class SingletonComponent<T> : MonoBehaviour
		where T : Component
	{
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					InitializeSingletonComponent();
				}
				return _instance;
			}
		}

		private static T _instance;

		protected static void InitializeSingletonComponent()
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
	}
}
