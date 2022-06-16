using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderImagesController : MonoBehaviour
{
    [SerializeField] Slide slide;

    [SerializeField] List<GameObject> _points;
    [SerializeField] List<GameObject> _images;

    private int index = 0;

    private void Start()
    {
        slide.OnSlideHidden.AddListener(Reinitialize);
    }

    private void Reinitialize()
    {
        index = 0;

        _points[index].SetActive(true);
        _images[index].SetActive(true);

        for (int i = 0; i < _points.Count; i++)
        {
            if (index != i)
            {
                _points[i].SetActive(false);
                _images[i].SetActive(false);
            }
        }
    }

    public void Press(bool right)
    {
        index = right ? index += 1 : index - 1;

        if (index < 0)
        {
            index = _points.Count - 1;
        }
        if (index > _points.Count - 1)
        {
            index = 0;
        }

        _points[index].SetActive(true);
        _images[index].SetActive(true);

        for (int i = 0; i < _points.Count; i++)
        {
            if (index != i)
            {
                _points[i].SetActive(false);
                _images[i].SetActive(false);
            }
        }
    }


}
