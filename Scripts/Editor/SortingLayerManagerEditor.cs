
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;

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

    void OnGUI()
    {
        Refresh();
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

    [MenuItem("CloudMacaca/Tools/SortingLayerManager")]
    public static void Init()
    {
        SortingLayerManagerEditor window = GetWindow<SortingLayerManagerEditor>("LayerManager");
        window.Show();
    }
}