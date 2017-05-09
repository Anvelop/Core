using System.Text;
using UnityEditor;

namespace Anvelop.TextureTools.Editor
{
	public class MipMapsTools
	{
		[MenuItem(Core.Path + "Disable MipMaps _&%M", priority = 1)]
		public static void DisableMipMaps()
		{
			SetMipMapsState(false);
		}

		[MenuItem(Core.Path + "Enable MipMaps", priority = 1)]
		public static void EnableMipMaps()
		{
			SetMipMapsState(true);
		}

		public static void SetMipMapsState(bool state)
		{
			Core.DoWithTexture(new StringBuilder("Setting mipmaps state to ").Append(state.ToString()).ToString(),
				delegate(TextureImporter importer) { importer.WriteMipMaps(false); });
		}
	}
}