
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.Rendering;
using System.Linq;

public class SortingLayerManagerEditor : EditorWindow
{

    private static MethodInfo EditorGUILayoutSortingLayerField;

    Dictionary<string, List<Renderer>> renders = new Dictionary<string, List<Renderer>>();
    Dictionary<string, bool> selectDic = new Dictionary<string, bool>();
    SerializedObject spRenderSerializedObject;
    Vector2 scrollPos;
    Renderer[] spRenders;

    void OnEnable()
    {
        if (EditorGUILayoutSortingLayerField == null)
            EditorGUILayoutSortingLayerField = typeof(EditorGUILayout).GetMethod("SortingLayerField", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(GUIContent), typeof(SerializedProperty), typeof(GUIStyle) }, null);

        spRenders = GameObject.FindObjectsOfType<Renderer>();
        Resort();
    }

    int sortingOffset = 0;
    String sortingOffsetString = "";
    int sortingSpace = 0;
    String sortingSpaceString = "";
    bool isSortReverse = false;

    public struct PrepareSortSortingLayerItems
    {
        public GameObject gameObject;
        public SpriteRenderer spriteRenderer;
        public SortingGroup sortingGroup;
        public ParticleSystemRenderer particleSystemRenderer;
    }

    List<PrepareSortSortingLayerItems> prepareSortingLayerItemsList = new List<PrepareSortSortingLayerItems>();
    enum SortingBy
    {
        X,
        Y,
        Z
    }

    SortingBy sortingBy = SortingBy.Z;

    void OnGUI()
    {
        isSortReverse = EditorGUILayout.Toggle("is Sort Reverse", isSortReverse);

        sortingOffsetString = EditorGUILayout.TextField("Sorting Offset", sortingOffsetString);
        sortingSpaceString = EditorGUILayout.TextField("Sorting Space", sortingSpaceString);
        sortingBy = (SortingBy)EditorGUILayout.EnumPopup("Sorting By", sortingBy);

        if (GUILayout.Button("Sorting Selected"))
        {
            SortingSelected(sortingBy);
        }

        Refresh();
    }
    void SortingSelected(SortingBy sortingBy)
    {
        Debug.Log("------- Sorting Selected Start -------");

        //參數歸零
        prepareSortingLayerItemsList = new List<PrepareSortSortingLayerItems>();
        sortingOffset = 0;
        sortingSpace = 0;

        //轉換參數
        if (sortingOffsetString == null || sortingSpaceString == null)
        {
            Debug.LogWarning("Sorting Space / Offset 數值未填");
            return;
        }

        int.TryParse(sortingOffsetString, out sortingOffset);
        int.TryParse(sortingSpaceString, out sortingSpace);

        if (sortingSpace < 0)
        {
            Debug.LogWarning("Sorting Space 無法為負數");
            return;
        }


        if (Selection.gameObjects != null)
        {
            // 取得場上選擇的物件放入 List
            foreach (var item in Selection.gameObjects)
            {
                PrepareSortSortingLayerItems tempPSSLI = new PrepareSortSortingLayerItems();
                tempPSSLI.gameObject = item;
                if (item.TryGetComponent(out SpriteRenderer sr))
                {
                    tempPSSLI.spriteRenderer = sr;
                }
                if (item.TryGetComponent(out SortingGroup sg))
                {
                    tempPSSLI.sortingGroup = sg;
                }
                if (item.TryGetComponent(out ParticleSystemRenderer psr))
                {
                    tempPSSLI.particleSystemRenderer = psr;
                }
                prepareSortingLayerItemsList.Add(tempPSSLI);
            }

            // List 內容排序
            List<PrepareSortSortingLayerItems> sortingResultList = new List<PrepareSortSortingLayerItems>();
            if (sortingBy == SortingBy.X)
            {
                sortingResultList = prepareSortingLayerItemsList.OrderBy(_ => _.gameObject.transform.position.x).ToList();
            }
            else if (sortingBy == SortingBy.Y)
            {
                sortingResultList = prepareSortingLayerItemsList.OrderBy(_ => _.gameObject.transform.position.y).ToList();
            }
            else if (sortingBy == SortingBy.Z)
            {
                sortingResultList = prepareSortingLayerItemsList.OrderBy(_ => _.gameObject.transform.position.z).ToList();
            }


            // 依照順序賦 Order in Layer 值 
            int reverseIt = (isSortReverse) ? 1 : 0;
            int index = sortingOffset + (sortingResultList.Count * sortingSpace + (2 - sortingSpace)) * reverseIt;
            reverseIt = (isSortReverse) ? -1 : 1;
            foreach (var item in sortingResultList)
            {
                if (item.spriteRenderer != null)
                {
                    item.spriteRenderer.sortingOrder = index;
                }
                if (item.sortingGroup != null)
                {
                    item.sortingGroup.sortingOrder = index;
                }
                if (item.particleSystemRenderer != null)
                {
                    item.particleSystemRenderer.sortingOrder = index;
                }
                index += (sortingSpace + 1) * reverseIt;
            }
            Debug.Log("------- Sorting Selected Done -------");

        }
        else
        {
            Debug.LogWarning("未選擇場上的 Game Object");
        }
    }


    void Resort()
    {
        renders.Clear();
        foreach (Renderer spRender in spRenders)
        {
            if (!renders.ContainsKey(spRender.sortingLayerName))
            {
                renders[spRender.sortingLayerName] = new List<Renderer>();
            }
            renders[spRender.sortingLayerName].Add(spRender);
        }
    }


    void Refresh()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.Space();
        foreach (KeyValuePair<string, List<Renderer>> keyValue in renders)
        {
            string layerName = keyValue.Key;
            bool isSelected = false;
            if (!selectDic.ContainsKey(layerName))
            {
                selectDic[layerName] = true;
            }
            isSelected = selectDic[layerName];
            isSelected = EditorGUILayout.Foldout(isSelected, layerName);
            selectDic[layerName] = isSelected;
            if (isSelected)
            {
                int length = keyValue.Value.Count;
                for (int i = 0; i < length; i++)
                {
                    Renderer spRender = keyValue.Value[i];
                    if (spRender == null) continue;
                    EditorGUILayout.BeginHorizontal();
                    bool isActive = spRender.gameObject.activeSelf;
                    isActive = EditorGUILayout.Toggle(isActive, GUILayout.Width(10f));
                    spRender.gameObject.SetActive(isActive);
                    EditorGUILayout.LabelField(spRender.name, GUILayout.MaxWidth(200f));

                    SerializedObject serializedObject = new SerializedObject(spRender);
                    SerializedProperty serializedProperty = serializedObject.FindProperty("m_SortingLayerID");

                    if (EditorGUILayoutSortingLayerField != null && serializedProperty != null)
                    {
                        EditorGUILayoutSortingLayerField.Invoke(null, new object[] { new GUIContent(""), serializedProperty, EditorStyles.popup });
                    }
                    else
                    {
                        spRender.sortingLayerID = EditorGUILayout.IntField("Sorting Layer ID", spRender.sortingLayerID);
                    }
                    spRender.sortingOrder = EditorGUILayout.IntField(spRender.sortingOrder);
                    if (GUILayout.Button("Select"))
                    {
                        Selection.activeGameObject = spRender.gameObject;
                    }
                    EditorGUILayout.EndHorizontal();

                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(spRender);
                }
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("Resort", GUILayout.MaxWidth(100f)))
        {
            Resort();
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }

    [MenuItem("MacacaGames/Tools/SortingLayerManager")]
    public static void Init()
    {
        SortingLayerManagerEditor window = GetWindow<SortingLayerManagerEditor>("LayerManager");
        window.Show();
    }
}