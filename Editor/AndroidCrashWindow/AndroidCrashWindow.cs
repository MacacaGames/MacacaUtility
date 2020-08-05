using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics;

public class AndroidCrashWindow : EditorWindow
{
    public static AndroidCrashWindow window;

    [MenuItem("Window/AndroidCrashWindow")]
    static void Init()
    {
        window = EditorWindow.GetWindow<AndroidCrashWindow>();
        window.minSize = new Vector2(600, 400);
        window.titleContent = new GUIContent("AndroidCrashWindow");
    }
    public static string AndroidNdkRoot
    {
        get
        {
            return EditorPrefs.GetString("AndroidNdkRoot");
        }
    }
    private void OnFocus()
    {
        unityEditorPath = EditorApplication.applicationPath.Replace("Unity.app", "");
    }
    static string addr2linePathSubFix = "/toolchains/arm-linux-androideabi-4.9/prebuilt/darwin-x86_64/bin/";
    static string addr2lineExculde
    {
        get
        {
            return "arm-linux-androideabi-addr2line";
        }
    }
    string addr2linePath;

    string libUnityPathSubFix = "AndroidPlayer/Variations/il2cpp/Release/Symbols/{platform}/libunity.sym.so";
    string unityPath;

    string unityEditorPath;
    string ndkPath;

    enum Platform
    {
        ArmV7,
        Arm64
    }
    Platform _platform = Platform.ArmV7;
    string GetPlatformPath(Platform platform)
    {
        switch (platform)
        {
            case Platform.Arm64:
                return "arm64-v8a";
            case Platform.ArmV7:
                return "armeabi-v7a";
            default:
                return "";
        }
    }

    enum Lib
    {
        libUnity,
        il2cpp
    }
    Lib _lib;

    string il2cppSymbolsPath;

    string crashLogs;
    Vector2 scrollPosInput, scrollPosResult;
    void OnGUI()
    {
        using (var horizon = new EditorGUILayout.HorizontalScope())
        {
            ndkPath = EditorGUILayout.TextField("NDK Path", ndkPath);
            if (GUILayout.Button("Use"))
            {
                ndkPath = EditorUtility.OpenFolderPanel("Select NDK", "", "");
            }
        }
        addr2linePath = ndkPath + addr2linePathSubFix + addr2lineExculde;

        _platform = (Platform)EditorGUILayout.EnumPopup("Platform", _platform);
        _lib = (Lib)EditorGUILayout.EnumPopup("Symbols", _lib);

        using (var disable = new EditorGUI.DisabledGroupScope(_lib != Lib.il2cpp))
        {
            using (var horizon = new EditorGUILayout.HorizontalScope())
            {
                il2cppSymbolsPath = EditorGUILayout.TextField("il2cppSymbolsPath", il2cppSymbolsPath);
                if (GUILayout.Button("select"))
                {
                    il2cppSymbolsPath = EditorUtility.OpenFilePanel("Select Symbols", "", "");
                }
            }
        }

        switch (_lib)
        {
            case Lib.libUnity:
                unityPath = unityEditorPath + "PlaybackEngines/" + libUnityPathSubFix.Replace("{platform}", GetPlatformPath(_platform));
                break;
            case Lib.il2cpp:
                unityPath = il2cppSymbolsPath;
                break;
        }

        EditorGUILayout.LabelField("addr2linePath path :", addr2linePath);
        EditorGUILayout.LabelField("unityPath path :", unityPath);
        using (var horizon = new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.HelpBox("One line per address", MessageType.Info);
            if (GUILayout.Button("Run", GUILayout.Width(80)))
            {
                var lines = crashLogs.Split('\n', '\r');
                MakeCommand(lines);
            }
        }
        using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosInput, GUILayout.Height(200)))
        {
            scrollPosInput = scroll.scrollPosition;
            crashLogs = EditorGUILayout.TextArea(crashLogs);
        }
        using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosResult, GUILayout.Height(200)))
        {
            scrollPosResult = scroll.scrollPosition;
            EditorGUILayout.LabelField("Result");
            EditorGUILayout.TextArea(string.Join("\n", result));
        }
    }

    List<string> result = new List<string>();

    void MakeCommand(string[] address)
    {
        result.Clear();
        List<string> cmds = new List<string>();
        foreach (var item in address)
        {
            if (string.IsNullOrEmpty(item))
            {
                continue;
            }

            RunCmd(addr2linePath + " -f -C -e " + unityPath + " " + item, (r) =>
              {
                  result.Add(r);
              });
        }
    }

    public static void RunCmd(string cmd, Action<string> act = null)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        UnityEngine.Debug.LogWarning("Run cmd : " + cmd);

        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \" " + cmd + " \"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        UnityEngine.Debug.LogWarning("result : " + result);

        process.WaitForExit();
        act?.Invoke(result);
    }
}
