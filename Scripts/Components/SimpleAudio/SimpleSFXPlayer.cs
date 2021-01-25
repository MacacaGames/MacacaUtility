using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class SimpleSFXPlayer : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler
{

    [SerializeField]
    private bool m_Interactable = true;
    private bool m_SelectableInteractable
    {
        get
        {
            if (m_SelectableCache != null)
            {
                return m_SelectableCache.interactable;
            }
            return true;
        }
    }
    private bool m_GroupsAllowInteraction = true;
    private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
    private UnityEngine.UI.Selectable m_SelectableCache;
    protected override void OnCanvasGroupChanged()
    {
        // Figure out if parent groups allow interaction
        // If no interaction is alowed... then we need
        // to not do that :)
        var groupAllowInteraction = true;
        Transform t = transform;
        while (t != null)
        {
            t.GetComponents(m_CanvasGroupCache);
            bool shouldBreak = false;
            for (var i = 0; i < m_CanvasGroupCache.Count; i++)
            {
                // if the parent group does not allow interaction
                // we need to break
                if (!m_CanvasGroupCache[i].interactable)
                {
                    groupAllowInteraction = false;
                    shouldBreak = true;
                }
                // if this is a 'fresh' group, then break
                // as we should not consider parents
                if (m_CanvasGroupCache[i].ignoreParentGroups)
                    shouldBreak = true;
            }
            if (shouldBreak)
                break;

            t = t.parent;
        }

        if (groupAllowInteraction != m_GroupsAllowInteraction)
        {
            m_GroupsAllowInteraction = groupAllowInteraction;
        }
    }

    public virtual bool IsInteractable
    {
        get
        {
            return m_GroupsAllowInteraction && m_Interactable && m_SelectableInteractable;
        }
    }


    [System.Serializable]
    public enum PlayTiming
    {
        OnEnable,
        OnDisable,
        OnHoverEnter,
        OnHoverExit,
        OnClickUp,
        OnClickDown,
        OnScroll,   // Mouse scroll
        DontPlay,
    }
    [System.Serializable]
    public class PlayerModule
    {
        public PlayTiming playTiming;
        public AudioClip clip;
    }
    PlayerModule _currentPlayerModule;
    [Tooltip("playOnAwake")]
    public bool playOnAwake = false;
    public PlayerModule[] playerModules;
    new void Start()
    {
        playOnAwake = true;
        m_SelectableCache = GetComponent<UnityEngine.UI.Selectable>();
    }


    void OnEnable()
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnEnable);
        Play();
    }

    void OnDisable()
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnDisable);
        Play();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnHoverEnter);
        Play();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnHoverExit);
        Play();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnClickDown);
        Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnClickUp);
        Play();
    }
    public void OnScroll(PointerEventData eventData)
    {
        _currentPlayerModule = playerModules.FirstOrDefault(m => m.playTiming == PlayTiming.OnScroll);
        Play();
    }

    public void Play()
    {
        if (IsInteractable == false) return;
        if (_currentPlayerModule == null) return;
        if (playOnAwake == false) return;
        OnPlaySfx?.Invoke(_currentPlayerModule.clip);
        _currentPlayerModule = null;
    }

    public static System.Action<AudioClip> OnPlaySfx;
}
