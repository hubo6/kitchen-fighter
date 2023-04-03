using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class counterAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public event EventHandler OnAnimEvt;
    [SerializeField] protected Animator _animator;
    void Start() {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void onClosedAnim()
    {
        OnAnimEvt?.Invoke(this, EventArgs.Empty);
    }

    public virtual void play()
    {
        _animator.SetTrigger("OpenClose");
    }

    public virtual bool playing() {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f; //todo  index magic
    }
}
