using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if TextMeshPro
using TMPro;
#endif
namespace MacacaGames
{
#if TextMeshPro
    [RequireComponent(typeof(TMP_Text))]
#else
    [RequireComponent(typeof(Text))]
#endif
    public class JuicyText : MonoBehaviour
    {
        public enum ScaleTarget
        {
            Self, Parent
        }
        public ScaleTarget scaleTarget = ScaleTarget.Self;
        RectTransform __transform;
        public RectTransform _transform
        {
            get
            {
                if (__transform == null) __transform = GetComponent<RectTransform>();
                return __transform;
            }
        }
#if TextMeshPro
        TMP_Text _textComponent;
        public TMP_Text textComponent
        {
            get
            {
                if (_textComponent == null)
                    _textComponent = GetComponent<TMP_Text>();
                return _textComponent;
            }
        }
#else
        Text _textComponent;
        public Text textComponent
        {
            get
            {
                if (_textComponent == null)
                    _textComponent = GetComponent<Text>();
                return _textComponent;
            }
        }
#endif


        bool isHasShadow { get { return shadow != null; } }
        Shadow shadow;
        Color initShadowColor;

        [SerializeField]
        Vector3 startScale = Vector3.one * 2f;
        [SerializeField]
        float animationDuration = .5f;
        // [SerializeField]
        // EaseStyle animationEaseType = Ease.OutCubic;
        [SerializeField]
        EaseStyle animationEaseType = EaseStyle.CubicEaseOut;

        Coroutine currentTextCoroutine;
        Coroutine currentNumberCoroutune;

        private void Awake()
        {
            shadow = GetComponent<Shadow>();
            if (isHasShadow)
                initShadowColor = shadow.effectColor;
        }

        public void ClearTextInstant()
        {
            SetTextInstant(string.Empty);
        }

        public void SetTextInstant(string text)
        {
            textComponent.text = text;
        }

        [SerializeField]
        ParticleSystem _particleToPlay;
        [SerializeField]
        AudioClip _sfxToPlay;

        public Coroutine PlayAnimation(Vector3 startScale, float animationDuration, EaseStyle animationEaseType, bool isPlayParticle = true)
        {
            return SetTextAnimation(textComponent.text, startScale, animationDuration, animationEaseType, isPlayParticle);
        }
        public Coroutine SetTextAnimation(string text, bool isPlayParticle = true)
        {
            return SetTextAnimation(text, startScale, animationDuration, animationEaseType, isPlayParticle);
        }
        public Coroutine SetTextAnimation(string text, Vector3 startScale, float animationDuration, EaseStyle animationEaseType, bool isPlayParticle)
        {
            if (currentTextCoroutine != null)
                StopCoroutine(currentTextCoroutine);

            if (_particleToPlay != null)
                _particleToPlay.Play();
            // if (_sfxToPlay != null)
            //     AudioManager.PlaySFX(_sfxToPlay);

            currentTextCoroutine = CoroutineManager.Instance.StartCoroutine(ChangeTextAnimation(text, startScale, animationDuration, animationEaseType));
            return currentTextCoroutine;
        }

        public Coroutine SetNumberAnimation(int number, float numberDuration, string extraTextInFront = "", string extraTextInEnd = "", float delay = 0)
        {
            if (currentNumberCoroutune != null)
                CoroutineManager.Instance.StopCoroutine(currentNumberCoroutune);

            currentNumberCoroutune = CoroutineManager.Instance.StartCoroutine(ChangeNumberAnimation(number, numberDuration, extraTextInFront, extraTextInEnd, delay));
            return currentNumberCoroutune;
        }
        IEnumerator ChangeTextAnimation(string text, Vector3 startScale, float animationDuration, EaseStyle animationEaseType)
        {
            SetTextInstant(text);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
            if (scaleTarget == ScaleTarget.Self)
                textComponent.transform.localScale = startScale;
            else
                textComponent.transform.parent.localScale = startScale;

            float progress = 0;
            while (progress < animationDuration)
            {
                progress += Time.deltaTime;
                float param = progress / animationDuration;
                SetTextLerpView(param);
                yield return null;
            }
            SetTextLerpView(1);
        }

        [SerializeField]
        bool isChangeOpacity = true;
        void SetTextLerpView(float param)
        {
            float ease = EaseUtility.EasedLerp(0, 1, param, animationEaseType);
            if (scaleTarget == ScaleTarget.Self)
                textComponent.transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.one, ease);
            else
                textComponent.transform.parent.localScale = Vector3.LerpUnclamped(startScale, Vector3.one, ease);

            Color c = initShadowColor;
            if (isChangeOpacity)
            {
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, ease);
                if (isHasShadow)
                    shadow.effectColor = new Color(c.r, c.g, c.b, Mathf.Lerp(0, c.a, param));
            }
            else
            {
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1);
                if (isHasShadow)
                    shadow.effectColor = new Color(c.r, c.g, c.b, Mathf.Lerp(0, c.a, 1));
            }
        }

        // [SerializeField]
        // EaseStyle numberEaseType = Ease.InOutCubic;
        [SerializeField]
        EaseStyle numberEaseType = EaseStyle.CubicEaseInOut;


        IEnumerator ChangeNumberAnimation(int targetNumber, float numberDuration, string extraTextInFront = "", string extraTextInEnd = "", float delay = 0)
        {
            if (delay != 0) yield return new WaitForSeconds(delay);
            int startNumber;
            if (!int.TryParse(textComponent.text, out startNumber))
                startNumber = 0;

            float progress = 0;
            float v = 1f / numberDuration;
            int lastNumber = startNumber;
            while (progress <= 1)
            {
                progress += v * Time.deltaTime;
                float easedValue = EaseUtility.EasedLerp(startNumber, targetNumber, progress, numberEaseType);
                int tmp = Mathf.FloorToInt(easedValue);
                if (lastNumber != tmp)
                {
                    lastNumber = tmp;
                    SetTextAnimation(extraTextInFront + lastNumber.ToString() + extraTextInEnd);
                }
                yield return null;
            }
            SetTextAnimation(extraTextInFront + targetNumber.ToString() + extraTextInEnd);
        }
        public void SetTextColor(Color color)
        {
            textComponent.color = color;
        }
    }

}
