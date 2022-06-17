using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNetworkIp : MonoBehaviour
{
    private MRTKTMPInputField mRTKTMPInputField;
    [SerializeField] NetworkManager NetworkManager;


    private void Start()
    {
        mRTKTMPInputField = GetComponent<MRTKTMPInputField>();
        mRTKTMPInputField.onValueChanged.AddListener(ChangeIp);
    }

    private void ChangeIp(string newValue)
    {
        NetworkManager.networkAddress = newValue;
    }


}
