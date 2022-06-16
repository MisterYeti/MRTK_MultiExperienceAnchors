using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderImagesControllerNetwork : NetworkBehaviour
{
    [SerializeField] SliderImagesController sliderImagesController;


    [Command(requiresAuthority = false)]
    public void CmdPress(bool right)
    {
        RpcPress(right);
    }

    [ClientRpc]
    public void RpcPress(bool right)
    {
        sliderImagesController.Press(right);
    }

}
