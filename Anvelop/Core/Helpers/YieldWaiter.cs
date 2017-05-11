using System.Collections;
using System.Collections.Generic;

namespace Anvelop.Core.Helpers
{
	public class YieldWaiter
	{
		private int _count;

		public YieldWaiter()
		{
		}

		public YieldWaiter(int count)
		{
			Set(count);
		}

		public void Increase(int count = 1)
		{
			_count += count;
		}

		public void Set(int count)
		{
			_count = count;
		}

		public void Descrease()
		{
			_count--;
		}

		public IEnumerator<float> WaitAsFloat()
		{
			while (_count > 0)
			{
				yield return 0f;
			}
		}

		public IEnumerator Wait()
		{
			while (_count > 0)
			{
				yield return null;
			}
		}
	}
}