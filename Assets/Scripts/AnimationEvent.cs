using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] SlideController _slideController;
    [SerializeField] Slide _nextSlide;

    public void GoToNextSlide()
    {
        _slideController.XSlide(_nextSlide);
    }
}
