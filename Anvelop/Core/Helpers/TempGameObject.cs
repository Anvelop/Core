using Anvelop.Core.Singletone;
using UnityEngine;

namespace Anvelop.Core.Helpers
{
	public class TempGameObject : CurrentSceneSingleton<TempGameObject>
	{
		private static int _lastId;

		private static int LastTempUid
		{
			get
			{
				_lastId++;
				return _lastId;
			}
		}
	
		public static GameObject Create()
		{
			return Create(Vector3.zero);
		}

		public static GameObject Create(Vector3 posisition)
		{
			GameObject go = new GameObject("_temp_" + LastTempUid);
			go.transform.SetParent(Instance.transform);
			go.transform.position = posisition;
			return go;
		}
	}
}