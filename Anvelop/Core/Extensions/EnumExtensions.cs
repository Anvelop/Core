using System;

namespace Anvelop.Core.Extensions
{
    public partial class EnumExtensions
    {
    }

    public static partial class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof (T).IsEnum)
                throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof (T).FullName));

            T[] Arr = (T[]) Enum.GetValues(src.GetType());
            int j = Array.IndexOf(Arr, src) + 1;
            return Arr.Length == j ? Arr[0] : Arr[j];
        }
    }
}