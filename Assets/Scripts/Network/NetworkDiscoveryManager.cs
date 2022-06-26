using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDiscoveryManager : MonoBehaviour
{
    [SerializeField] NetworkDiscovery networkDiscovery;
    [SerializeField] AnchorModuleScript anchorModule;
    [SerializeField] private float delayDiscover = 1.0f;

    [SerializeField] ButtonStatusController buttonHost;
    [SerializeField] ButtonStatusController buttonClient;

    [SerializeField] List<ButtonStatusController> buttonsAnchors;

    private ServerResponse serverResponse;

    IEnumerator DiscoverNetwork;

    private void Start()
    {
        DiscoverNetwork = DiscoverNetworkCoroutine();
        StartCoroutine(DiscoverNetwork);
    }

    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        StopCoroutine(DiscoverNetwork);
        buttonHost.SetStatus(false);
        StartCoroutine(StartAzureCoroutine(true));
        
    }

    private IEnumerator StartAzureCoroutine(bool activateButtons)
    {
        yield return new WaitForSeconds(2f);
        anchorModule.StartAzureSession();
        yield return new WaitForSeconds(2f);
        if (activateButtons)
        {
            ActivateAnchorsButton();
        }
    }

    public void StartClient()
    {
        NetworkManager.singleton.StartClient(serverResponse.uri);
        StopCoroutine(DiscoverNetwork);
        buttonClient.SetStatus(false);
        StartCoroutine(StartAzureCoroutine(false));
        //ActivateAnchorsButton();
    }


    public void OnDiscoveredServer(ServerResponse info)
    {
        Debug.Log("Server discovered");
        serverResponse = info;
        buttonHost.SetStatus(false);
        buttonClient.SetStatus(true);
    }

    private IEnumerator DiscoverNetworkCoroutine()
    {   
        while (true)
        { 
            Debug.Log("Discovering network...");
            networkDiscovery.StartDiscovery();
            yield return new WaitForSeconds(delayDiscover);
        }
    }

    private void ActivateAnchorsButton()
    {
        foreach (ButtonStatusController button in buttonsAnchors)
        {
            button.SetStatus(true);
        }
    }
}
