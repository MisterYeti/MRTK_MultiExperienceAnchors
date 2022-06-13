using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FadableCanvasGroup))]
public class Slide : MonoBehaviour
{
    private bool _startHidden = true;
    private FadableCanvasGroup _fadable;
    private CanvasGroup _canvasGroup;

    [Header("Event triggered when the canvas is fully shown")]
    public UnityEvent OnSlideShown;
    [Header("Event triggered when the canvas is fully hidden")]
    public UnityEvent OnSlideHidden;

    [SerializeField] protected bool _autoTransition = true;
    private Action _userActionOnFade;


    public bool AutoTransition { get => _autoTransition; set => _autoTransition = value; }

    protected virtual void Awake()
    {
        Init();
    }

    private void Init()
    {

        _fadable = GetComponent<FadableCanvasGroup>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_startHidden)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.0f;
        }
        gameObject.SetActive(false);
    }


    public virtual void Show(Action onShow, float duration)
    {
        gameObject.SetActive(true);
        _userActionOnFade = onShow;
        _fadable.OnFadeIn.RemoveListener(TriggerSlideShowEvent);
        _fadable.OnFadeIn.AddListener(TriggerSlideShowEvent);
        _fadable.FadeIn(duration,0f);
    }

    public virtual void Show(Action onShow)
    {
        gameObject.SetActive(true);
        Show(onShow, _fadable.FadeDuration);
    }


    public virtual void Hide(Action onHide, float duration)
    {
        _userActionOnFade = onHide;
        _fadable.OnFadeOut.RemoveListener(TriggerSlideHideEvent);
        _fadable.OnFadeOut.AddListener(TriggerSlideHideEvent);
        _fadable.FadeOut(duration);
    }

    public virtual void Hide(Action onHide)
    {
        Hide(onHide, _fadable.FadeDuration);
    }

    private void TriggerSlideHideEvent()
    {
        TriggerSlideEvent(false);
    }

    private void TriggerSlideShowEvent()
    {
        TriggerSlideEvent(true);
    }

    private void TriggerSlideEvent(bool fadeIn)
    {
        if (fadeIn)
        {
            OnSlideShown?.Invoke();
        }
        else
        {
            OnSlideHidden?.Invoke();
        }
        _userActionOnFade?.Invoke();
        _userActionOnFade = null;
    }

}
