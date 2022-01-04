using System;

namespace Extension
{
    public static class StringExtenions
    {
        public static string ZeroTerminate(this string s)
        {
            return s.Remove(s.IndexOf('\0'));
        }
    }
}
