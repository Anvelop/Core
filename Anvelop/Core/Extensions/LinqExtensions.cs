using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Anvelop.Core.Extensions
{
	public static class LinqExtensions
	{
		private const int MaxBubbleSortSize = 100;

		public static IEnumerable<TR> Select<T, TR>(this IEnumerable<T> self, Func<T, TR> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			foreach (T x in self)
				yield return func(x);
		}

		public static IEnumerable<TR> Select<T, TR>(this IEnumerable<T> self, Func<T, int, TR> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			int index = 0;
			foreach (T x in self)
				yield return func(x, index++);
		}

		public static IEnumerable<TR> SelectMany<T, TR>(this IEnumerable<T> self, Func<T, IEnumerable<TR>> selector)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			foreach (T x in self)
			{
				foreach (TR y in selector(x))
					yield return y;
			}
		}

		public static IEnumerable<T> Where<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			foreach (T x in self)
				if (func(x))
					yield return x;
		}

		public static IEnumerable<T> Where<T>(this IEnumerable<T> self, Func<T, int, bool> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			int index = 0;
			foreach (T x in self)
				if (func(x, index++))
					yield return x;
		}

		public static IEnumerable<T> OfType<T>(this IEnumerable self)
			where T : class
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			foreach (object x in self)
				if (x is T)
					yield return x as T;
		}

		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
		{
			if (source == null)
			{
				Debug.LogError("Enumerable is null.");
				return Empty<TResult>();
			}

			IEnumerable<TResult> enumerable = source as IEnumerable<TResult>;
			if (enumerable != null)
				return enumerable;
			return CastIterator<TResult>(source);
		}

		private static IEnumerable<TResult> CastIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
				yield return (TResult) obj;
		}

		public static void Do<T>(this IEnumerable<T> self, Action<T> action)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return;
			}

			foreach (T x in self)
				action(x);
		}

		public static void Do<T>(this IEnumerable<T> self, Action<T, int> action)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return;
			}

			int i = 0;
			foreach (T x in self)
				action(x, i++);
		}

		public static IEnumerable<T> DoLazy<T>(this IEnumerable<T> self, Action<T> action)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			foreach (T x in self)
			{
				action(x);
				yield return x;
			}
		}

		public static int Count<T>(this IEnumerable<T> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return 0;
			}

			if (self is ICollection<T>)
				return (self as ICollection<T>).Count;

			int count = 0;
#pragma warning disable 168
			foreach (T x in self)
#pragma warning restore 168
				count++;
			return count;
		}

		public static int Count<T>(this IEnumerable<T> self, Func<T, bool> checker)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return 0;
			}

			int count = 0;
			foreach (T x in self)
				if (checker(x))
					count++;
			return count;
		}

		public static T[] ToArray<T>(this IEnumerable<T> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return null;
			}

			if (self is ICollection<T>)
			{
				ICollection<T> clct = self as ICollection<T>;
				T[] arr = new T[clct.Count];
				clct.CopyTo(arr, 0);
				return arr;
			}

			T[] result = new T[self.Count()];
			int index = 0;
			foreach (T x in self)
				result[index++] = x;
			return result;
		}

		public static List<T> ToList<T>(this IEnumerable<T> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return null;
			}

			return new List<T>(self);
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue, T>(this IEnumerable<T> self, Func<T, TKey> key,
			Func<T, TValue> value)
		{
			Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
			foreach (T x in self)
				dict.Add(key(x), value(x));
			return dict;
		}

		public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> self, Func<T, bool> predicate)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			foreach (T x in self)
			{
				if (!predicate(x))
					break;
				yield return x;
			}
		}

		public static IEnumerable<T> Take<T>(this IEnumerable<T> self, int n)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			int i = 0;
			foreach (T x in self)
			{
				if (i++ == n)
					break;
				yield return x;
			}
		}

		public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> self, Func<T, bool> predicate)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				yield break;
			}

			using (IEnumerator<T> enumerator = self.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					yield break;

				while (predicate(enumerator.Current) && enumerator.MoveNext())
				{
				}

				while (enumerator.MoveNext())
					yield return enumerator.Current;
			}
		}

		public static IEnumerable<T> Skip<T>(this IEnumerable<T> self, int n)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return self;
			}

			return n == 0 ? self : SkipIterator(self, n);
		}

		private static IEnumerable<T> SkipIterator<T>(this IEnumerable<T> self, int n)
		{
			int i = 1;
			using (IEnumerator<T> enumerator = self.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					yield break;

				while (i++ < n && enumerator.MoveNext())
				{
				}
				while (enumerator.MoveNext())
					yield return enumerator.Current;
			}
		}

		public static bool Contains<T>(this IEnumerable<T> self, T value)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return false;
			}

			foreach (T x in self)
			{
				if (Equals(x, value))
					return true;
			}
			return false;
		}


		public static T Aggregate<T>(this IEnumerable<T> self, Func<T, T, T> aggregator)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			using (IEnumerator<T> enumerator = self.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					return default(T);

				T value = enumerator.Current;
				while (enumerator.MoveNext())
					value = aggregator(value, enumerator.Current);
				return value;
			}
		}

		public static bool All<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return false;
			}

			foreach (T x in self)
				if (!func(x))
					return false;
			return true;
		}

		public static bool Any<T>(this IEnumerable<T> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return false;
			}

#pragma warning disable 168
			foreach (T x in self)
#pragma warning restore 168
				return true;
			return false;
		}

		public static bool Any<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return false;
			}

			foreach (T x in self)
				if (func(x))
					return true;
			return false;
		}

		public static T Max<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			Comparer<T> @default = Comparer<T>.Default;
			T y = default(T);
			if (y == null)
			{
				foreach (T x in source)
				{
					if (x != null && (y == null || @default.Compare(x, y) > 0))
						y = x;
				}
				return y;
			}
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					return default(T);

				y = enumerator.Current;
				while (enumerator.MoveNext())
				{
					T x = enumerator.Current;
					if (@default.Compare(x, y) > 0)
						y = x;
				}
				return y;
			}
		}

		public static T Max<T, TR>(this IEnumerable<T> source, Func<T, TR> selector)
		{
			if (source == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			Comparer<TR> @default = Comparer<TR>.Default;
			T y = default(T);
			if (y == null)
			{
				foreach (T x in source)
				{
					if (x != null && (y == null || @default.Compare(selector(x), selector(y)) > 0))
						y = x;
				}
				return y;
			}
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					return default(T);

				y = enumerator.Current;
				while (enumerator.MoveNext())
				{
					T x = enumerator.Current;
					if (@default.Compare(selector(x), selector(y)) > 0)
						y = x;
				}
				return y;
			}
		}

		public static T Min<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			Comparer<T> @default = Comparer<T>.Default;
			T y = default(T);
			if (y == null)
			{
				foreach (T x in source)
				{
					if (x != null && (y == null || @default.Compare(x, y) < 0))
						y = x;
				}
				return y;
			}
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					return default(T);

				y = enumerator.Current;
				while (enumerator.MoveNext())
				{
					T x = enumerator.Current;
					if (@default.Compare(x, y) < 0)
						y = x;
				}
				return y;
			}
		}

		public static T Min<T, TR>(this IEnumerable<T> source, Func<T, TR> selector)
		{
			if (source == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			Comparer<TR> @default = Comparer<TR>.Default;
			T y = default(T);
			if (y == null)
			{
				foreach (T x in source)
				{
					if (x != null && (y == null || @default.Compare(selector(x), selector(y)) < 0))
						y = x;
				}
				return y;
			}
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					return default(T);

				y = enumerator.Current;
				while (enumerator.MoveNext())
				{
					T x = enumerator.Current;
					if (@default.Compare(selector(x), selector(y)) < 0)
						y = x;
				}
				return y;
			}
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			foreach (T x in self)
				return x;
			return default(T);
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			foreach (T x in self)
				if (func(x))
					return x;
			return default(T);
		}

		public static T First<T>(this IEnumerable<T> self)
		{
			return FirstOrDefault(self);
		}

		public static T First<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			return FirstOrDefault(self, func);
		}

		public static T LastOrDefault<T>(this IEnumerable<T> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			if (self is T[])
			{
				T[] arr = self as T[];
				return arr[arr.Length - 1];
			}
			if (self is List<T>)
			{
				List<T> lst = self as List<T>;
				return lst[lst.Count - 1];
			}

			T result = default(T);
			foreach (T x in self)
				result = x;
			return result;
		}

		public static T LastOrDefault<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return default(T);
			}

			if (self is T[])
			{
				T[] arr = self as T[];
				for (int i = arr.Length - 1; i >= 0; --i)
					if (func(arr[i]))
						return arr[i];
			}
			if (self is List<T>)
			{
				List<T> lst = self as List<T>;
				for (int i = lst.Count - 1; i >= 0; --i)
					if (func(lst[i]))
						return lst[i];
			}

			T result = default(T);
			foreach (T x in self)
				if (func(x))
					result = x;

			return result;
		}

		public static T Last<T>(this IEnumerable<T> self)
		{
			return LastOrDefault(self);
		}

		public static T Last<T>(this IEnumerable<T> self, Func<T, bool> func)
		{
			return LastOrDefault(self, func);
		}

		public static float Sum<T>(this IEnumerable<T> self, Func<T, float> summator)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return 0;
			}

			float result = 0;
			foreach (T x in self)
				result += summator(x);
			return result;
		}

		public static int Sum<T>(this IEnumerable<T> self, Func<T, int> summator)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return 0;
			}

			int result = 0;
			foreach (T x in self)
				result += summator(x);
			return result;
		}

		public static int Sum(this IEnumerable<int> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return 0;
			}

			int result = 0;
			foreach (int x in self)
				result += x;
			return result;
		}

		public static decimal Sum(this IEnumerable<decimal> self)
		{
			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return 0;
			}

			decimal result = 0;
			foreach (decimal x in self)
				result += x;
			return result;
		}

		public static IEnumerable<T> SortBy<T>(this IEnumerable<T> self, Func<T, T, bool> lessFunc, bool byHeap = false)
		{
			return SortInternal(self, lessFunc, false, byHeap);
		}

		public static IEnumerable<T> SortByDescending<T>(this IEnumerable<T> self, Func<T, T, bool> lessFunc,
			bool byHeap = false)
		{
			return SortInternal(self, lessFunc, true, byHeap);
		}

		private static IEnumerable<T> SortInternal<T>(IEnumerable<T> self, Func<T, T, bool> lessFunc, bool descending,
			bool heap)
		{
			Func<T, T, bool> func = descending ? (a, b) => !lessFunc(a, b) : lessFunc;

			if (heap)
				return new SortingHeap<T>(self, func);

			T[] sortInternal = self.ToArray();

			if (sortInternal.Length > MaxBubbleSortSize)
				QuickSort(sortInternal, 0, sortInternal.Length - 1, func);
			else
				BubbleSort(sortInternal, func);

			return sortInternal;
		}

		public static IEnumerable<T> Union<T>(this IEnumerable<T> self, IEnumerable<T> other)
		{
			foreach (T x in self)
				yield return x;
			foreach (T x in other)
				yield return x;
		}

		public static IEnumerable<T> Union<T>(this IEnumerable<T> self, T value)
		{
			foreach (T x in self)
				yield return x;
			yield return value;
		}

		public static IEnumerable<T> Reverse<T>(this IEnumerable<T> self)
		{
			List<T> list = self.ToList();
			list.Reverse();
			return list;
		}

		public static bool Intersaction<T>(this List<T> self, List<T> other)
		{
			if (self == null || other == null)
			{
				return false;
			}

			for (int i = 0; i < self.Count; i++)
			{
				T selfObject = self[i];
				for (int index = 0; index < other.Count; index++)
				{
					T otherObject = other[index];
					if (selfObject.Equals(otherObject))
					{
						return true;
					}
				}
			}

			return false;
		}

		public static IEnumerable<TR> Zip<T, TQ, TR>(this IEnumerable<T> self, IEnumerable<TQ> other,
			Func<T, TQ, TR> selector)
		{
			using (IEnumerator<T> selfEn = self.GetEnumerator())
			{
				using (IEnumerator<TQ> otherEn = other.GetEnumerator())
				{
					while (selfEn.MoveNext() && otherEn.MoveNext())
					{
						yield return selector(selfEn.Current, otherEn.Current);
					}
				}
			}
		}

		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer,
			IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
			Func<TOuter, TInner, TResult> resultSelector)
		{
			TInner[] innerArray = inner.ToArray();
			TOuter[] outerArray = outer.ToArray();

			for (int j = 0; j < outerArray.Length; ++j)
			{
				for (int i = 0; i < innerArray.Length; ++i)
				{
					if (innerKeySelector(innerArray[i]).Equals(outerKeySelector(outerArray[j])))
					{
						yield return resultSelector(outerArray[j], innerArray[i]);
					}
				}
			}
		}

		public static IEnumerable<KeyValuePair<TKey, IEnumerable<T>>> GroupBy<T, TKey>(this IEnumerable<T> self,
			Func<T, TKey> selector)
		{
			Dictionary<TKey, IEnumerable<T>> dict = new Dictionary<TKey, IEnumerable<T>>();

			if (self == null)
			{
				Debug.LogError("Enumerable is null.");
				return dict;
			}

			foreach (T item in self)
			{
				TKey key = selector(item);
				IEnumerable<T> values;
				if (!dict.TryGetValue(key, out values))
				{
					values = new List<T>();
					dict.Add(key, values);
				}
				List<T> list = values as List<T>;
				if (list != null)
					list.Add(item);
			}
			return dict;
		}

		public static IEnumerable<T> Empty<T>()
		{
			yield break;
		}

		public static IEnumerable<int> Range(int count)
		{
			for (int i = 0; i < count; ++i)
				yield return i;
		}

		public static T UnityRandomElement<T>(this IEnumerable<T> list)
		{
			if (list == null || list.Count() == 0)
			{
				return default(T);
			}
			return list.ElementAt(Random.Range(0, list.Count()));
		}

		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
		{
			if (source == null)
			{
				throw new NullReferenceException("source");
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return list[index];
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException("index");
			}
			using (IEnumerator<TSource> e = source.GetEnumerator())
			{
				while (true)
				{
					if (!e.MoveNext())
					{
						throw new IndexOutOfRangeException("index");
					}
					if (index == 0)
					{
						return e.Current;
					}
					index--;
				}
			}
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> knownKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (knownKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		private sealed class SortingPair<T, TKey> : IComparable
		{
			private readonly IComparer<TKey> comparer;
			private readonly TKey key;

			public SortingPair(TKey key, T value, IComparer<TKey> comparer)
			{
				Value = value;
				this.key = key;
				this.comparer = comparer;
			}

			public T Value { get; private set; }

			public int CompareTo(object obj)
			{
				return comparer.Compare(key, (obj as SortingPair<T, TKey>).key);
			}
		}

		#region Sort algorithms

		private static void QuickSort<T>(T[] elements, int left, int right, Func<T, T, bool> lessFunc)
		{
			int i = left, j = right;
			T pivot = elements[(left + right)/2];

			while (i <= j)
			{
				while (lessFunc(elements[i], pivot))
					i++;

				while (lessFunc(pivot, elements[j]))
					j--;

				if (i > j) continue;

				T tmp = elements[i];
				elements[i] = elements[j];
				elements[j] = tmp;

				i++;
				j--;
			}

			if (left < j)
				LinqExtensions.QuickSort(elements, left, j, lessFunc);

			if (i < right)
				LinqExtensions.QuickSort(elements, i, right, lessFunc);
		}

		private static void BubbleSort<T>(T[] elements, Func<T, T, bool> lessFunc)
		{
			int rightBorder = elements.Length - 1;
			do
			{
				int lastExchange = 0;

				for (int i = 0; i < rightBorder; i++)
				{
					if (lessFunc(elements[i + 1], elements[i]))
					{
						T tmp = elements[i];
						elements[i] = elements[i + 1];
						elements[i + 1] = tmp;

						lastExchange = i;
					}
				}

				rightBorder = lastExchange;
			} while (rightBorder > 0);
		}

		#endregion
	}

	public sealed class SortingHeap<T> : IEnumerable<T>
	{
		private readonly T[] elements;
		private readonly Func<T, T, bool> lessFunc;
		private int size;

		public SortingHeap(IEnumerable<T> data, Func<T, T, bool> lessFunc)
		{
			this.lessFunc = lessFunc;
			elements = data.ToArray();
			size = elements.Length;

			Heapify();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new SelfEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void SiftDown(int currentNode)
		{
			bool needSwap = true;
			while (needSwap)
			{
				int leftChild = 2*currentNode + 1,
					rightChild = 2*currentNode + 2,
					smallestChild = currentNode;

				if (leftChild < size && lessFunc(elements[leftChild], elements[smallestChild]))
				{
					smallestChild = leftChild;
				}

				if (rightChild < size && lessFunc(elements[rightChild], elements[smallestChild]))
				{
					smallestChild = rightChild;
				}

				needSwap = smallestChild != currentNode;

				if (needSwap)
				{
					T tmp = elements[currentNode];
					elements[currentNode] = elements[smallestChild];
					elements[smallestChild] = tmp;

					currentNode = smallestChild;
				}
			}
		}

		private void Heapify()
		{
			for (int i = size/2; i >= 0; --i)
				SiftDown(i);
		}

		private T Peek()
		{
			return elements[0];
		}

		private T Pop()
		{
			T data = Peek();

			T tmp = elements[0];
			elements[0] = elements[size - 1];
			elements[size - 1] = tmp;
			--size;

			SiftDown(0);
			return data;
		}

		private class SelfEnumerator : IEnumerator<T>
		{
			private readonly SortingHeap<T> heap;
			private bool firstStep;

			public SelfEnumerator(SortingHeap<T> heap)
			{
				this.heap = heap;
				firstStep = false;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (!firstStep)
					firstStep = true;
				else
					heap.Pop();
				return heap.size > 0;
			}

			public void Reset()
			{
				heap.size = heap.elements.Length;
				heap.Heapify();
			}

			public T Current
			{
				get { return heap.Peek(); }
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}
		}
	}
}