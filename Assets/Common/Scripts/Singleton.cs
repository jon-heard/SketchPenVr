
namespace Common
{
	public class Singleton<T>
		where T : new()
	{
		public static readonly T Instance = new T();
		static Singleton() {}
		protected Singleton() {}
	}
}
