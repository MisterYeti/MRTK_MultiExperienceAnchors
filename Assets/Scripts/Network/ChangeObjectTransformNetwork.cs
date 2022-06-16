using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectTransformNetwork : NetworkBehaviour
{
    [SerializeField] ChangeObjectTransform ChangeObjectTransform;


    [Command(requiresAuthority = false)]
    public void CmdHorizontal()
    {
        RpcHorizontal();
    }

    [ClientRpc]
    public void RpcHorizontal()
    {
        ChangeObjectTransform.Horizontal();
    }

    [Command(requiresAuthority = false)]
    public void CmdVertical()
    {
        RpcVertical();
    }

    [ClientRpc]
    public void RpcVertical()
    {
        ChangeObjectTransform.Vertical();
    }

}
