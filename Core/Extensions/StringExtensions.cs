using System;
using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string str)
            => Encoding.Unicode.GetBytes(str);
        
        public static string Reverse(this string s)
        {
            var chars = s.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }
}
