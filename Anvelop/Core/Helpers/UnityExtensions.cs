using System;
using System.Collections.Generic;
using Anvelop.Core.Extensions;
using UnityEngine;
using BaseObject = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Anvelop.Core.Helpers
{
	public static class UnityExtensions
	{
		public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> condition)
		{
			int index = -1;
			int counter = 0;

			foreach (var item in collection)
			{
				if (condition(item))
				{
					index = counter;
					break;
				}

				counter++;
			}

			return index;
		}

		public static void Destroy(this BaseObject obj, float time)
		{
			BaseObject.Destroy(obj, time);
		}

		public static void Destroy(this BaseObject obj)
		{
			BaseObject.Destroy(obj);
		}

		public static List<Transform> GetAllChildrenList(this Transform root)
		{
			var totalList = new List<Transform>();
			var parentList = new List<Transform>();
			var childList = new List<Transform>();

			parentList.Add(root);

			while (parentList.Count > 0)
			{
				for (int i = 0; i < parentList.Count; i++)
				{
					foreach (Transform child in parentList[i])
					{
						childList.Add(child);
					}
				}

				totalList.AddRange(childList);

				var tempList = parentList;
				parentList = childList;
				childList = tempList;

				childList.Clear();
			}

			return totalList;
		}

		public static Transform[] GetAllChildren(this Transform root)
		{
			return root.GetAllChildrenList().ToArray();
		}

		public static Transform[] GetTopChildren(this Transform root)
		{
			Transform[] children = new Transform[root.childCount];

			int i = 0;
			foreach (Transform child in root)
			{
				children[i] = child;
				i++;
			}

			return children;
		}

		public static Sprite ToSprite(this Texture2D texture, Vector2 pivot)
		{
			if (!texture)
			{
				Debug.LogWarning("Texture to sprite failed because of texture is null");
				return null;
			}
			return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), pivot);
		}

		public static void Ignore<T>(this List<T> list, List<T> ignore)
		{
			if (list == null)
				return;

			list.RemoveAll(ignore.Contains);
		}

		public static void Shuffle<T>(this List<T> list)
		{
			if (list == null)
				return;

			for (int i = 0; i < list.Count; i++)
			{
				var index = Random.Range(0, list.Count);
				var obj = list[i];
				list[i] = list[index];
				list[index] = obj;
			}
		}

		public static bool Replace<T>(this List<T> list, T obj, Predicate<T> predicate)
		{
			if (list == null)
				return false;

			int index = list.IndexOf(arg1 => (predicate(arg1)));

			if (index < 0) return false;

			list.RemoveAt(index);

			list.Insert(index, obj);

			return true;
		}

		public static void AddToRandomIndex<T>(this List<T> list, T value)
		{
			if (list == null)
				return;

			if (list.Count == 0)
			{
				list.Add(value);
				return;
			}

			var targetIndex = Random.Range(0, list.Count);

			list.Add(value);

			for (int i = list.Count - 1; i > targetIndex; i--)
			{
				list[i] = list[i - 1];
			}

			list[targetIndex] = value;
		}

		public static float GetAnimationClipLength(this Animator animator, string clipName)
		{
			AnimationClip clip =
				animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name.Equals(clipName));

			if (clip == null)
			{
				Debug.LogError("Cant find clip with state : " + clipName);
				return 0;
			}

			return clip.length;
		}

		public static float GetAnimationClipLength(this Animator animator, int clipNumber)
		{
			AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

			if (clips.Length < clipNumber)
			{
				Debug.LogError("Сant find clip with number : " + clipNumber);
			}

			return clips[clipNumber].length;
		}

		public static void SetParentAndReset(this Transform trans, Transform parent)
		{
			trans.SetParent(parent);
			trans.Reset();
		}

		public static void SetParentAndReset(this RectTransform trans, Transform parent)
		{
			trans.SetParent(parent);
			trans.Reset();
		}

		public static void SetParentAndResetZ(this Transform trans, Transform parent)
		{
			trans.SetParent(parent);
			trans.ResetLocalZ();
		}

		public static void Reset(this Transform trans)
		{
			trans.localPosition = Vector3.zero;
			trans.localScale = Vector3.one;
			trans.localRotation = Quaternion.identity;
		}

		public static void Reset(this RectTransform trans)
		{
			trans.anchoredPosition = Vector3.zero;
			trans.localScale = Vector3.one;
			trans.localRotation = Quaternion.identity;
		}

		public static void SetZ(this Transform trans, float z)
		{
			trans.position = new Vector3(trans.position.x, trans.position.y, z);
		}

		public static void SetLocalZ(this Transform trans, float z)
		{
			trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, z);
		}

		public static void ResetLocalZ(this Transform trans)
		{
			trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, 0f);
		}

		public static RectTransform GetRectTransform(this GameObject go)
		{
			return go.GetComponent<RectTransform>();
		}

		public static void SafeDestroy(this GameObject go)
		{
#if UNITY_EDITOR
			if (Application.isPlaying)
			{
				GameObject.Destroy(go);
				return;
			}

			UnityEditor.EditorApplication.delayCall += delegate { BaseObject.DestroyImmediate(go, true); };
#else
		GameObject.Destroy(go);
#endif
		}
	}
}