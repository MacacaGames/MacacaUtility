﻿using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;
using System;
#if AssetBundleBrowser
using AssetBundleBrowser.AssetBundleDataSource;
#endif
namespace CloudMacaca
{
    public class CMAutoBuilder
    {

        static void buildAssetBundle(BuildTarget target)
        {
#if AssetBundleBrowser
            BuildAssetBundleOptions opt = BuildAssetBundleOptions.None;
            opt |= BuildAssetBundleOptions.ChunkBasedCompression;

            ABBuildInfo buildInfo = new ABBuildInfo();
            string outputPath = GetAssetBundlePath(target);
            buildInfo.outputDirectory = outputPath;
            buildInfo.options = opt;
            buildInfo.buildTarget = target;

            try
            {
                if (Directory.Exists(outputPath))
                    Directory.Delete(outputPath, true);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            AssetBundleBrowser.AssetBundleModel.Model.DataSource.BuildAssetBundles(buildInfo);


            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
#endif
        }

        // 一個簡單的 Build pipeline 範例
        [MenuItem("CloudMacaca/Build/Android")]
        public static void BuildAndroid()
        {
            var buildTarget = BuildTarget.Android;
            buildAssetBundle(buildTarget);

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
#if UNITY_2018_1_OR_NEWER
            if (result.files.Length <= 0)
            {
                throw new System.Exception(result.ToString());
            }
#else
            if (!string.IsNullOrEmpty(result))
            {
                throw new System.Exception(result.ToString());
            }
#endif
        }
        enum ArchitectureValue
        {
            None,
            ARM64,
            Universal
        }
        public enum CocoapodsIntegrationMethod
        {
            None,
            Project,
            Workspace
        }

        [MenuItem("CloudMacaca/Build/iOS")]
        public static void BuildiOS()
        {
            EditorPrefs.SetInt("Google.IOSResolver.CocoapodsIntegrationMethod", (int)CocoapodsIntegrationMethod.Workspace);
            //PlayerSetting
            PlayerSettings.statusBarHidden = true;
            PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
            PlayerSettings.iOS.appInBackgroundBehavior = iOSAppInBackgroundBehavior.Suspend;
            PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            PlayerSettings.iOS.requiresFullScreen = true;

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)ArchitectureValue.Universal);


            var buildTarget = BuildTarget.iOS;
            var buildScenes = GetBuildScenes();
            var outputPath = GetOutputPath(buildTarget);
            var outputDir = Path.GetDirectoryName(outputPath);


            buildAssetBundle(buildTarget);


            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, buildTarget);

            var result = BuildPipeline.BuildPlayer(buildScenes, outputPath, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);

#if UNITY_2018_1_OR_NEWER
            if (result.files.Length <= 0)
            {
                throw new System.Exception(result.ToString());
            }
#else
            if (!string.IsNullOrEmpty(result))
            {
                throw new System.Exception(result.ToString());
            }
#endif
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
                result = Path.GetFullPath(desktopPath + Path.DirectorySeparatorChar + "BuildIosTemp" + Path.DirectorySeparatorChar + Application.productName);
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

        static string GetAssetBundlePath(BuildTarget target)
        {
            string result = "";
            if (target == BuildTarget.Android)
            {
                result = "Assets/StreamingAssets/Android";
            }
            else if (target == BuildTarget.iOS)
            {
                result = "Assets/StreamingAssets/iOS";
            }
            return result;
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