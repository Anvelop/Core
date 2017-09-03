using System;
using System.Collections.Generic;
using Anvelop.Core.Extensions;
using UnityEngine;

namespace Anvelop.Core.Loadable
{
	public class PrewarmCachedResources
	{
		private readonly CachedResources _cachedResources;

		public PrewarmCachedResources(CachedResources cachedResources)
		{
			_cachedResources = cachedResources;
		}

		public void PrewarmAsync(List<string> paths, Action callbackAction)
		{
			int count = paths.Count;

			if (count == 0)
			{
				callbackAction.Invoke();
				return;
			}

			paths.Do(delegate(string path)
			{
				_cachedResources.LoadAsync<GameObject>(path, o =>
				{
					count--;

					if (count == 0)
					{
						callbackAction.Invoke();
					}
				});
			});
		}
	}
}