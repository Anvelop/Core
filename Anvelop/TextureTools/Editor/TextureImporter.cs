using UnityEditor;
using UnityEngine;

namespace Anvelop.TextureTools.Editor
{
	public class TextureImporter
	{
		public TextureImporter(Texture2D tex)
		{
			Texture = tex;
			Importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as UnityEditor.TextureImporter;
		}

		public UnityEditor.TextureImporter Importer { get; private set; }

		public Texture2D Texture { get; private set; }

		public bool Valid()
		{
			return Importer != null;
		}

		public void WriteReadable(bool state)
		{
			if (Importer.isReadable == state) return;

			Importer.isReadable = state;
			Importer.SaveAndReimport();
		}

		public void WriteMipMaps(bool state)
		{
			if (Importer.mipmapEnabled == state) return;

			Importer.mipmapEnabled = state;
			Importer.SaveAndReimport();
		}
	}
}