using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerExample : MonoBehaviour
{
    [SerializeField] TimedSlide _timedSlide = null;
    [SerializeField] Text _text = null;
    private float _time;


    public void StartTimer()
    {
        _time = _timedSlide.TimeBeforeFadeOut;
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        float countdown = _time;
        while (countdown > 0)
        {
            _text.text = countdown.ToString();
            yield return new WaitForSeconds(1.0f);
            countdown--;
        }
        _text.text = countdown.ToString();
    }
}
