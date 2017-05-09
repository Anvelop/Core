using System;
using UnityEditor;
using UnityEngine;

namespace Anvelop.TextureTools.Editor
{
	public class Core
	{
		public const string Path = "Assets/Anvelop-Textures/";

		public static void DoWithTexture(string title, Action<TextureImporter> @do)
		{
			var textures = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);

			for (var i = 0; i < textures.Length; i++)
			{
				EditorUtility.DisplayProgressBar(title,
					"In progress ...", (float) i / textures.Length);

				var tex = textures[i];
				if (tex == null) continue;

				var importer = new TextureImporter(tex);
				if (!importer.Valid()) continue;

				@do(importer);
			}

			EditorUtility.ClearProgressBar();
		}
	}
}