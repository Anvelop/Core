using UnityEngine;

namespace Anvelop.Core.ScriptableObject
{
	public abstract class WithDefaultPrefix : SelectableScriptableObject
	{
		protected abstract string GetDefaultPrefix { get; }

		public override void OnValidate()
		{
			base.OnValidate();

			CheckDefaultPrefix();
		}

		private void CheckDefaultPrefix()
		{
			if (name.Contains(GetDefaultPrefix))
			{
				return;
			}

			Debug.LogAssertionFormat("{0} doesn`t contain prefix {1}", name, GetDefaultPrefix);
		}
	}
}