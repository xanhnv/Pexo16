namespace Pexo16
{
    class Simulate
    {
        public static string RSet(string source, int length)
        {
            if (source == null)
                return string.Empty.PadLeft(length);
            else if (length < source.Length)
                return source.Substring(0, length);
            else
                return source.PadLeft(length);
        }


        public static string LSet(string source, int length)
        {
            if (source == null)
                return string.Empty.PadRight(length);
            else if (length < source.Length)
                return source.Substring(0, length);
            else
                return source.PadRight(length);
        }


        public static char asciiTochar(byte[] a, int pos)
        {
            char[] characters = System.Text.Encoding.ASCII.GetChars(a);
            char c = characters[pos];
            return c;
        }
    }
}
