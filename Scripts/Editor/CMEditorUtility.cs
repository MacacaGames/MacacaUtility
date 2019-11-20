using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

namespace CloudMacaca
{
    public class CMEditorUtility
    {

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
            {"ImageNotFound","iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAACWElEQVQoFS1SS2/TQBCe2V07TuqkTtPQViiFtEKqQOVAJSROXLlw4ILED+Q39ITEHYESEAJUlT4k1AbapLHj+LGPYbaw9sXa7/P3mEFra3IkpQAEAn8cCAQhbr+InLUghQT0l4iogJxUcjqdp9nCGEIprXNCMIeA7NpaknQ66NHIBCISQgoinM3SPNfGSF2Dfw3WGi8mV6PROF3MHFTOOUZ7BeYSk4VIuknYiJH1A7TW8J+KonRReHF5gWKz3Vr3dhlIJBnkyC3L5bLIUJqoCZbqfFlWpWOiVNHHT6PZbGaMuSUAXwMThLCd1cZKWypF7XbEaZuteFnoyeUNUDAej7MsY1dKkH8sGKFaeS2yqgJpK7Cp0531/saduyrQeTE5/vZDSukzsH9kAeD61DwFDidsYFVoJRW1IY1C2tIJxbrkm1UkPFpSADoAFlQitFyUw5D7ptoadLrX7+1sJo1Gw1viCNyTs4gCndS5S6+qL3/KXxAk/WC43dzuxWG3IyLh0f8sCT8QBxCZOV0dXY/GN4d5MMGyNaC9jQevktX9fJEfnX0fDodxHPMGSCTlwBUwP56PPt+8y4KJFmUgy3ubyc5gi7v8nWbn5+daa5/B8TrwpGR1ubj4OR1N6VSSbi7jJxvPXzx+XS/t+6+HjRYP9P9RKNEC9zA/ORlRkfZpywVwMHz28tGb8ro4/PD2LDvtRrv3acAF3iqQVSAH8RB7iqyPZQTt7z7siOaing2Tg73uU2VpJVmNoogJfm14l7haYxyiJOS2KwVhCOiErklyyABKwAavMBP+AlRiZzNZlw+zAAAAAElFTkSuQmCC"},
            {"PlusButton","iVBORw0KGgoAAAANSUhEUgAAAB4AAAAQCAIAAACOWFiFAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABESURBVDhPYxQSEmKgDWCC0jQAo0ajARKMPgUGUA4RYGgGCOF0jSsQzMzMoCwcYEBdDQcQ5xN0LBwM12gkGwzFAGFgAABuGwuxlT4SZwAAAABJRU5ErkJggg=="},
            {"BorderBackgroundGray","iVBORw0KGgoAAAANSUhEUgAAAAUAAAAECAYAAABGM/VAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAAAMElEQVQYV2P4//8/Q1FR0X8YBvHBAp8+ffp/+fJlMA3igwUfPnwIFgDRYEFM7f8ZAG1EOYL9INrfAAAAAElFTkSuQmCC"},
            {"ItemNotSelect","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"},
            {"EditPencil","iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAHdElNRQfjCAcQCBLW5ejEAAAAn0lEQVQoz6XPMWoCYRCG4YeNLsZGcxjvENLmSEIK0SPYWZguRAi7raRxT5EDpLAUFMfCZXGzv1VmquH9vheG9Ix9+fF8hxqqhHDwksIjAwshhN+UvFI0kWW3vRNC6dGbd71uO+otDGRkrXZp0lx7J+eU/Lrrv/L/4uoGrzy0cV/u8z7m1UzuQ4ju3zAXdSSJ+RbCVN6VQ8+Tja2to0gFLuSUTEAjUjawAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE5LTA4LTA3VDE0OjA4OjE4KzAyOjAwO2e1mQAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOS0wOC0wN1QxNDowODoxOCswMjowMEo6DSUAAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"}
        };

        public static Texture2D TryGetEditorTexture(string name)
        {
            Texture2D result;
            if (s_Cached == null)
            {
                LoadResourceAssets();
            }
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
                GetImageSize(array2, out width, out height);
                Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
                texture2D.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
                texture2D.name = "(Generated) ReorderableList:" + item.Key;
                texture2D.filterMode = FilterMode.Point;
                texture2D.LoadImage(array2);
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

        public static void InspectTarget(UnityEngine.Object target)
        {
            ViewInInspectorInstance(target);
        }
        public static UnityEditor.EditorWindow ViewInInspectorInstance(UnityEngine.Object viewTarget, UnityEditor.EditorWindow inspectorInstance = null)
        {
            var inspectorType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            if (inspectorInstance == null)
                inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as UnityEditor.EditorWindow;

            if (inspectorInstance.GetType() != inspectorType)
                throw new System.NotImplementedException();

            inspectorInstance.Show();

            System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            System.Reflection.MethodInfo isLockedMethodInfo = inspectorType.GetProperty("isLocked", bindingFlags).GetSetMethod();
            isLockedMethodInfo.Invoke(inspectorInstance, new object[] { false });       //解除InspectorLock
            var prevSelection = UnityEditor.Selection.objects;                          //記錄前一個選擇的物件
            UnityEditor.Selection.objects = new UnityEngine.Object[] { viewTarget };    //選擇viewTarget讓Inspector刷新
            isLockedMethodInfo.Invoke(inspectorInstance, new object[] { true });        //Inspector Lock
            UnityEditor.Selection.objects = prevSelection;                              //重新選擇前一個物件，當作什麼都沒發生

            return inspectorInstance;
        }

    }
    class CustomElement
    {
        static Dictionary<int, bool> control = new Dictionary<int, bool>();
        static void SetFocus(int id, bool target)
        {
            if (control.ContainsKey(id))
            {
                control[id] = target;
            }
            else
            {
                control.Add(id, target);
            }
        }
        static bool GetFocus(int id)
        {
            if (control.TryGetValue(id, out bool focus))
            {
                return focus;
            }
            else
            {
                return false;
            }
        }
        public static bool Button(int id, Rect rect, GUIContent content, GUIStyle style, bool processEvent)
        {
            bool result = false;


            var e = Event.current;
            switch (e.type)
            {
                case EventType.Repaint:
                    var isMouseOver = rect.Contains(Event.current.mousePosition);
                    style.Draw(rect, content, isMouseOver, GetFocus(id), GetFocus(id), GetFocus(id));
                    break;
                case EventType.MouseDown:
                    if (processEvent == false)
                    {
                        SetFocus(id, false);
                        GUI.changed = true;
                        return result;
                    }
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            SetFocus(id, true);
                            e.Use();
                            GUI.changed = true;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (processEvent == false)
                    {
                        SetFocus(id, false);
                        GUI.changed = true;
                        return result;
                    }
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            SetFocus(id, false);
                            result = true;
                            GUI.changed = true;
                        }
                    }
                    break;

            }
            return result;
        }
    }
    public class CMEditorLayout
    {
        #region  BitMask
        static GUIStyle _toggleStyle;
        static GUIStyle toggleStyle
        {
            get
            {
                if (_toggleStyle == null)
                {
                    _toggleStyle = new GUIStyle
                    {
                        normal = {
                                background = CMEditorUtility.CreatePixelTexture("_toggleStyle_on",new Color32(64,64,64,255)),
                                textColor = Color.gray
                            },
                        onNormal = {
                                    background = CMEditorUtility.CreatePixelTexture("_toggleStyle",new Color32(128,128,128,255)),

                                 textColor = Color.white
                            },

                        alignment = TextAnchor.MiddleCenter,
                        clipping = TextClipping.Clip,
                        imagePosition = ImagePosition.TextOnly,
                        stretchHeight = true,
                        stretchWidth = true,
                        padding = new RectOffset(0, 0, 0, 0),
                        margin = new RectOffset(0, 0, 0, 0)
                    };
                }
                return _toggleStyle;
            }
        }
        public static void BitMaskField<T>(ref T enumValue) where T : System.Enum
        {
            Dictionary<int, bool> toggleBools = new Dictionary<int, bool>();
            int possiableInt = System.Enum.GetValues(typeof(T)).Cast<int>().Max();
            foreach (T item in System.Enum.GetValues(typeof(T)))
            {
                int intValue = System.Convert.ToInt32(item);
                if (intValue == 0 || intValue == possiableInt)
                {
                    toggleBools.Add(intValue, object.Equals(enumValue, item));
                    continue;
                }
                toggleBools.Add(intValue, FlagsHelper.IsSet(enumValue, item));
            }
            using (var horizon = new EditorGUILayout.HorizontalScope())
            {
                foreach (T item in System.Enum.GetValues(typeof(T)))
                {
                    int intValue = System.Convert.ToInt32(item);

                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        toggleBools[intValue] = GUILayout.Toggle(toggleBools[intValue], item.ToString(), toggleStyle);
                        if (check.changed)
                        {
                            if (intValue == 0 || intValue == possiableInt)
                            {
                                if (toggleBools[intValue])
                                {
                                    enumValue = item;
                                }
                                continue;
                            }
                            if (toggleBools[intValue])
                            {
                                FlagsHelper.Set(ref enumValue, item);
                            }
                            else
                            {
                                FlagsHelper.Unset(ref enumValue, item);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region  GroupedPopup
        static Dictionary<int, Rect> rectGroupedPopupFieldDict = new Dictionary<int, Rect>();
        public static void GroupedPopupField(int id, GUIContent content, IEnumerable<GroupedPopupData> groupedPopupData, GroupedPopupData selected, System.Action<GroupedPopupData> OnSelect, params GUILayoutOption[] layoutOptions)
        {
            if (!rectGroupedPopupFieldDict.ContainsKey(id))
            {
                rectGroupedPopupFieldDict.Add(id, new Rect());
            }

            if (content != GUIContent.none) EditorGUILayout.LabelField(content, GUILayout.Width(EditorGUIUtility.labelWidth));
            string popupTitle = "";

            if (groupedPopupData.Contains(selected))
            {
                popupTitle = selected.name;
            }
            else if (selected == null)
            {
                popupTitle = "Nothing Selected";
            }
            else
            {
                popupTitle = "Missing";
            }
            if (GUILayout.Button(popupTitle, EditorStyles.popup, layoutOptions))
            {
                PopupWindow.Show(rectGroupedPopupFieldDict[id], new GroupedPopupWindow { groupedPopupData = groupedPopupData.ToArray(), Current = selected, OnSelect = OnSelect, WantedWidth = rectGroupedPopupFieldDict[id].width });
            }
            if (Event.current.type == EventType.Repaint) rectGroupedPopupFieldDict[id] = GUILayoutUtility.GetLastRect();
        }

        public static void GroupedPopupField<T>(int id, GUIContent content, IEnumerable<GroupedPopupData<T>> groupedPopupData, GroupedPopupData<T> selected, System.Action<GroupedPopupData<T>> OnSelect, params GUILayoutOption[] layoutOptions) where T : struct
        {
            if (!rectGroupedPopupFieldDict.ContainsKey(id))
            {
                rectGroupedPopupFieldDict.Add(id, new Rect());
            }

            if (content != GUIContent.none) EditorGUILayout.LabelField(content, GUILayout.Width(EditorGUIUtility.labelWidth));
            string popupTitle = "";

            if (groupedPopupData.Contains(selected))
            {
                popupTitle = selected.item.ToString();
            }
            else if (selected == null)
            {
                popupTitle = "Nothing Selected";
            }
            else
            {
                popupTitle = "Missing";
            }
            if (GUILayout.Button(popupTitle, EditorStyles.popup, layoutOptions))
            {
                PopupWindow.Show(rectGroupedPopupFieldDict[id], new GroupedPopupWindow<T> { groupedPopupDataGeneric = groupedPopupData.ToArray(), CurrentGeneric = selected, OnSelectGeneric = OnSelect, WantedWidth = rectGroupedPopupFieldDict[id].width });
            }
            if (Event.current.type == EventType.Repaint) rectGroupedPopupFieldDict[id] = GUILayoutUtility.GetLastRect();
        }

        public class GroupedPopupWindow<T> : GroupedPopupWindow where T : struct
        {
            public GroupedPopupData<T>[] groupedPopupDataGeneric;
            public System.Action<GroupedPopupData<T>> OnSelectGeneric;
            public GroupedPopupData<T> CurrentGeneric;

            protected override void DrawItem()
            {
                using (var scroll = new GUILayout.ScrollViewScope(scrollPos))
                {
                    scrollPos = scroll.scrollPosition;
                    using (var vertical = new GUILayout.VerticalScope())
                    {
                        var grouped = groupedPopupDataGeneric.GroupBy(m => m.group);

                        foreach (var item in grouped)
                        {
                            if (!string.IsNullOrEmpty(searchString))
                            {
                                if (searchString.ToLower().Contains("g:"))
                                {
                                    var s = searchString.ToLower().Split(':');
                                    if (!item.Key.ToLower().Contains(s.Last().ToLower()))
                                    {
                                        continue;
                                    }
                                }
                            }
                            string label = string.IsNullOrEmpty(item.Key) ? " Ungrouped" : " " + item.Key;
                            GUILayout.Label(label, GroupHeader);
                            foreach (var child in item)
                            {
                                if (!string.IsNullOrEmpty(searchString))
                                {
                                    if (!searchString.ToLower().Contains("g:"))
                                    {
                                        if (!child.item.ToString().ToLower().Contains(searchString.ToLower()))
                                        {
                                            continue;
                                        }
                                    }
                                }
                                var contetn = new GUIContent(child.item.ToString());
                                if (CurrentGeneric != null)
                                {
                                    if (CurrentGeneric.item.Equals(child.item))
                                    {
                                        contetn.image = EditorGUIUtility.FindTexture("d_P4_CheckOutRemote");
                                    }
                                }
                                if (GUILayout.Button(contetn, ItemStyle))
                                {
                                    OnSelectGeneric?.Invoke(child);
                                    editorWindow.Close();
                                }
                                Rect btnRect = GUILayoutUtility.GetLastRect();
                                if (btnRect.Contains(Event.current.mousePosition))
                                {
                                    //GUI.Box(btnRect, "", new GUIStyle("U2D.createRect"));
                                    editorWindow.Repaint();
                                }
                            }
                        }
                    }

                }
            }
            // string searchString;
            // protected override void DrawSerachBar()
            // {
            //     GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            //     searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            //     var rect = GUILayoutUtility.GetLastRect();
            //     if (string.IsNullOrEmpty(searchString))
            //     {
            //         GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            //         rect.x += 15;
            //         rect.width -= 15;
            //         GUI.Label(rect, "g: for find by group", new GUIStyle("AnimationSelectionTextField"));
            //         GUI.color = Color.white;
            //     }
            //     if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            //     {
            //         // Remove focus if cleared
            //         searchString = "";
            //         GUI.FocusControl(null);
            //     }
            //     GUILayout.EndHorizontal();
            // }
        }

        public class GroupedPopupWindow : UnityEditor.PopupWindowContent
        {
            public GroupedPopupData[] groupedPopupData;
            public System.Action<GroupedPopupData> OnSelect;
            public GroupedPopupData Current;
            public float WantedWidth;
            static GUIStyle _ItemStyle;
            protected static GUIStyle ItemStyle
            {
                get
                {
                    if (_ItemStyle == null)
                    {
                        _ItemStyle = new GUIStyle(EditorStyles.label);
                        // _ItemStyle.contentOffset = Vector2.zero;
                        _ItemStyle.hover.textColor = Color.white;
                        ColorUtility.TryParseHtmlString("#49beb7", out Color c);
                        _ItemStyle.hover.background = CloudMacaca.CMEditorUtility.CreatePixelTexture("_ItemStyle hover Pixel (List GUI)", c);
                    }
                    return _ItemStyle;
                }
            }
            static GUIStyle _GroupHeader;
            protected static GUIStyle GroupHeader
            {
                get
                {
                    if (_GroupHeader == null)
                    {
                        _GroupHeader = new GUIStyle(EditorStyles.label);
                        _GroupHeader.alignment = TextAnchor.MiddleCenter;
                        _GroupHeader.normal.textColor = Color.white;
                        ColorUtility.TryParseHtmlString("#ff502f", out Color c);
                        _GroupHeader.normal.background = CloudMacaca.CMEditorUtility.CreatePixelTexture("_group header Pixel (List GUI)", c);
                    }
                    return _GroupHeader;
                }
            }
            // public GroupedPopupWindow(IEnumerable<GroupedPopupData> groupedPopupData, GroupedPopupData current, System.Action<GroupedPopupData> OnSelect, float wantedWidth = 200)
            // {
            //     this.OnSelect = OnSelect;
            //     this.groupedPopupData = groupedPopupData.ToArray();
            //     this.current = current;
            //     this.wantedWidth = wantedWidth;
            // }
            public override Vector2 GetWindowSize()
            {
                return new Vector2(300, WantedWidth);
            }
            public override void OnGUI(Rect rect)
            {
                DrawSerachBar();
                DrawItem();
            }
            protected Vector2 scrollPos;
            protected virtual void DrawItem()
            {
                using (var scroll = new GUILayout.ScrollViewScope(scrollPos))
                {
                    scrollPos = scroll.scrollPosition;
                    using (var vertical = new GUILayout.VerticalScope())
                    {
                        var grouped = groupedPopupData.GroupBy(m => m.group);

                        foreach (var item in grouped)
                        {
                            if (!string.IsNullOrEmpty(searchString))
                            {
                                if (searchString.ToLower().Contains("g:"))
                                {
                                    var s = searchString.ToLower().Split(':');
                                    if (!item.Key.ToLower().Contains(s.Last().ToLower()))
                                    {
                                        continue;
                                    }
                                }
                            }
                            string label = string.IsNullOrEmpty(item.Key) ? " Ungrouped" : " " + item.Key;
                            GUILayout.Label(label, GroupHeader);
                            foreach (var child in item)
                            {
                                if (!string.IsNullOrEmpty(searchString))
                                {
                                    if (!searchString.ToLower().Contains("g:"))
                                    {
                                        if (!child.name.ToLower().Contains(searchString.ToLower()))
                                        {
                                            continue;
                                        }
                                    }
                                }
                                var contetn = new GUIContent(child.name);
                                if (Current != null)
                                {
                                    if (Current.name == child.name)
                                    {
                                        contetn.image = EditorGUIUtility.FindTexture("d_P4_CheckOutRemote");
                                    }
                                }
                                if (GUILayout.Button(contetn, ItemStyle))
                                {
                                    OnSelect?.Invoke(child);
                                    editorWindow.Close();
                                }
                                Rect btnRect = GUILayoutUtility.GetLastRect();
                                if (btnRect.Contains(Event.current.mousePosition))
                                {
                                    //GUI.Box(btnRect, "", new GUIStyle("U2D.createRect"));
                                    editorWindow.Repaint();
                                }
                            }
                        }
                    }

                }
            }
            protected string searchString;
            protected virtual void DrawSerachBar()
            {
                GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
                searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
                var rect = GUILayoutUtility.GetLastRect();
                if (string.IsNullOrEmpty(searchString))
                {
                    GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    rect.x += 15;
                    rect.width -= 15;
                    GUI.Label(rect, "g: for find by group", new GUIStyle("AnimationSelectionTextField"));
                    GUI.color = Color.white;
                }
                if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
                {
                    // Remove focus if cleared
                    searchString = "";
                    GUI.FocusControl(null);
                }
                GUILayout.EndHorizontal();
            }
        }

        public class GroupedPopupData
        {
            public string name;
            public string group;
        }

        public class GroupedPopupData<T> where T : struct
        {
            public T item;
            public string group;
        }
        #endregion
    }
}
