#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Anvelop.Common.ScriptableObject
{
	public class SelectableScriptableObject : UnityEngine.ScriptableObject
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

		protected virtual void OnValidate()
		{
		}
	}
}