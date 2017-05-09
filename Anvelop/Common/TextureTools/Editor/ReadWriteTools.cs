using System.Text;
using UnityEditor;

namespace Anvelop.TextureTools.Editor
{
	public class ReadWriteTools
	{
		public static void SetReadWhriteState(bool state)
		{
			Core.DoWithTexture(new StringBuilder("Setting ReadWrite state to ").Append(state.ToString()).ToString(),
				delegate(TextureImporter importer) { importer.WriteReadable(state); });
		}

		[MenuItem(Core.Path + "Disable ReadWrite _&%W", priority = 3)]
		public static void DisableReadWrite()
		{
			SetReadWhriteState(false);
		}

		[MenuItem(Core.Path + "Enable ReadWrite", priority = 3)]
		public static void EnableReadWrite()
		{
			SetReadWhriteState(true);
		}
	}
}