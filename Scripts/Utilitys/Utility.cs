using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CloudMacaca
{

    public class Utility
    {
        /// <summary>
        /// 取得 1970/01/01 至現在時間的總秒數
        /// </summary>

        [System.Obsolete("Please consider don't use CloudMacaca.Utility.GetTimeStamp() directly, use CloudMacaca.GlobalTimer.currentTimeStamp instead, since CloudMacaca.GlobalTimer.currentTimeStamp will cache timestamp value every frame.")]
        public static int GetTimeStamp()
        {
            DateTime gtm = new DateTime(1970, 1, 1); //宣告一個GTM時間出來
            DateTime utc = DateTime.UtcNow.AddHours(8); //宣告一個目前的時間
            return Convert.ToInt32(((TimeSpan)utc.Subtract(gtm)).TotalSeconds);
        }

        public static DateTime GetDateTimeFromTimeStamp(int timestamp)
        {
            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(timestamp);
            return dt;
        }

        /// <summary>
        /// 以 TypeName 尋找對應 Type UnityEngine 專用
        /// </summary>
        public static Type GetType(string TypeName)
        {
            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, in the same assembly as the caller, etc.
            var type = Type.GetType(TypeName);

            // If it worked, then we're done here
            if (type != null)
                return type;


            // If the TypeName is a full name, then we can try loading the defining assembly directly
            if (TypeName.Contains("."))
            {
                // Get the name of the assembly (Assumption is that we are using 
                // fully-qualified type names)
                var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

                type = Type.GetType(TypeName + "," + assemblyName);
                if (type != null)
                    return type;

                // Attempt to load the indicated Assembly
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    goto LAST_WAY;
                }
                if (assembly == null)
                    goto LAST_WAY;

                // Ask that assembly to return the proper Type
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;

            }

        LAST_WAY:
            //For WebPlayer use this
            var currentAssembly = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in currentAssembly)
            {
                // Load the referenced assembly
                if (assembly != null)
                {
                    // See if that assembly defines the named type
                    type = assembly.GetType(TypeName);
                    if (type != null)
                        return type;
                }
            }

            // The type just couldn't be found...
            return null;
        }

        //Base64 編碼
        public static string Base64Encode(string source)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(source));
        }

        //Base64 解碼
        public static string Base64Decode(string base64Str)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
        }

        //Convert to Utf8 String
        public static string BytesToString(byte[] bytes)
        {
            try
            {
                return System.Text.UTF8Encoding.Default.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }
        //Convert to Utf8 Bytes
        public static byte[] StringToBytes(string source)
        {
            return System.Text.UTF8Encoding.Default.GetBytes(source);
        }
    }

}