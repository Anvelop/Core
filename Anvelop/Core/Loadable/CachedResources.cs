using System.Collections.Generic;
using UnityEngine;

namespace Anvelop.Core.Loadable
{
	public class CachedResources : MonoBehaviour
	{
		private readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();

		public T Load<T>(string path) where T : Object
		{
			if (!_cache.ContainsKey(path))
			{
				_cache[path] = new LoadableResource<T>(path).Load();
			}

			return (T)_cache[path];
		}
	}
}