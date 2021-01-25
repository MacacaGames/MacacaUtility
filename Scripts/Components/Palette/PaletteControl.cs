using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaletteControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Main Gradient Color")]
    public Gradient gradient = null;

    [Header("Shadow Image Color")]
    public Image shadowImage = null;
    public Shadow shadowComponent = null;

    [SerializeField]
    SelectablePalette palette;

    [SerializeField]
    Selectable selectable;
    void Awake()
    {
        if (selectable == null)
        {
            selectable = GetComponent<Selectable>();
        }
    }

    void Update()
    {
        if (lastSelectionState != currentSelectionState)
        {
            lastSelectionState = currentSelectionState;
            OnSelectionStateChagne(currentSelectionState);
        }
    }

    void OnSelectionStateChagne(SelectionState state)
    {
        SelectablePalette.PaletteSet paletteSet;
        switch (state)
        {
            case SelectionState.Normal:
                paletteSet = palette.GetNormalColor();
                break;
            case SelectionState.Highlighted:
                paletteSet = palette.GetHighlightedColor();
                break;
            case SelectionState.Pressed:
                paletteSet = palette.GetPressedColor();
                break;
            case SelectionState.Selected:
                paletteSet = palette.GetSelectedColor();
                break;
            case SelectionState.Disabled:
                paletteSet = palette.GetDisabledColor();
                break;
            default:
                paletteSet = palette.GetNormalColor();
                break;
        }
        palette.ChangeColor(paletteSet, shadowImage, shadowComponent, gradient);
    }

    SelectionState lastSelectionState;
    protected SelectionState currentSelectionState
    {
        get
        {
            if (!selectable.IsInteractable())
                return SelectionState.Disabled;
            if (isPointerDown)
                return SelectionState.Pressed;
            if (hasSelection)
                return SelectionState.Selected;
            if (isPointerInside)
                return SelectionState.Highlighted;
            return SelectionState.Normal;
        }
    }

    private bool isPointerInside { get; set; }
    private bool isPointerDown { get; set; }
    private bool hasSelection { get; set; }
    void InstantClearState()
    {
        isPointerInside = false;
        isPointerDown = false;
        hasSelection = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
    }

    void OnEnable()
    {
        isPointerDown = false;
    }

    void OnDisable()
    {
        InstantClearState();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        hasSelection = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        hasSelection = true;
    }

    /// <summary>
    /// An enumeration of selected states of objects
    /// </summary>
    protected enum SelectionState
    {
        /// <summary>
        /// The UI object can be selected.
        /// </summary>
        Normal,

        /// <summary>
        /// The UI object is highlighted.
        /// </summary>
        Highlighted,

        /// <summary>
        /// The UI object is pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// The UI object is selected
        /// </summary>
        Selected,

        /// <summary>
        /// The UI object cannot be selected.
        /// </summary>
        Disabled,
    }
}
