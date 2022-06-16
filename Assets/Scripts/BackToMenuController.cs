using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuController : MonoBehaviour
{
    [SerializeField] SlideController _sliderController;
    [SerializeField] Slide _slideBackToMenu;

    private int currentSlideIndex;

    public void BackToMenuAsk()
    {
        currentSlideIndex = _sliderController._currentSlideIndex;
        _sliderController.XSlide(_slideBackToMenu);
    }

    public void CancelBackToMenu()
    {
        _sliderController.XSlide(_sliderController.GetSlide(currentSlideIndex));
    }
}
