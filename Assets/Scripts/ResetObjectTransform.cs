using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectTransform : MonoBehaviour
{
    [SerializeField] GameObject _objectToReset;

    private Vector3 _initPos;
    private Quaternion _initRot;
    private Vector3 _initScale;

    private void Start()
    {
        _initPos = _objectToReset.transform.position;
        _initRot= _objectToReset.transform.rotation; 
        _initScale = _objectToReset.transform.localScale;
    }

    public void ResetObject()
    {
        _objectToReset.transform.position = _initPos;
        _objectToReset.transform.rotation = _initRot;
        _objectToReset.transform.localScale = _initScale;
    }
}
