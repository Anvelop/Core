#if UNITY_ANDROID && !UNITY_EDITOR
using System.Text;
#endif
#if UNITY_EDITOR
using System.IO;
#endif
using UnityEngine;

namespace Anvelop.Core.Crossplatform
{
	public static class CrossplatformStreamingAssetsPath
	{
		public static string Path
		{
			get
			{
#if UNITY_ANDROID && !UNITY_EDITOR
				return new StringBuilder("jar:file://").Append(Application.dataPath).Append("!/assets").ToString();
#else

#if UNITY_EDITOR
				CheckStreamingAssetsPath();
#endif

				return Application.streamingAssetsPath;
#endif
			}
		}

#if UNITY_EDITOR
		private static void CheckStreamingAssetsPath()
		{
			Directory.CreateDirectory(Application.streamingAssetsPath);
		}
#endif
	}
}