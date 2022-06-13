using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderstandSlide : Slide
{

    public bool isDone = false;



    private void Update()
    {
        if (isDone)
        {
            Hide(null);
        }
    }

}
