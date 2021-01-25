using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

[CreateAssetMenu]
public class SelectablePalette : ScriptableObject
{
    public Color addHighlightedColor = new Color(0.05f, 0.05f, 0.05f, 0f);
    public Color addPressedColor = new Color(0.1f, 0.1f, 0.1f, 0f);

    const string Main_Gradient_Color = "Main Gradient Color";

    [Header(Main_Gradient_Color)]
#if ODIN_INSPECTOR
    [FoldoutGroup(Main_Gradient_Color)]
#endif
    public Color normalColorV1;

#if ODIN_INSPECTOR
    [FoldoutGroup(Main_Gradient_Color)]
#endif
    public Color normalColorV2;

#if ODIN_INSPECTOR
    [FoldoutGroup(Main_Gradient_Color)]
#endif
    public Color disabledColorV1;

#if ODIN_INSPECTOR
    [FoldoutGroup(Main_Gradient_Color)]
#endif
    public Color disabledColorV2;

    public Color highlightedColorV1
    {
        get
        {
            return normalColorV1 + addHighlightedColor;
        }
    }
    public Color highlightedColorV2
    {
        get
        {
            return normalColorV2 + addHighlightedColor;
        }
    }
    public Color pressedColorV1
    {
        get
        {
            return normalColorV1 + addPressedColor;
        }
    }
    public Color pressedColorV2
    {
        get
        {
            return normalColorV2 + addPressedColor;
        }
    }
    public Color selectedColorV1
    {
        get
        {
            return highlightedColorV1;
        }
    }
    public Color selectedColorV2
    {
        get
        {
            return highlightedColorV2;
        }
    }

    const string Shadow_Image_Color = "Shadow Image Color";
#if ODIN_INSPECTOR
    [FoldoutGroup(Shadow_Image_Color)]
#endif
    public Color shadowNormalColor;

#if ODIN_INSPECTOR
    [FoldoutGroup(Shadow_Image_Color)]
#endif
    public Color shadowDisabledColor;

    public Color shadowHighlightedColor
    {
        get
        {
            return shadowNormalColor + addHighlightedColor;

        }
    }
    public Color shadowPressedColor
    {
        get
        {
            return shadowNormalColor + addPressedColor;

        }
    }
    public Color shadowSelectedColor
    {
        get
        {
            return shadowHighlightedColor;
        }
    }


    public struct PaletteSet
    {
        public Color shadowColor;
        public Color color1;
        public Color color2;
    }

    public PaletteSet GetNormalColor()
    {
        return new PaletteSet
        {
            shadowColor = shadowNormalColor,
            color1 = normalColorV1,
            color2 = normalColorV2
        };
    }

    public PaletteSet GetHighlightedColor()
    {
        return new PaletteSet
        {
            shadowColor = shadowHighlightedColor,
            color1 = highlightedColorV1,
            color2 = highlightedColorV2
        };
    }

    public PaletteSet GetPressedColor()
    {
        return new PaletteSet
        {
            shadowColor = shadowPressedColor,
            color1 = pressedColorV1,
            color2 = pressedColorV2
        };
    }

    public PaletteSet GetSelectedColor()
    {
        return new PaletteSet
        {
            shadowColor = shadowSelectedColor,
            color1 = selectedColorV1,
            color2 = selectedColorV2
        };
    }

    public PaletteSet GetDisabledColor()
    {
        return new PaletteSet
        {
            shadowColor = shadowDisabledColor,
            color1 = disabledColorV1,
            color2 = disabledColorV2
        };
    }
    public void ChangeColor(PaletteSet paletteSet, Image shadowImage, Shadow shadowComponent, Gradient mainGradientComponent)
    {
        if (shadowImage != null & shadowImage?.color != paletteSet.shadowColor) shadowImage.color = paletteSet.shadowColor;
        if (shadowComponent != null & shadowComponent?.effectColor != paletteSet.shadowColor) shadowComponent.effectColor = paletteSet.shadowColor;
        if (mainGradientComponent != null)
        {
            if (mainGradientComponent.Vertex1 != paletteSet.color1) mainGradientComponent.Vertex1 = paletteSet.color1;
            if (mainGradientComponent.Vertex2 != paletteSet.color2) mainGradientComponent.Vertex2 = paletteSet.color2;
        }
#if UNITY_EDITOR
        if (mainGradientComponent) UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(mainGradientComponent);
        if (shadowComponent) UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(shadowComponent);
        if (shadowImage) UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(shadowImage);
#endif
    }
}