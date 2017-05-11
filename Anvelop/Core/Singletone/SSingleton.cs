using UnityEngine;

namespace Anvelop.Core.Singletone
{
	public abstract class SSingleton<T> : GO where T : SSingleton<T>
	{
		private static T _instance;

		/// <summary>
		///     When Unity quits, it destroys objects in a random order.
		///     In principle, a Singleton is only destroyed when application quits.
		///     If any script calls Instance after it have been destroyed,
		///     it will create a buggy ghost object that will stay on the Editor scene
		///     even after stopping playing the Application. Really bad!
		///     So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		private static bool applicationIsQuitting;

		public static T Instance
		{
			get { return GetInstance(); }
		}

		public static T I
		{
			get { return Instance; }
		}

		public static bool Exists
		{
			get { return !applicationIsQuitting && _instance != null; }
		}

		private static T GetInstance()
		{
			if (applicationIsQuitting)
			{
				Debug.Log("[Singleton] Instance '" + typeof (T) +
				          "' already destroyed on application quit." +
				          " Won't create again - returning null.");

				return null;
			}

			if (_instance == null)
			{
				_instance = pGO.AddComponent<T>();
			}

			return _instance;
		}

		void OnApplicationQuit()
		{
			applicationIsQuitting = true;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			_instance = null;
		}
	}
}