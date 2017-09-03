using UnityEngine;

namespace Anvelop.Core.ScriptableObject.Complex
{
	[CreateAssetMenu(fileName = DefaultPrefix + "Strings", menuName = Constants.EntitiesMenuPath + "Complex of Strings", order = 1)]
	class ComplexOfStrings : ListOf<ListOfStrings>
	{
		public const string DefaultPrefix = "[Complex][Strings] ";

		protected override string GetDefaultPrefix
		{
			get { return DefaultPrefix; }
		}
	}
}