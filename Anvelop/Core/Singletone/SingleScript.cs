#if ADVANCED_INSPECTOR
using AdvancedInspector;

#endif

namespace Anvelop.Core.Singletone
{
	public abstract class SingleScript<T> : Script where T : SingleScript<T>
	{
#if ADVANCED_INSPECTOR
		[Inspect]
#endif
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
				}

				return _instance;
			}
		}

		public static bool Exists
		{
			get { return _instance != null; }
		}

		//call this method in Awake()
		protected void InitInstance(bool throughScenes = false)
		{
			_instance = (T)this;

			if (throughScenes)
			{
				DontDestroyOnLoad(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			_instance = null;
		}
	}
}