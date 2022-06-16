using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerNetwork : NetworkBehaviour
{
    [SerializeField] AnimationTrigger animationTrigger;

    [Command(requiresAuthority = false)]
    public void CmdPlayAnimation()
    {
        RpcPlayAnimation();
    }

    [ClientRpc]
    public void RpcPlayAnimation()
    {
        animationTrigger.PlayAnimation();
    }
}
