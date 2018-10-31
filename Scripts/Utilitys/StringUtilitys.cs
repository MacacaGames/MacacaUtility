using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMacaca
{
    public static class StringUtilitys
    {
        public static string GetRandomString(int length)
        {
            string result = Guid.NewGuid().ToString().Replace("-", "");
            while(result.Length < length){
                result += Guid.NewGuid().ToString().Replace("-", "");
            }
            return result.Substring(0, length);
        }

        public static string Random(string chars, int length = 8)
        {
            var randomString = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
                randomString.Append(chars[random.Next(chars.Length)]);

            return randomString.ToString();
        }
    }


}
