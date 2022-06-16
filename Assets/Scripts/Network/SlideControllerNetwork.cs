using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideControllerNetwork : NetworkBehaviour
{
    public static SlideControllerNetwork Instance;
    [SerializeField] SlideController SlideController;

    [SyncVar (hook = nameof(SetIndex))] int slideIndex; 

    private void Awake()
    {
        Instance = this;
    }

    public void SetIndex(int oldIndex, int newIndex)
    {
        Debug.Log(newIndex);
        SlideController.XSlide(SlideController.GetSlide(newIndex));
    }

    [Command(requiresAuthority = false)]
    public void CmdSetIndexFromAdmin(int index)
    {
        slideIndex = index;
    }
                                 
}
