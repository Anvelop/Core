using System;
using System.Collections.Generic;
using Full.Scripts.Helpers;
using UnityEngine;
using Object = UnityEngine.Object;

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

		private T Local
		{
			get { return _local; }
			set
			{
				ValidateSource(value);

				_local = value;
			}
		}

		public T Load()
		{
			return Local ?? (Local = Resources.Load(_id, typeof(T)) as T);
		}

		private void ValidateSource(object source)
		{
			if (source != null) return;

			Debug.LogErrorFormat("Loading failed at path: {0}", _id);
		}

		private List<Action<T>> _callbacks;

		private ResourceRequest _resourceRequest;

		public void LoadAsync(Action<T> callback)
		{
			if (Local != null)
			{
				callback(Local);
				return;
			}

			if (_callbacks == null)
			{
				_callbacks = new List<Action<T>>();
			}

			_callbacks.Add(callback);

			if (_resourceRequest != null) return;

			_resourceRequest = Resources.LoadAsync<T>(_id);

			DeferredRun.AsyncLoadResource(_resourceRequest, () =>
			{
				Local = _resourceRequest.asset as T;

				foreach (var action in _callbacks)
				{
					action(Local);
				}

				_callbacks = null;
				_resourceRequest = null;
			});
		}

		public void Unload()
		{
			if (Local == null) return;

			if (Local is GameObject || Local is Component || Local is AssetBundle)
			{
				return;
			}


			Resources.UnloadAsset(Local);
		}
	}
}