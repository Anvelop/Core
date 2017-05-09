using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Anvelop.TextureTools.Editor
{
	public class MuiltipleOf4Tools
	{
		[MenuItem(Core.Path + "Force To Multiple Of 4 _&%F", priority = 2)]
		public static void ForceToMultipleOf4()
		{
			Core.DoWithTexture("Forcing to multiple of 4", delegate(TextureImporter importer)
			{
				if (!string.IsNullOrEmpty(importer.Importer.spritePackingTag)) return;

				if (IsMultipleOf4(importer.Texture.width) && IsMultipleOf4(importer.Texture.height)) return;

				importer.WriteReadable(true);

				Fix(importer.Texture);

				importer.WriteReadable(false);
			});
		}

		private static void Fix(Texture2D texture)
		{
			var nextWidth = NextMultipleOf4(texture.width);
			var nextHeight = NextMultipleOf4(texture.height);

			var addWidth = nextWidth - texture.width;
			var addHeight = nextHeight - texture.height;

			var resized = new Texture2D(nextWidth, nextHeight, texture.format, texture.mipmapCount > 0);

			for (var i = 0; i < nextWidth; i++)
			for (var j = 0; j < nextHeight; j++)
				resized.SetPixel(i, j, Color.clear);

			resized.SetPixels(Mathf.RoundToInt(addWidth / 2f), Mathf.RoundToInt(addHeight / 2f), texture.width, texture.height,
				texture.GetPixels());
			resized.Apply();

			var pngData = resized.EncodeToPNG();
			if (pngData != null)
				File.WriteAllBytes(AssetDatabase.GetAssetPath(texture), pngData);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private static int NextMultipleOf4(float value)
		{
			var source = 1;
			while (true)
			{
				var target = source * 4;
				if (target >= value)
					return target;

				source++;
			}
		}

		private static bool IsMultipleOf4(float value)
		{
			return Math.Abs(value % 4) < 0f;
		}
	}
}