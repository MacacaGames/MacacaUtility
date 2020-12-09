#if (UNITY_EDITOR && UNITY_2019_1_OR_NEWER)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.UIElements;
public class OnionAlignUtiltiyEditorWindow : EditorWindow
{
    [MenuItem("MacacaGames/Utility/Align Window")]
    public static void Open()
    {
        var window = GetWindow<OnionAlignUtiltiyEditorWindow>();

        window.minSize = new Vector2(210F, 35F);
        window.titleContent = new GUIContent("Align Window");
    }

    private void Awake()
    {

        VisualElement root = new VisualElement()
        {
            style =
                {
                    flexDirection = FlexDirection.Row,

                    marginBottom = 5F,
                    marginLeft =  5F,
                    marginRight =  5F,
                    marginTop =  5F,
                }
        };

        VisualElement horizontalGroup = new VisualElement()
        {
            style = { flexDirection = FlexDirection.Row }
        };
        horizontalGroup.Add(() =>
        {
            List<VisualElement> visualElements = new List<VisualElement>
                {
                    GetButton("H_L").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignHorizontal.Left);    }),
                    GetButton("H_C").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignHorizontal.Center);  }),
                    GetButton("H_R").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignHorizontal.Right);   }),
                    GetButton("H_A").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignHorizontal.Average); }),
                };

            return visualElements;
        });

        VisualElement verticalGroup = new VisualElement()
        {
            style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) }
        };
        verticalGroup.Add(() =>
        {
            List<VisualElement> visualElements = new List<VisualElement>
                {
                    GetButton("V_T").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignVertical.Top);      }),
                    GetButton("V_C").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignVertical.Center);   }),
                    GetButton("V_B").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignVertical.Bottom);   }),
                    GetButton("V_A").Action(_ => { _.clickable.clicked += () => UseAlignInSelections(OnionAlignUtility.AlignVertical.Average);  }),
                };

            return visualElements;
        });

        root.Add(
            horizontalGroup,
            new VisualElement() { style = { width = new StyleLength(10F) } },
            verticalGroup);

        rootVisualElement.Add(root);
    }

    Button GetButton(string iconName)
    {
        const float size = 24F;
        const float iconSize = 20F;

        var imgs = MacacaGames.CMEditorUtility.LoadResourceAssets();

        Button targetBtn = new Button()
        {
            style =
                {
                    width = size,
                    height = size,

                    paddingBottom = 0F,
                    paddingLeft =0F,
                    paddingRight = 0F,
                    paddingTop = 0F,
                }
        };

        targetBtn.Add(new VisualElement()
        {
            style =
                {
                    backgroundImage = imgs["AlignUtility." + iconName],

                    width = iconSize,
                    height = iconSize,

                    marginBottom = (size - iconSize)/2,
                    marginLeft = (size - iconSize)/2,
                    marginRight = (size - iconSize)/2,
                    marginTop = (size - iconSize)/2,
                }
        });

        return targetBtn;
    }

    void UseAlignInSelections(OnionAlignUtility.AlignHorizontal alignHorizontal)
    {
        var s = Selection.GetFiltered<Transform>(SelectionMode.ExcludePrefab);
        if (s.Length > 0)
            s.SetHorizontalAlign(alignHorizontal);
    }
    void UseAlignInSelections(OnionAlignUtility.AlignVertical alignVertical)
    {
        var s = Selection.GetFiltered<Transform>(SelectionMode.ExcludePrefab);
        if (s.Length > 0)
            s.SetVerticalAlign(alignVertical);
    }
}


#endif
