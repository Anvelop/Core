using System.Collections.Generic;

namespace Anvelop.Core.ScriptableObject
{
	internal abstract class ListOf<T> : WithDefaultPrefix
	{
		public List<T> List = new List<T>(); 
	}
}