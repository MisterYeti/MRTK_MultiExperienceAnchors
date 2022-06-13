using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSlide : Slide
{
    [SerializeField]
    private float _timeBeforeFadeOut = 5.0f;

    public float TimeBeforeFadeOut { get => _timeBeforeFadeOut; set => _timeBeforeFadeOut = value; }


    public override void Show(Action onShow)
    {
        OnSlideShown.AddListener(HideAfterTimer);
        base.Show(onShow);
    }

    public override void Show(Action onShow, float duration)
    {
        OnSlideShown.AddListener(HideAfterTimer);
        base.Show(onShow, duration);
    }

    private void HideAfterTimer()
    {
        OnSlideShown.RemoveListener(HideAfterTimer);
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(_timeBeforeFadeOut);
        base.Hide(null);
    }
}
