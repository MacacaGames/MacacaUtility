using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuiltInResourcesWindow : EditorWindow
{
    [MenuItem("MacacaGames/Built-in styles and icons")]
    public static void ShowWindow()
    {
        BuiltInResourcesWindow w = (BuiltInResourcesWindow)EditorWindow.GetWindow<BuiltInResourcesWindow>();
        w.Show();
    }

    private struct Drawing
    {
        public Rect Rect;
        public Action Draw;
    }

    private List<Drawing> Drawings;

    private List<UnityEngine.Object> _objects;
    private float _scrollPos;
    private float _maxY;
    private Rect _oldPosition;

    private bool _showingStyles = true;
    private bool _showingIcons = false;
    private bool _showingCmResources = false;
    public enum CurrentTab
    {
        Styles, Icons, Icons_old, MacacaResource
    }
    CurrentTab currentTab = CurrentTab.Styles;
    private string _search = "";

    void OnGUI()
    {
        if (position.width != _oldPosition.width && Event.current.type == EventType.Layout)
        {
            Drawings = null;
            _oldPosition = position;
        }

        using (var horizon = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Toggle(currentTab == CurrentTab.Styles, CurrentTab.Styles.ToString(), EditorStyles.toolbarButton))
            {
                currentTab = CurrentTab.Styles;
                Drawings = null;
            }
            if (GUILayout.Toggle(currentTab == CurrentTab.Icons, CurrentTab.Icons.ToString(), EditorStyles.toolbarButton))
            {
                currentTab = CurrentTab.Icons;
                Drawings = null;
            }
            if (GUILayout.Toggle(currentTab == CurrentTab.Icons_old, CurrentTab.Icons_old.ToString(), EditorStyles.toolbarButton))
            {
                currentTab = CurrentTab.Icons_old;
                Drawings = null;
            }
            if (GUILayout.Toggle(currentTab == CurrentTab.MacacaResource, CurrentTab.MacacaResource.ToString(), EditorStyles.toolbarButton))
            {
                currentTab = CurrentTab.MacacaResource;
                Drawings = null;
            }
        }
        string newSearch = "";
        using (var horizon = new GUILayout.HorizontalScope())
        {
            newSearch = GUILayout.TextField(_search, new GUIStyle("ToolbarSeachTextField"));
            if (GUILayout.Button(GUIContent.none, new GUIStyle("ToolbarSeachCancelButtonEmpty")))
            {
                newSearch = "";
            }
        }
        if (newSearch != _search)
        {
            _search = newSearch;
            Drawings = null;
        }

        float top = 36;

        if (Drawings == null)
        {
            string lowerSearch = _search.ToLower();

            Drawings = new List<Drawing>();

            GUIContent inactiveText = new GUIContent("inactive");
            GUIContent activeText = new GUIContent("active");

            float x = 5.0f;
            float y = 5.0f;

            if (currentTab == CurrentTab.Styles)
            {
                foreach (GUIStyle ss in GUI.skin.customStyles)
                {
                    if (!string.IsNullOrEmpty(lowerSearch) && !ss.name.ToLower().Contains(lowerSearch))
                        continue;

                    GUIStyle thisStyle = ss;

                    Drawing draw = new Drawing();

                    float width = Mathf.Max(
                        100.0f,
                        GUI.skin.button.CalcSize(new GUIContent(ss.name)).x,
                        ss.CalcSize(inactiveText).x + ss.CalcSize(activeText).x
                                      ) + 16.0f;

                    float height = 60.0f;

                    if (x + width > position.width - 32 && x > 5.0f)
                    {
                        x = 5.0f;
                        y += height + 10.0f;
                    }

                    draw.Rect = new Rect(x, y, width, height);

                    width -= 8.0f;

                    draw.Draw = () =>
                    {
                        if (GUILayout.Button(thisStyle.name, GUILayout.Width(width)))
                            CopyText("new GUIStyle(\"" + thisStyle.name + "\")");

                        GUILayout.BeginHorizontal();
                        GUILayout.Toggle(false, inactiveText, thisStyle, GUILayout.Width(width / 2));
                        GUILayout.Toggle(false, activeText, thisStyle, GUILayout.Width(width / 2));
                        GUILayout.EndHorizontal();
                    };

                    x += width + 18.0f;

                    Drawings.Add(draw);
                }
            }
            else if (currentTab == CurrentTab.Icons)
            {

                var editorAssetBundle = GetEditorAssetBundle();
                var iconsPath = GetIconsPath();

                var assetNames = EnumerateIcons(editorAssetBundle, iconsPath).ToArray();
                float rowHeight = 0.0f;

                for (var i = 0; i < assetNames.Length; i++)
                {

                    var assetName = assetNames[i];
                    var icon = editorAssetBundle.LoadAsset<Texture2D>(assetName);
                    if (icon == null)
                        continue;

                    if (icon.name == "")
                        continue;

                    if (icon.name.Contains("(Generated)"))
                        continue;

                    if (!string.IsNullOrEmpty(lowerSearch) && !icon.name.ToLower().Contains(lowerSearch))
                        continue;

                    Drawing draw = new Drawing();

                    float width = Mathf.Max(
                        GUI.skin.button.CalcSize(new GUIContent(icon.name)).x,
                        icon.width
                    ) + 8.0f;

                    float height = icon.height + GUI.skin.button.CalcSize(new GUIContent(icon.name)).y + 8.0f;

                    if (x + width > position.width - 32.0f)
                    {
                        x = 5.0f;
                        y += rowHeight + 8.0f;
                        rowHeight = 0.0f;
                    }

                    draw.Rect = new Rect(x, y, width, height);

                    rowHeight = Mathf.Max(rowHeight, height);

                    width -= 8.0f;

                    draw.Draw = () =>
                    {
                        if (GUILayout.Button(icon.name, GUILayout.Width(width)))
                            CopyText("EditorGUIUtility.FindTexture( \"" + icon.name + "\" )");

                        Rect textureRect = GUILayoutUtility.GetRect(icon.width, icon.width, icon.height, icon.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                        EditorGUI.DrawTextureTransparent(textureRect, icon);
                    };

                    x += width + 8.0f;

                    Drawings.Add(draw);
                }
            }
            else if (currentTab == CurrentTab.Icons_old)
            {
                if (_objects == null)
                {
                    _objects = new List<UnityEngine.Object>(Resources.FindObjectsOfTypeAll(typeof(Texture)));
                    _objects.Sort((pA, pB) => System.String.Compare(pA.name, pB.name, System.StringComparison.OrdinalIgnoreCase));
                }

                float rowHeight = 0.0f;

                foreach (UnityEngine.Object oo in _objects)
                {

                    Texture texture = (Texture)oo;

                    if (texture.name == "")
                        continue;

                    if (texture.name.Contains("(Generated)"))
                        continue;

                    if (!string.IsNullOrEmpty(lowerSearch) && !texture.name.ToLower().Contains(lowerSearch))
                        continue;

                    Drawing draw = new Drawing();

                    float width = Mathf.Max(
                        GUI.skin.button.CalcSize(new GUIContent(texture.name)).x,
                        texture.width
                    ) + 8.0f;

                    float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;

                    if (x + width > position.width - 32.0f)
                    {
                        x = 5.0f;
                        y += rowHeight + 8.0f;
                        rowHeight = 0.0f;
                    }

                    draw.Rect = new Rect(x, y, width, height);

                    rowHeight = Mathf.Max(rowHeight, height);

                    width -= 8.0f;

                    draw.Draw = () =>
                    {
                        if (GUILayout.Button(texture.name, GUILayout.Width(width)))
                            CopyText("EditorGUIUtility.FindTexture( \"" + texture.name + "\" )");

                        Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                        EditorGUI.DrawTextureTransparent(textureRect, texture);
                    };

                    x += width + 8.0f;

                    Drawings.Add(draw);
                }
            }
            else if (currentTab == CurrentTab.MacacaResource)
            {
                var dict = MacacaGames.CMEditorUtility.LoadResourceAssets();
                float rowHeight = 0.0f;
                foreach (var ss in dict)
                {
                    Texture texture = ss.Value;
                    Drawing draw = new Drawing();

                    float width = Mathf.Max(
                        GUI.skin.button.CalcSize(new GUIContent(ss.Key)).x,
                        texture.width
                    ) + 8.0f;

                    float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(ss.Key)).y + 8.0f;

                    if (x + width > position.width - 32.0f)
                    {
                        x = 5.0f;
                        y += rowHeight + 8.0f;
                        rowHeight = 0.0f;
                    }

                    draw.Rect = new Rect(x, y, width, height);

                    rowHeight = Mathf.Max(rowHeight, height);

                    width -= 8.0f;

                    draw.Draw = () =>
                    {
                        if (GUILayout.Button(texture.name, GUILayout.Width(width)))
                            CopyText("CloudMacaca.CMEditorUtility.TryGetEditorTexture( \"" + ss.Key + "\" )");

                        Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                        EditorGUI.DrawTextureTransparent(textureRect, texture);
                    };

                    x += width + 8.0f;

                    Drawings.Add(draw);
                }
                y += 100;
            }

            _maxY = y;
        }

        Rect r = position;
        r.y = top;
        r.height -= r.y;
        r.x = r.width - 16;
        r.width = 16;

        float areaHeight = position.height - top;
        _scrollPos = GUI.VerticalScrollbar(r, _scrollPos, areaHeight, 0.0f, _maxY);

        Rect area = new Rect(0, top, position.width - 16.0f, areaHeight);
        GUILayout.BeginArea(area);

        int count = 0;
        foreach (Drawing draw in Drawings)
        {
            Rect newRect = draw.Rect;
            newRect.y -= _scrollPos;

            if (newRect.y + newRect.height > 0 && newRect.y < areaHeight)
            {
                GUILayout.BeginArea(newRect, GUI.skin.textField);
                draw.Draw();
                GUILayout.EndArea();

                count++;
            }
        }

        GUILayout.EndArea();
        using (var disable = new EditorGUI.DisabledGroupScope(false))
        {
            GUI.TextField(new Rect(0, position.height - 16, position.width, EditorGUIUtility.singleLineHeight), "some texture needs using EditorGUIUtility.IconContent() API to get, usually the icon name contains space char will need use this API.",new GUIStyle("AppToolbar"));
        }
    }

    void CopyText(string pText)
    {
        TextEditor editor = new TextEditor();

        //editor.content = new GUIContent(pText); // Unity 4.x code
        editor.text = pText; // Unity 5.x code

        editor.SelectAll();
        editor.Copy();
    }

    private static IEnumerable<string> EnumerateIcons(AssetBundle editorAssetBundle, string iconsPath)
    {
        foreach (var assetName in editorAssetBundle.GetAllAssetNames())
        {
            if (assetName.StartsWith(iconsPath, StringComparison.OrdinalIgnoreCase) == false)
                continue;
            if (assetName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) == false &&
                assetName.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) == false)
                continue;

            yield return assetName;
        }
    }
    private static AssetBundle GetEditorAssetBundle()
    {
        var editorGUIUtility = typeof(EditorGUIUtility);
        var getEditorAssetBundle = editorGUIUtility.GetMethod(
            "GetEditorAssetBundle",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        return (AssetBundle)getEditorAssetBundle.Invoke(null, new object[] { });
    }
    private static string GetIconsPath()
    {
#if UNITY_2018_3_OR_NEWER
        return UnityEditor.Experimental.EditorResources.iconsPath;
#else
            var assembly = typeof(EditorGUIUtility).Assembly;
            var editorResourcesUtility = assembly.GetType("UnityEditorInternal.EditorResourcesUtility");

            var iconsPathProperty = editorResourcesUtility.GetProperty(
                "iconsPath",
                BindingFlags.Static | BindingFlags.Public);

            return (string)iconsPathProperty.GetValue(null, new object[] { });
#endif
    }
}