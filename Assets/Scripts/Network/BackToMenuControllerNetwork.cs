using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuControllerNetwork : NetworkBehaviour
{
    [SerializeField] BackToMenuController backToMenuController;

    [Command(requiresAuthority = false)]
    public void CmdBackToMenuAsk()
    {
        RpcBackToMenuAsk();
    }

    [ClientRpc]
    public void RpcBackToMenuAsk()
    {
        backToMenuController.BackToMenuAsk();
    }

    [Command(requiresAuthority = false)]
    public void CmdCancelBackToMenu()
    {
        RpcCancelBackToMenu();
    }

    [ClientRpc]
    public void RpcCancelBackToMenu()
    {
        backToMenuController.CancelBackToMenu();
    }


}
