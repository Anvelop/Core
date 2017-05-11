using System.IO;
using UnityEditor;
using UnityEngine;

namespace Anvelop.TextureTools.Editor
{
	public class MultipleOf4Tools
	{
		[MenuItem(Core.Path + "Force To Multiple Of 4 _&%F", priority = 2)]
		public static void ForceToMultipleOf4()
		{
			Core.DoWithTexture("Forcing to multiple of 4", delegate(TextureImporter importer)
			{
				if (!string.IsNullOrEmpty(importer.Importer.spritePackingTag))
				{
					Debug.LogAssertionFormat("Texture {0} contains packing tag. Ignored", importer.Texture.name);

					return;
				}

				if (IsMultipleOf4(importer.Texture.width) && IsMultipleOf4(importer.Texture.height)) return;

				importer.WriteReadable(true);

				Fix(importer.Texture);

				importer.WriteReadable(false);
			});
		}

		private static void Fix(Texture2D texture)
		{
			int nextWidth = NextMultipleOf4(texture.width);
			int nextHeight = NextMultipleOf4(texture.height);

			int addWidth = nextWidth - texture.width;
			int addHeight = nextHeight - texture.height;

			Texture2D resized = new Texture2D(nextWidth, nextHeight, texture.format, texture.mipmapCount > 0);

			for (int i = 0; i < nextWidth; i++)
				for (int j = 0; j < nextHeight; j++)
					resized.SetPixel(i, j, Color.clear);

			resized.SetPixels(Mathf.RoundToInt(addWidth/2f), Mathf.RoundToInt(addHeight/2f), texture.width, texture.height,
				texture.GetPixels());
			resized.Apply();

			byte[] pngData = resized.EncodeToPNG();
			if (pngData != null)
				File.WriteAllBytes(AssetDatabase.GetAssetPath(texture), pngData);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private static int NextMultipleOf4(float value)
		{
			return ((int) value/4 + 1)*4;
		}

		private static bool IsMultipleOf4(float value)
		{
			return value%4 <= 0f;
		}
	}
}