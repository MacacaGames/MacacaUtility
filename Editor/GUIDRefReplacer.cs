using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class GUIDRefReplacer : EditorWindow
{
    private static GUIDRefReplacer _window;
    private Object _sourceOld;
    private Object _sourceNew; private string _oldGuid;
    private string _newGuid; private bool isContainScene = true;
    private bool isContainPrefab = true;
    private bool isContainMat = true;
    private bool isContainAsset = false; private List<string> withoutExtensions = new List<string>();
    [MenuItem("MacacaGames/Tools/GUID Ref Replacer")] // 選單開啟並點選的 處理
    public static void GUIDRefReplaceWin()
    {// true 表示不能停靠的
        _window = (GUIDRefReplacer)EditorWindow.GetWindow(typeof(GUIDRefReplacer), true, "引用替換 (●'◡'●)");
        _window.Show();
    }
    void OnGUI()
    {
        // 要被替換的(需要移除的)
        GUILayout.Space(20);
        _sourceOld = EditorGUILayout.ObjectField("舊的資源", _sourceOld, typeof(Object), true);
        _sourceNew = EditorGUILayout.ObjectField("新的資源", _sourceNew, typeof(Object), true);
        // 在那些型別中查詢(.unity/.prefab/.mat)
        GUILayout.Space(20);
        GUILayout.Label("要在哪些型別中查詢替換:");
        EditorGUILayout.BeginHorizontal(); isContainScene = GUILayout.Toggle(isContainScene, ".unity");
        isContainPrefab = GUILayout.Toggle(isContainPrefab, ".prefab");
        isContainMat = GUILayout.Toggle(isContainMat, ".mat");
        isContainAsset = GUILayout.Toggle(isContainAsset, ".asset"); EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("開始替換!"))
        {
            if (EditorSettings.serializationMode != SerializationMode.ForceText)
            {
                Debug.LogError("需要設定序列化模式為 SerializationMode.ForceText");
                ShowNotification(new GUIContent("需要設定序列化模式為 SerializationMode.ForceText"));
            }
            else if (_sourceNew == null || _sourceOld == null)
            {
                Debug.LogError("不能為空!");
                ShowNotification(new GUIContent("不能為空!"));
            }
            else if (_sourceNew.GetType() != _sourceOld.GetType())
            {
                Debug.LogError("兩種資源型別不一致!");
                ShowNotification(new GUIContent("兩種資源型別不一致!"));
            }
            else if (!isContainScene && !isContainPrefab && !isContainMat && !isContainAsset)
{
                Debug.LogError("要選擇一種 查詢替換的型別");
                ShowNotification(new GUIContent("要選擇一種 查詢替換的型別"));
            }
else // 執行替換邏輯
            {
                StartReplace();
            }
        }
    }
    private void StartReplace()
    {
        var path = AssetDatabase.GetAssetPath(_sourceOld); _oldGuid = AssetDatabase.AssetPathToGUID(path);
        _newGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_sourceNew)); Debug.Log("oldGUID = " + _oldGuid + "" + "_newGuid = " + _newGuid); withoutExtensions = new List<string>();
        if (isContainScene)
        {
            withoutExtensions.Add(".unity");
        }
        if (isContainPrefab)
        {
            withoutExtensions.Add(".prefab");
        }
        if (isContainMat)
        {
            withoutExtensions.Add(".mat");
        }
        if (isContainAsset)
        {
            withoutExtensions.Add(".asset");
        }
        Find();
    }
    
     /// <summary>
    /// 查詢並 替換
    /// </summary>
    private void Find()
    {
        if (withoutExtensions == null || withoutExtensions.Count == 0)
        {
            withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
        }
        string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
       .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        int startIndex = 0; if (files == null || files.Length == 0)
        {
            Debug.Log("沒有找到 篩選的引用");
            return;
        }
        EditorApplication.update = delegate ()
       {
           string file = files[startIndex]; bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配資源中", file, (float)startIndex / (float)files.Length); var content = File.ReadAllText(file);
           if (Regex.IsMatch(content, _oldGuid))
           {
               Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file))); content = content.Replace(_oldGuid, _newGuid); File.WriteAllText(file, content);
           }
           else
           {
               Debug.Log(file);
           }
           startIndex++;
           if (isCancel || startIndex >= files.Length)
           {
               EditorUtility.ClearProgressBar();
               EditorApplication.update = null;
               startIndex = 0; AssetDatabase.Refresh();
               Debug.Log("替換結束");
           }
       };
    }
    private string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }
}
