using UnityEngine;

namespace Anvelop.Core.Helpers
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}