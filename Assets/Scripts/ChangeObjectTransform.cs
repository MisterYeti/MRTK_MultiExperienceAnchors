using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectTransform : MonoBehaviour
{
    [SerializeField] GameObject _objectToMove;

    [SerializeField] Vector3 _PosVertical;
    [SerializeField] Vector3 _RotVertical;

    [SerializeField] Vector3 _PosHorizontal;
    [SerializeField] Vector3 _RotHorizontal;


    public void Horizontal()
    {
        _objectToMove.transform.localPosition = _PosHorizontal;
        _objectToMove.transform.rotation = Quaternion.Euler(_RotHorizontal);
    }

    public void Vertical()
    {
        _objectToMove.transform.localPosition = _PosVertical;
        _objectToMove.transform.rotation = Quaternion.Euler(_RotVertical);
    }
}
