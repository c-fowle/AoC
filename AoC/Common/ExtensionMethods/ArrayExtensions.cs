namespace AoC.Common.ExtensionMethods
{
    public static class ArrayExtensions
    {
        public static T[] Populate<T>(this T[] array, T value)
        {
            for (var i = 0; i < array.Length; ++i) array[i] = value;
            return array;
        }
    }
}
