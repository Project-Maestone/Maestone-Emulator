using System;

namespace DevServer
{
    public static class Util
    {
        private static readonly Random Random = new Random();

        public static int GetRandom(int maxValue)
        {
            return Random.Next(maxValue);
        }

        public static short GetRandomShort(int maxValue)
        {
            return Convert.ToInt16(Random.Next(maxValue));
        }

        public static byte GetRandomByte(int maxValue)
        {
            return Convert.ToByte(Random.Next(maxValue));
        }

        public static bool GetRandomBool()
        {
            return Convert.ToBoolean(Random.Next(2));
        }

        public static int GetRandom(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        public static short GetRandomShort(int minValue, int maxValue)
        {
            return Convert.ToInt16(Random.Next(minValue, maxValue));
        }

        public static byte GetRandomByte(int minValue, int maxValue)
        {
            return Convert.ToByte(Random.Next(minValue, maxValue));
        }

        public static T GetRandom<T>()
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Next(values.Length));
        }

        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Next(array.Length)];
        }
    }
}