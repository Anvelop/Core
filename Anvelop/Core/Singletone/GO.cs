using UnityEngine;

namespace Anvelop.Core.Singletone
{
	public abstract class GO : Script
	{
		private static GameObject _go;

		protected static GameObject pGO
		{
			get
			{
				if (_go == null)
				{
					_go = GameObject.Find("Singletones") ?? new GameObject("Singletones");

					if (Application.isPlaying)
					{
						DontDestroyOnLoad(_go);
					}
				}

				return _go;
			}
		}

		protected virtual void OnDestroy()
		{
			_go = null;
		}
	}
}