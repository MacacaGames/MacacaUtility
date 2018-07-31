using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;

namespace CloudMacaca {

    public class Utility {
        public static string photoFilePath = "";
        /// <summary>
        /// 以攝影機正中心依長寬進行截圖
        /// 比 3:2 更寬的螢幕 例如 大部分的手機，以手機的寬度當作截圖寬
        /// 其他情況 例如 iPad 以其高度計算 16:9 狀態時應該的寬度，並設定為截圖寬
        /// </summary>
        /// <param name="screenShotName">檔案名稱</param>
        /// <param name="yOffect">調整高度正中間的偏移量，可以往上或往下調整，但若偏移量太大超出螢幕範圍時在部分平台可能會報錯</param>
        /// <param name="ratio">截圖的比例</param>

        public static IObservable<Texture2D> ScreenShot (string screenShotNam, int yOffect, float ratio = 1.45f, float delay = 0) {
            // convert coroutine to IObservable
            return Observable.FromCoroutine<Texture2D> ((observer, cancellationToken) => ScreenShotCoroutine (observer, cancellationToken, screenShotNam, yOffect, ratio, delay));
        }

        static IEnumerator ScreenShotCoroutine (IObserver<Texture2D> observer, CancellationToken cancellationToken, string screenShotName, int yOffect, float ratio, float delay, Camera camera = null) {
            yield return Yielders.GetWaitForSeconds (delay);
            string screenShotPath = Application.temporaryCachePath + "/" + screenShotName + ".png";
            int resWidth = 0;
            int resHeight = 0;
            int screenShotYOffect = 0;

            photoFilePath = screenShotPath;

            yield return new WaitForEndOfFrame ();

            // 比 3:2 更寬的螢幕 例如 大部分的手機
            // 以手機的寬度當作截圖寬

            if (Screen.height / Screen.width > 1.5f) {
                resWidth = (int) (Screen.width * 1);
                resHeight = (int) (resWidth * ratio);
            }
            //其他情況 例如 iPad
            //此時 以其高度計算 16:9 狀態時應該的寬度，並設定為截圖寬
            else {
                resWidth = (int) (Screen.height / 1.77f);
                resHeight = (int) (resWidth * ratio);
            }

            screenShotYOffect = yOffect;
            Rect screenShotRect = new Rect (Screen.width * 0.5f - resWidth * 0.5f, screenShotYOffect + (Screen.height * 0.5f - resHeight * 0.5f), Screen.width, Screen.height);
            Texture2D imageOverview = new Texture2D (resWidth, resHeight, TextureFormat.RGB24, false);

            imageOverview.ReadPixels (screenShotRect, 0, 0);
            imageOverview.Apply ();

            byte[] bytes = imageOverview.EncodeToPNG ();

            if (System.IO.File.Exists (screenShotPath))
                System.IO.File.Delete (screenShotPath);

            System.IO.File.WriteAllBytes (screenShotPath, bytes);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag (screenShotPath); //Apple will reject the app if this is backed up
#endif
            if (imageOverview != null) {
                observer.OnNext (imageOverview);
                observer.OnCompleted (); // IObserver needs OnCompleted after OnNext!
            }
        }

        /// <summary>
        /// 取得 1970/01/01 至現在時間的總秒數
        /// </summary>

        public static int GetTimeStamp () {
            DateTime gtm = new DateTime (1970, 1, 1); //宣告一個GTM時間出來
            DateTime utc = DateTime.UtcNow.AddHours (8); //宣告一個目前的時間
            return Convert.ToInt32 (((TimeSpan) utc.Subtract (gtm)).TotalSeconds);
        }

        /// <summary>
        /// 以 TypeName 尋找對應 Type UnityEngine 專用
        /// </summary>
        public static Type GetType (string TypeName) {
            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, in the same assembly as the caller, etc.
            var type = Type.GetType (TypeName);

            // If it worked, then we're done here
            if (type != null)
                return type;

            // If the TypeName is a full name, then we can try loading the defining assembly directly
            if (TypeName.Contains (".")) {

                // Get the name of the assembly (Assumption is that we are using 
                // fully-qualified type names)
                var assemblyName = TypeName.Substring (0, TypeName.IndexOf ('.'));

                // Attempt to load the indicated Assembly
                var assembly = Assembly.Load (assemblyName);
                if (assembly == null)
                    return null;

                // Ask that assembly to return the proper Type
                type = assembly.GetType (TypeName);
                if (type != null)
                    return type;

            }

            //For WebPlayer use this
            var currentAssembly = System.AppDomain.CurrentDomain.GetAssemblies ();
            foreach (var assembly in currentAssembly) {
                // Load the referenced assembly
                if (assembly != null) {
                    // See if that assembly defines the named type
                    type = assembly.GetType (TypeName);
                    if (type != null)
                        return type;
                }
            }

            // The type just couldn't be found...
            return null;
        }

        //Base64 編碼
        public static string Base64Encode (string source) {
            return Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (source));
        }

        //Base64 解碼
        public static string Base64Decode (string base64Str) {
            return System.Text.Encoding.UTF8.GetString (Convert.FromBase64String (base64Str));
        }

        //Convert to Utf8 String
        public static string BytesToString (byte[] bytes) {
            return  System.Text.UTF8Encoding.Default.GetString ( bytes );
        }
        //Convert to Utf8 Bytes
        public static byte[] StringToBytes (string source) {
            return System.Text.UTF8Encoding.Default.GetBytes (source);
        }
    }

}