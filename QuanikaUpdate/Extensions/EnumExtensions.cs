using System;
using System.Collections.Generic;

namespace QuanikaUpdate.Extensions
{
    internal static class EnumExtensions
    {
        public static List<TEnum> ToList<TEnum>() where TEnum : struct, Enum
        {
            Type enumType = typeof(TEnum);
            if (enumType.IsEnum)
            {
                return new List<TEnum>((TEnum[])Enum.GetValues(enumType));
            }
            else
            {
                throw new ArgumentException("Type is not an enum.");
            }
        }
    }
}
