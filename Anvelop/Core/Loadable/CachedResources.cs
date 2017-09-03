using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Anvelop.Core.Loadable
{
	public class CachedResources : MonoBehaviour
	{
		private readonly Dictionary<string, LoadableResource<Object>> _cache = new Dictionary<string, LoadableResource<Object>>();

		public T Load<T>(string path) where T : Object
		{
			AddLoader<T>(path);

			return (T)_cache[path].Load();
		}

		private void AddLoader<T>(string path) where T : Object
		{
			if (!_cache.ContainsKey(path))
			{
				var res = new LoadableResource<Object>(path);
				_cache[path] = res;
			}
		}

		public void LoadAsync<T>(string path, Action<T> callback) where T : Object
		{
			AddLoader<T>(path);

			_cache[path].LoadAsync(delegate(Object o)
			{
				callback(o as T);
			});
		}

		public CachedResources Remove(string path)
		{
			_cache.Remove(path);
			return this;
		}

		public CachedResources Unload(string path)
		{
			_cache[path].Unload();
			return this;
		}

		public CachedResources UnloadAll()
		{
			foreach (var o in _cache)
			{
				o.Value.Unload();
			}

			return this;
		}

		private void OnDestroy()
		{
			UnloadAll();
		}
	}
}