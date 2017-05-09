using UnityEngine;

namespace Anvelop.Core.Loadable
{
	public class LoadableResource<T> where T : Object
	{
		private readonly string _id;

		public LoadableResource(string id)
		{
			_id = id;
		}

		private T _local;

		public T Load()
		{
			return _local ?? (_local = Resources.Load(_id, typeof(T)) as T);
		}
	}
}