using System;
using System.Collections.Generic;

namespace LAIeRS.ExtensiveMethods
{
    public static class GeneralExtensiveMethodsCollection
    {
        public static bool IsGreaterThan(this int a, int b)
            => a > b;
        public static bool IsGreaterOrEqual(this int a, int b)
            => a >= b;
        public static bool IsLessThan(this int a, int b)
            => a < b;
        public static bool IsLessOrEqual(this int a, int b)
            => a <= b;
        
        public static bool IsNotEqual<T>(this T a, T b) 
            => !a.Equals(b);

        // TODO: Try out to create the randomizer instance inside this function
        public static T GetRandomItem<T>(this IList<T> itemList, Random randomizer)
            => itemList[randomizer.Next(0, itemList.Count)];

        public static bool NotContains<T>(this IList<T> itemList, T item)
            => !itemList.Contains(item);
    }
}