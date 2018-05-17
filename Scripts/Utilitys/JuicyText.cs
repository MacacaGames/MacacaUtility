﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
#if TextMeshPro
using TMPro;
#endif
namespace CloudMacaca
{
#if TextMeshPro
    [RequireComponent(typeof(TextMeshProUGUI))]
#else
    [RequireComponent(typeof(Text))]
#endif
    public class JuicyText : MonoBehaviour
    {
        public enum ScaleTarget{
            Self,Parent
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
        TextMeshProUGUI _textComponent;
        public TextMeshProUGUI textComponent
        {
            get
            {
                if (_textComponent == null)
                    _textComponent = GetComponent<TextMeshProUGUI>();
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
        [SerializeField]
        Ease animationEaseType = Ease.OutCubic;

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

        public Coroutine PlayAnimation()
        {
            return SetTextAnimation(textComponent.text);
        }

        public Coroutine SetTextAnimation(string text)
        {
            if (currentTextCoroutine != null)
                StopCoroutine(currentTextCoroutine);

            if (_particleToPlay != null)
                _particleToPlay.Play();
            // if (_sfxToPlay != null)
            //     AudioManager.PlaySFX(_sfxToPlay);

            currentTextCoroutine = CoroutineManager.Instance.StartCoroutine(ChangeTextAnimation(text));
            return currentTextCoroutine;
        }


        public Coroutine SetNumberAnimation(int number, float numberDuration, string extraTextInFront = "", string extraTextInEnd = "", float delay = 0)
        {
            if (currentNumberCoroutune != null)
                CoroutineManager.Instance.StopCoroutine(currentNumberCoroutune);

            currentNumberCoroutune = CoroutineManager.Instance.StartCoroutine(ChangeNumberAnimation(number, numberDuration, extraTextInFront, extraTextInEnd, delay));
            return currentNumberCoroutune;
        }
        IEnumerator ChangeTextAnimation(string text)
        {
            SetTextInstant(text);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
            if(scaleTarget == ScaleTarget.Self)
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

        void SetTextLerpView(float param)
        {
            Color c = initShadowColor;
            if (isHasShadow)
                shadow.effectColor = new Color(c.r, c.g, c.b, Mathf.Lerp(0, c.a, param));

            float ease = DOVirtual.EasedValue(0, 1, param, animationEaseType);
            if(scaleTarget == ScaleTarget.Self)
                textComponent.transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.one, ease);
            else
                textComponent.transform.parent.localScale = Vector3.LerpUnclamped(startScale, Vector3.one, ease);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, ease);
        }

        [SerializeField]
        Ease numberEaseType = Ease.InOutCubic;


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
                float easedValue = DOVirtual.EasedValue(startNumber, targetNumber, progress, numberEaseType);
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
