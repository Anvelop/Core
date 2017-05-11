using System;
using System.Text;
using Anvelop.Core.ScriptableObject;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Anvelop.Core.Binding
{
	[CreateAssetMenu(fileName = DefaultPrefix + "ResourcesPathBinding",
		menuName = Constants.EntitiesMenuPath + "ResourcesPathBinding",
		order = 0)]
	public class ResourcesPathBinding : SelectableScriptableObject
	{
		public const string DefaultPrefix = "[Binding] ";

#if ADVANCED_INSPECTOR
		[AdvancedInspector.ReadOnly]
#endif
		[Header("Auto (Read-Only)")]
		public string Path;

#if ADVANCED_INSPECTOR
		[AdvancedInspector.ReadOnly]
#endif
			public string RuledPath;

		[Header("Manual")]
		public string SubRule;

		public string GetRuledPathWithParams(object p1)
		{
			return new StringBuilder().AppendFormat(RuledPath, p1).ToString();
		}

		public string GetRuledPathWithParams(params object[] array)
		{
			return new StringBuilder().AppendFormat(RuledPath, array).ToString();
		}

#if UNITY_EDITOR
		public override void OnValidate()
		{
			string path = AssetDatabase.GetAssetPath(this);

			if (path.Contains(Constants.ResourcesFolderName))
			{
				int index = path.LastIndexOf(Constants.ResourcesFolderName, StringComparison.Ordinal);

				path = path.Substring(index + Constants.ResourcesFolderName.Length);

				path = path.Replace(name + Constants.AssetSubName, string.Empty);
			}

			Path = path;

			RuledPath = new StringBuilder(Path).Append(SubRule).ToString();

			CheckDefaultPrefix();
		}

		private void CheckDefaultPrefix()
		{
			if (name.Contains(DefaultPrefix))
			{
				return;
			}

			Debug.LogAssertionFormat("{0} doesn`t contain prefix {1}", name, DefaultPrefix);
		}
#endif
	}
}