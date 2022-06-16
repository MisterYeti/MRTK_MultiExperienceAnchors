using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] string _triggerName;

    //[SerializeField] GameObject _objectToDesactivate;

    public void PlayAnimation()
    {
        _animator.SetTrigger(_triggerName);
        //_objectToDesactivate.SetActive(false);
    }
}
