using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;
using System;
namespace CloudMacaca
{
    public class CMAutoBuilder
    {
        // 一個簡單的 Build pipeline 範例
        [MenuItem("CloudMacaca/Build/Android")]
        public static void BuildAndroid()
        {
            var buildTarget = BuildTarget.Android;
            //些換到 Android 
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, buildTarget);

            //取得建置環境資料
            var AndroidSdkRoot = EditorSetup.AndroidSdkRoot;
            var AndroidNdkRoot = EditorSetup.AndroidNdkRoot;
            var JdkRoot = EditorSetup.JdkRoot;

            //設定 key
            PlayerSettings.Android.keyaliasName = GetKeyStoreAlias();
            PlayerSettings.Android.keyaliasPass = GetKeyStorePassword();
            PlayerSettings.Android.keystorePass = GetKeyStorePassword();
            PlayerSettings.Android.keystoreName = GetKeyStorePath();

            var outputPath = GetOutputPath(buildTarget);
            var outputDir = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var buildScenes = GetBuildScenes();

            var buildPlayerOptions = new BuildPlayerOptions()
            {
                target = buildTarget,
                scenes = buildScenes,
                options = BuildOptions.None,
                locationPathName = outputPath,
            };

            var result = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (result.files.Length <= 0)
            {
                throw new System.Exception(result.ToString());
            }
        }

        [MenuItem("CloudMacaca/Build/iOS")]
        public static void BuildiOS()
        {
            var buildTarget = BuildTarget.Android;
            var buildScenes = GetBuildScenes();
            var outputPath = GetOutputPath(buildTarget);
            var outputDir = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var buildPlayerOptions = new BuildPlayerOptions()
            {
                target = buildTarget,
                scenes = buildScenes,
                options = BuildOptions.None,
                locationPathName = outputPath,
            };

            var result = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (result.files.Length <= 0)
            {
                throw new System.Exception(result.ToString());
            }
        }
        static string[] GetBuildScenes()
        {
            return EditorBuildSettings.scenes.Where(v => v.enabled).Select(v => v.path).ToArray();
        }
        static string GetOutputPath(BuildTarget target)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string date = DateTime.Now.Date.ToString("yyyy_MM_dd");
            string result = "";
            if (target == BuildTarget.Android)
            {
                result = Path.GetFullPath(desktopPath + Path.DirectorySeparatorChar + "Build" + Path.DirectorySeparatorChar + Application.productName + Path.DirectorySeparatorChar + "Android" + Path.DirectorySeparatorChar + date + ".apk");
            }
            else if (target == BuildTarget.iOS)
            {
                result = Path.GetFullPath(desktopPath + Path.DirectorySeparatorChar + "BuildIosTemp" + Path.DirectorySeparatorChar + Application.productName );
            }
            return result;
        }


        static string GetKeyStorePassword(string defaultValue = "ASDFrewq1234$#@!")
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-keystorepw")
                {
                    defaultValue = args[i + 1];
                    break;
                }
            }
            return defaultValue;
        }
        static string GetKeyStoreAlias(string defaultValue = "")
        {
            defaultValue = PlayerSettings.Android.keyaliasName;
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-keystorealias")
                {
                    defaultValue = args[i + 1];
                    break;
                }
            }
            return defaultValue;
        }

        static string GetKeyStorePath(string defaultValue = "")
        {
            defaultValue = PlayerSettings.Android.keystoreName;
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-keystorepath")
                {
                    defaultValue = args[i + 1];
                    break;
                }
            }
            return defaultValue;
        }
    }


    public class EditorSetup
    {
        public static string AndroidSdkRoot
        {
            get { return EditorPrefs.GetString("AndroidSdkRoot"); }
            set { EditorPrefs.SetString("AndroidSdkRoot", value); }
        }

        public static string JdkRoot
        {
            get { return EditorPrefs.GetString("JdkPath"); }
            set { EditorPrefs.SetString("JdkPath", value); }
        }

        // This requires Unity 5.3 or later
        public static string AndroidNdkRoot
        {
            get { return EditorPrefs.GetString("AndroidNdkRoot"); }
            set { EditorPrefs.SetString("AndroidNdkRoot", value); }
        }
    }
}
