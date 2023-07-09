using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class counterAnim : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] string _animTrigger;
    public event Action OnAnimEvt;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected bool _playing;
    void Start() {
        Assert.IsNotNull(_animator);
        _playing = false;
    }

    // Update is called once per frame
    public void onTriggerAnimFinshed() {
        _playing = false;
        OnAnimEvt?.Invoke();

    }

    public virtual bool play() {
        if (_playing)
            return false;
        _playing = true;
        _animator.SetTrigger(_animTrigger);
        return true;
    }

    public virtual bool playing() {
        return _playing; //todo  index magic
    }
}
