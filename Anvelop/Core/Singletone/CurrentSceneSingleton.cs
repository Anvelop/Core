using UnityEngine.SceneManagement;

namespace Anvelop.Core.Singletone
{
	public abstract class CurrentSceneSingleton<T> : SSingleton<T> where T : CurrentSceneSingleton<T>
	{
		private void Awake()
		{
			SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
		}

		private void SceneManagerOnSceneUnloaded(Scene scene)
		{
			if (gameObject.scene != scene)
			{
				Destroy(this);
				SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
			}
		}
	}
}