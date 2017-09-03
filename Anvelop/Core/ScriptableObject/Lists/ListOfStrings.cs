using UnityEngine;

namespace Anvelop.Core.ScriptableObject
{
	[CreateAssetMenu(fileName = DefaultPrefix + "Strings", menuName = Constants.EntitiesMenuPath + "Strings", order = 0)]
	class ListOfStrings : ListOf<string>
	{
		public const string DefaultPrefix = "[Strings] ";

		protected override string GetDefaultPrefix
		{
			get { return DefaultPrefix; }
		}
	}
}