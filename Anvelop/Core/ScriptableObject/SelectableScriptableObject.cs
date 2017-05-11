
using Anvelop.Core.Interfaces;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Anvelop.Core.ScriptableObject
{
	public class SelectableScriptableObject : UnityEngine.ScriptableObject, IValidatable
	{
#if UNITY_EDITOR
#if ADVANCED_INSPECTOR
		[AdvancedInspector.Inspect]
#endif
		[ContextMenu("SelectThisFile")]
		private void SelectThisFile()
		{
			Selection.activeObject = this;
			EditorGUIUtility.PingObject(this);
		}
#endif

		public virtual void OnValidate()
		{
		}
	}
}