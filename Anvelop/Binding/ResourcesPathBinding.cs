﻿using System;
using System.Text;
using Anvelop.Common.ScriptableObject;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvelop.Common.Binding
{
	[CreateAssetMenu(fileName = "[Binding] ResourcesPathBinding", menuName = Constants.EntitiesMenuPath + "ResourcesPathBinding",
		order = 0)]
	public class ResourcesPathBinding : SelectableScriptableObject
	{
#if ADVANCED_INSPECTOR
		[AdvancedInspector.ReadOnly]
#endif
		public string Path;

#if ADVANCED_INSPECTOR
		[AdvancedInspector.ReadOnly]
#endif
		public string RuledPath;

		public string SubRule;

#if UNITY_EDITOR

		protected override void OnValidate()
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
		}

#endif

		public string GetRuledPathWithParams(object p1)
		{
			return new StringBuilder().AppendFormat(RuledPath, p1).ToString();
		}

		public string GetRuledPathWithParams(params object[] array)
		{
			return new StringBuilder().AppendFormat(RuledPath, array).ToString();
		}
	}
}