using System.Collections.Generic;
using UnityEngine;

namespace Anvelop.Core.Loadable
{
	public class CachedResources : MonoBehaviour
	{
		static readonly Dictionary<string, Object> Cache = new Dictionary<string, Object>();

		public T Load<T>(string path) where T : Object
		{
			if (!Cache.ContainsKey(path))
			{
				Cache[path] = new LoadableResource<T>(path).Load();
			}

			return (T)Cache[path];
		}
	}
}