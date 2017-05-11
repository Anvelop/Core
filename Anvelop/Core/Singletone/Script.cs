using System;
using System.Collections;
using UnityEngine;

namespace Anvelop.Core.Singletone
{
	public class Script : MonoBehaviour
	{
		protected virtual void RunDeferred(float wait, Action callback)
		{
			StartCoroutine(Wait(wait, callback));
		}

		protected void RunDeferred(Func<bool> exitCondition, Action callback)
		{
			StartCoroutine(Wait(exitCondition, callback));
		}


		protected void RunAtEndOfFrame(Action callback)
		{
			StartCoroutine(WaitEndOfFrame(callback));
		}

		protected void ExecuteUntil(Action callback, float exitTime)
		{
			StartCoroutine(ExecuteUntilCoroutine(callback, exitTime));
		}

		protected void ExecuteUntilIsActive(Action callback, GameObject go)
		{
			StartCoroutine(ExecuteUntilIsActiveCoroutine(callback, go));
		}

		protected IEnumerator GetRunDeferred(Func<bool> exitCondition, Action callback)
		{
			IEnumerator temp = Wait(exitCondition, callback);

			StartCoroutine(temp);
			return temp;
		}

		//--//

		IEnumerator Wait(float wait, Action callback)
		{
			while (wait > 0f)
			{
				wait -= Time.deltaTime;
				yield return null;
			}

			yield return new WaitForEndOfFrame();

			callback();
		}

		IEnumerator Wait(Func<bool> exitCondition, Action callback)
		{
			while (!exitCondition())
			{
				yield return null;
			}

			callback();
		}

		IEnumerator Wait(Action callback)
		{
			yield return null;
			yield return new WaitForEndOfFrame();

			callback();
		}

		IEnumerator WaitEndOfFrame(Action callback)
		{
			yield return new WaitForEndOfFrame();

			callback();
		}

		IEnumerator ExecuteUntilCoroutine(Action callback, float exitTime)
		{
			while (exitTime > 0)
			{
				yield return new WaitForEndOfFrame();
				callback();
				exitTime -= Time.deltaTime;
			}
		}

		IEnumerator ExecuteUntilIsActiveCoroutine(Action callback, GameObject go)
		{
			while (go.activeSelf)
			{
				yield return new WaitForEndOfFrame();
				callback();
			}
		}
	}
}