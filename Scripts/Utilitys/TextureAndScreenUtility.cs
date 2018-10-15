using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;

namespace CloudMacaca
{

    public class TextureAndScreenUtility
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

        public static IObservable<Texture2D> ScreenShot(string screenShotNam, int yOffect, float ratio = 1.45f, float delay = 0, bool writeToDisk = false)
        {
            // convert coroutine to IObservable
            return Observable.FromCoroutine<Texture2D>((observer, cancellationToken) => ScreenShotCoroutine(observer, cancellationToken, screenShotNam, yOffect, ratio, delay, writeToDisk: writeToDisk));
        }

        static IEnumerator ScreenShotCoroutine(IObserver<Texture2D> observer, CancellationToken cancellationToken, string screenShotName, int yOffect, float ratio, float delay, Camera camera = null, bool writeToDisk = false)
        {
            photoFilePath = "";
            yield return Yielders.GetWaitForSeconds(delay);
            int resWidth = 0;
            int resHeight = 0;
            int screenShotYOffect = 0;

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

            screenShotYOffect = Mathf.Clamp(yOffect, -(int)(Screen.height * 0.5f), (int)(Screen.height * 0.5f));
            Rect screenShotRect = new Rect(Screen.width * 0.5f - resWidth * 0.5f, screenShotYOffect + (Screen.height * 0.5f - resHeight * 0.5f), Screen.width, Screen.height);
            Texture2D imageOverview = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

            imageOverview.ReadPixels(screenShotRect, 0, 0);
            imageOverview.Apply();

            if (writeToDisk == true)
            {
                WriteImageToDisk(screenShotName, imageOverview);
            }

            if (imageOverview != null)
            {
                observer.OnNext(imageOverview);
                observer.OnCompleted(); // IObserver needs OnCompleted after OnNext!
            }
        }
        public enum WaterMarkPosition
        {
            RightTop,
            LeftTop,
            RightBottom,
            LeftBottom
        }
        public static Texture2D AddWatermark(Texture2D background, Texture2D watermark, WaterMarkPosition position = WaterMarkPosition.RightTop)
        {

            int startX = 0;
            int startY = 0;
            switch (position)
            {
                case WaterMarkPosition.RightTop:
                    startX = 0;
                    startY = background.height - watermark.height;
                    break;
                case WaterMarkPosition.LeftTop:
                    startX = background.width - watermark.width;
                    startY = background.height - watermark.height;
                    break;
                case WaterMarkPosition.RightBottom:
                    startX = 0;
                    startY = 0;
                    break;
                case WaterMarkPosition.LeftBottom:
                    startX = background.width - watermark.width;
                    startY = 0;
                    break;
            }


            for (int x = startX; x < background.width; x++)
            {

                for (int y = startY; y < background.height; y++)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    background.SetPixel(x, y, final_color);
                }
            }

            background.Apply();
            return background;
        }
        public static Texture2D ScaleTexture(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
        {
            Rect texR = new Rect(0, 0, width, height);
            _gpu_scale(src, width, height, mode);

            //Get rendered data back to a new texture
            Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, false);
            result.Resize(width, height);
            result.ReadPixels(texR, 0, 0, false);
            result.wrapMode = UnityEngine.TextureWrapMode.Clamp;
            return result;
        }
        // Internal unility that renders the source texture into the RTT - the scaling method itself.
        static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
        {
            //We need the source texture in VRAM because we render with it
            src.filterMode = fmode;
            src.Apply(true);

            //Using RTT for best quality and performance. Thanks, Unity 5
            RenderTexture rtt = new RenderTexture(width, height, 32);

            //Set the RTT in order to render to it
            Graphics.SetRenderTarget(rtt);

            //Setup 2D matrix in range 0..1, so nobody needs to care about sized
            GL.LoadPixelMatrix(0, 1, 1, 0);

            //Then clear & draw the texture to fill the entire RTT.
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
        }

        public static void WriteImageToDisk(string fileName, Texture2D imageOverview)
        {
            string screenShotPath = Application.temporaryCachePath + "/" + fileName + ".png";
            photoFilePath = screenShotPath;

            byte[] bytes = imageOverview.EncodeToPNG();

            if (System.IO.File.Exists(screenShotPath))
                System.IO.File.Delete(screenShotPath);

            System.IO.File.WriteAllBytes(screenShotPath, bytes);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag (screenShotPath); //Apple will reject the app if this is backed up
#endif
        }

    }

}