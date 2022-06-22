using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDiscoveryManager : MonoBehaviour
{
    [SerializeField] NetworkDiscovery networkDiscovery;
    [SerializeField] private float delayDiscover = 1.0f;

    [SerializeField] ButtonStatusController buttonHost;
    [SerializeField] ButtonStatusController buttonClient;

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
    }

    public void StartClient()
    {
        NetworkManager.singleton.StartClient(serverResponse.uri);
        StopCoroutine(DiscoverNetwork);
        buttonClient.SetStatus(false);
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
}
