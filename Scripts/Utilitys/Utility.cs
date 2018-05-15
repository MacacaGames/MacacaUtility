using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;

namespace CloudMacaca
{

    public class Utility
    {
        public static string photoFilePath = "";
        /// <summary>
        /// 以攝影機正中心依長寬進行截圖
        /// 比 3:2 更寬的螢幕 例如 大部分的手機，以手機的寬度當作截圖寬
        /// 其他情況 例如 iPad 以其高度計算 16:9 狀態時應該的寬度，並設定為截圖寬
        /// </summary>
        /// <param name="screenShotName">檔案名稱</param>
        /// <param name="yOffect">調整高度正中間的偏移量，可以往上或往下調整，但若偏移量太大超出螢幕範圍時在部分平台可能會報錯</param>
        /// <param name="ratio">截圖的比例</param>

        public static IObservable<Texture2D> ScreenShot(string screenShotNam, int yOffect, float ratio = 1.45f, float delay = 0)
        {
            // convert coroutine to IObservable
            return Observable.FromCoroutine<Texture2D>((observer, cancellationToken)
                => ScreenShotCoroutine(observer, cancellationToken, screenShotNam, yOffect, ratio, delay));
        }

        static IEnumerator ScreenShotCoroutine(IObserver<Texture2D> observer, CancellationToken cancellationToken, string screenShotName, int yOffect, float ratio, float delay, Camera camera = null)
        {
            yield return Yielders.GetWaitForSeconds(delay);
            string screenShotPath = Application.temporaryCachePath + "/" + screenShotName + ".png";
            int resWidth = 0;
            int resHeight = 0;
            int screenShotYOffect = 0;

            photoFilePath = screenShotPath;

            yield return new WaitForEndOfFrame();


            // 比 3:2 更寬的螢幕 例如 大部分的手機
            // 以手機的寬度當作截圖寬

            if (Screen.height / Screen.width > 1.5f)
            {
                resWidth = (int)(Screen.width * 1);
                resHeight = (int)(resWidth * ratio);
            }
            //其他情況 例如 iPad
            //此時 以其高度計算 16:9 狀態時應該的寬度，並設定為截圖寬
            else
            {
                resWidth = (int)(Screen.height / 1.77f);
                resHeight = (int)(resWidth * ratio);
            }

            screenShotYOffect = yOffect;
            Rect screenShotRect = new Rect(Screen.width * 0.5f - resWidth * 0.5f, screenShotYOffect + (Screen.height * 0.5f - resHeight * 0.5f), Screen.width, Screen.height);
            Texture2D imageOverview = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

            imageOverview.ReadPixels(screenShotRect, 0, 0);
            imageOverview.Apply();

            byte[] bytes = imageOverview.EncodeToPNG();

            if (System.IO.File.Exists(screenShotPath))
                System.IO.File.Delete(screenShotPath);

            System.IO.File.WriteAllBytes(screenShotPath, bytes);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(screenShotPath); //Apple will reject the app if this is backed up
#endif
            if (imageOverview != null)
            {
                observer.OnNext(imageOverview);
                observer.OnCompleted(); // IObserver needs OnCompleted after OnNext!
            }
        }

        /// <summary>
        /// 取得 1970/01/01 至現在時間的總秒數
        /// </summary>

        public static int GetTimeStamp()
        {
            DateTime gtm = new DateTime(1970, 1, 1);//宣告一個GTM時間出來
            DateTime utc = DateTime.UtcNow.AddHours(8);//宣告一個目前的時間
            return Convert.ToInt32(((TimeSpan)utc.Subtract(gtm)).TotalSeconds);
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

                // Attempt to load the indicated Assembly
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                    return null;

                // Ask that assembly to return the proper Type
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;

            }

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





        public static Texture2D CreatePixelTexture(string name, Color color)
        {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture2D.name = name;
            texture2D.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
            texture2D.filterMode = FilterMode.Point;
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();
            return texture2D;
        }
        private static Dictionary<string, string> EditorImg = new Dictionary<string, string>()
        {
            {"a","iVBORw0KGgoAAAANSUhEUgAAAB4AAAAQCAYAAAABOs/SAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAAAW0lEQVRIS+3NywnAQAhF0anI4mzVCmzBBl7QEBgGE5JFhBAXd+OHM5gZZgYRKcktNxu+HRFF2e6qhtOjtQM7K/tZ+xY89wSbazg9eqOfw6oag4rcChjY8coAjA2l1RxFDY8IFAAAAABJRU5ErkJggg=="},
            {"b","iVBORw0KGgoAAAANSUhEUgAAAB4AAAASCAYAAABM8m7ZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABkSURBVEhL7dXBCcAgDIVhJ8pwWTUTZIUs8EosLSJpwUNTSiN8HjTyH22+mBmZzqiqwsxSeKvHffMDEUnhrQovIaIuurtT4XBodsSuRG9m3wqPVmKjCodDT/h5+JVvEdjjmQC0DQcAzSE/W4DdAAAAAElFTkSuQmCC"},
            {"PlusButton","iVBORw0KGgoAAAANSUhEUgAAAB4AAAAQCAIAAACOWFiFAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABESURBVDhPYxQSEmKgDWCC0jQAo0ajARKMPgUGUA4RYGgGCOF0jSsQzMzMoCwcYEBdDQcQ5xN0LBwM12gkGwzFAGFgAABuGwuxlT4SZwAAAABJRU5ErkJggg=="},
            {"BorderBackgroundGray","iVBORw0KGgoAAAANSUhEUgAAAAUAAAAECAYAAABGM/VAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAAAMElEQVQYV2P4//8/Q1FR0X8YBvHBAp8+ffp/+fJlMA3igwUfPnwIFgDRYEFM7f8ZAG1EOYL9INrfAAAAAElFTkSuQmCC"},
            {"e","iVBORw0KGgoAAAANSUhEUgAAAAkAAAAFCAYAAACXU8ZrAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAAAIElEQVQYV2P49OnTf0KYobCw8D8hzPD/P2FMLesK/wMAs5yJpK+6aN4AAAAASUVORK5CYII=}"},
            {"f","iVBORw0KGgoAAAANSUhEUgAAAAgAAAACCAIAAADq9gq6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAABVJREFUeNpiVFZWZsAGmBhwAIAAAwAURgBt4C03ZwAAAABJRU5ErkJggg=="},
            {"g","iVBORw0KGgoAAAANSUhEUgAAAAgAAAACCAIAAADq9gq6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAABVJREFUeNpivHPnDgM2wMSAAwAEGAB8VgKYlvqkBwAAAABJRU5ErkJggg=="},
            {"ItemNotSelect","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"},
            {"h","iVBORw0KGgoAAAANSUhEUgAAAAUAAAAECAYAAABGM/VAAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAEFJREFUeNpi/P//P0NxcfF/BgRgZP78+fN/VVVVhpCQEAZjY2OGs2fPNrCApBwdHRkePHgAVwoWnDVrFgMyAAgwAAt4E1dCq1obAAAAAElFTkSuQmCC"},
        };

        public static Texture2D TryGetEditorTexture(string name)
        {
            Texture2D result;
            if (!s_Cached.TryGetValue(name, out result))
            {
                result = CreatePixelTexture(name, Color.gray);
                s_Cached.Add(name, result);
            }

            return result;
        }
        private static Dictionary<string, Texture2D> s_Cached;
        /// <summary>
        /// Read textures from base-64 encoded strings. Automatically selects assets based upon
        /// whether the light or dark (pro) skin is active.
        /// </summary>
        public static Dictionary<string, Texture2D> LoadResourceAssets()
        {
            if (s_Cached != null)
            {
                return s_Cached;
            }
            s_Cached = new Dictionary<string, Texture2D>();
            //for (int i = 0; i < s_Cached.Length; i++)
            foreach (var item in EditorImg)
            {
                byte[] array2 = System.Convert.FromBase64String(item.Value);
                int width = 1;
                int height = 1;
                Texture2D texture2D = new Texture2D(width, height);
                texture2D.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
                texture2D.name = "(Generated)" + item.Key;
                texture2D.filterMode = FilterMode.Point;
                texture2D.LoadRawTextureData(array2);
                texture2D.Apply();
                s_Cached.Add(item.Key, texture2D);
            }
            return s_Cached;
        }
        private static void GetImageSize(byte[] imageData, out int width, out int height)
        {
            width = ReadInt(imageData, 18);
            height = ReadInt(imageData, 22);
        }
        private static int ReadInt(byte[] imageData, int offset)
        {
            return (int)imageData[offset] << 8 | (int)imageData[offset + 1];
        }
    }

}