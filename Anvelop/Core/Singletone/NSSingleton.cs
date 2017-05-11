namespace Anvelop.Core.Singletone
{
	public abstract class NSSingleton<T> where T : NSSingleton<T>, new()
	{
		private static T _instance;

		public static T Instance
		{
			get { return GetInstance(); }
		}

		public static T I
		{
			get { return Instance; }
		}

		public static bool isNull
		{
			get { return _instance == null; }
		}

		public static T GetInstance()
		{
			if (_instance == null)
			{
				_instance = new T();
			}

			return _instance;
		}
	}
}