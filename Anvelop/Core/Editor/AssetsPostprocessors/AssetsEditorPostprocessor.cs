using Anvelop.Core.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Anvelop.Core.Editor.AssetsPostprocessors
{
	public class AssetsEditorPostprocessor : AssetPostprocessor
	{
		private static void OnPostprocessAllAssets(
			string[] importedAssets,
			string[] deletedAssets,
			string[] movedAssets,
			string[] movedFromAssetPaths)
		{
			foreach (string movedAsset in movedAssets)
			{
				Object[] assets = AssetDatabase.LoadAllAssetsAtPath(movedAsset);

				foreach (Object asset in assets)
				{
					CheckValidatable(asset);
				}
			}
		}

		private static void CheckValidatable(Object asset)
		{
			IValidatable validatable = asset as IValidatable;
			if (validatable != null)
			{
				validatable.OnValidate();
			}
		}
	}
}