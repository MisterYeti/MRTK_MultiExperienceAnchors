using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    [SerializeField] private List<Slide> _slides = new List<Slide>();
    public int _currentSlideIndex = -1;

    [Header("All slides will have the same transition time")]
    [SerializeField] private bool _globalTimeTransition = true;
    [SerializeField] [HideInInspector] private float _transitionDuration = 0.5f;
    Action onNewSlideCachedAction;

    public bool GlobalTimeTransition { get => _globalTimeTransition;}
    public float TransitionDuration { get => _transitionDuration; set => _transitionDuration = value; }

    private void Start()
    {
        if (!_slides[0])
        {
            return;
        }

        NextSlide();
    }

    public void NextSlide()
    {
        NextSlide(null);
    }

    public void LastSlide()
    {
        LastSlide(null);
    }

    public void XSlide(Slide slide)
    {
        XSlide(null, slide);
    }


    public void NextSlide(Action onNewSlide)
    {
        
        if (_currentSlideIndex == -1)
        {
            if (_slides[_currentSlideIndex+1].AutoTransition)
            {
                _slides[_currentSlideIndex + 1].OnSlideHidden.AddListener(GoToNextSlide);
            }

            if (GlobalTimeTransition)
            {
                _slides[_currentSlideIndex + 1].Show(onNewSlide, TransitionDuration);
            }
            else
            {
                _slides[_currentSlideIndex + 1].Show(onNewSlide);
            }

            _currentSlideIndex = (_currentSlideIndex + 1) % _slides.Count;
            return;
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Hide(() => { ShowNextSlideCallback(onNewSlide); },TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Hide(() => { ShowNextSlideCallback(onNewSlide); });

        }

    }

    private void ShowNextSlideCallback(Action onNewSlide)
    {
        _currentSlideIndex = (_currentSlideIndex + 1) % _slides.Count;
        if (_slides[_currentSlideIndex].AutoTransition)
        {
            _slides[_currentSlideIndex].OnSlideHidden.AddListener(GoToNextSlide);
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Show(onNewSlide,TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Show(onNewSlide);
        }
    }

    private void GoToNextSlide()
    {
        _slides[_currentSlideIndex].OnSlideHidden.RemoveListener(GoToNextSlide);

        _currentSlideIndex = (_currentSlideIndex + 1) % _slides.Count;
        if (_currentSlideIndex == 0) { return; };
        if (_slides[_currentSlideIndex].AutoTransition)
        {
            _slides[_currentSlideIndex].OnSlideHidden.AddListener(GoToNextSlide);
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Show(null,TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Show(null);
        }

    }

    public void LastSlide(Action onNewSlide)
    {

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Hide(() => { ShowLastSlideCallback(onNewSlide); }, TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Hide(() => { ShowLastSlideCallback(onNewSlide); });

        }

    }
    private void ShowLastSlideCallback(Action onNewSlide)
    {
        _currentSlideIndex = (_currentSlideIndex - 1) % _slides.Count;
        if (_slides[_currentSlideIndex].AutoTransition)
        {
            _slides[_currentSlideIndex].OnSlideHidden.AddListener(GoToLastSlide);
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Show(onNewSlide, TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Show(onNewSlide);
        }
    }

    private void GoToLastSlide()
    {

        _slides[_currentSlideIndex].OnSlideHidden.RemoveListener(GoToLastSlide);

        _currentSlideIndex = (_currentSlideIndex - 1) % _slides.Count;
        if (_currentSlideIndex == 0) { return; };
        if (_slides[_currentSlideIndex].AutoTransition)
        {
            _slides[_currentSlideIndex].OnSlideHidden.AddListener(GoToLastSlide);
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Show(null, TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Show(null);
        }

    }

    public void XSlide(Action onNewSlide, Slide slide)
    {
        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Hide(() => { ShowXSlideCallBack(onNewSlide, slide); }, TransitionDuration);
            _currentSlideIndex = _slides.IndexOf(slide);
        }
        else
        {
            _slides[_currentSlideIndex].Hide(() => { ShowXSlideCallBack(onNewSlide, slide); });
            _currentSlideIndex = _slides.IndexOf(slide);
        }
    }

    public void ShowXSlideCallBack(Action onNewSlide, Slide slide)
    {
        _currentSlideIndex = _slides.FindIndex(a => a == slide) % _slides.Count;
        if (_slides[_currentSlideIndex].AutoTransition)
        {
            _slides[_currentSlideIndex].OnSlideHidden.AddListener(GoToNextSlide);
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Show(onNewSlide, TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Show(onNewSlide);
        }
    }

    private void GoToXSlide()
    {

        _slides[_currentSlideIndex].OnSlideHidden.RemoveListener(GoToXSlide);

        _currentSlideIndex = (_currentSlideIndex - 1) % _slides.Count;
        if (_currentSlideIndex == 0) { return; };
        if (_slides[_currentSlideIndex].AutoTransition)
        {
            _slides[_currentSlideIndex].OnSlideHidden.AddListener(GoToXSlide);
        }

        if (GlobalTimeTransition)
        {
            _slides[_currentSlideIndex].Show(null, TransitionDuration);
        }
        else
        {
            _slides[_currentSlideIndex].Show(null);
        }

    }
}
