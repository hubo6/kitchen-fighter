using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class counter : interactable, owner {
    // Start is called before the first frame update
    [SerializeField] protected Transform _objAnchor;

    [SerializeField] protected item _holding;

    public static event Action<Transform> onPick;
    public static event Action<Transform> onDrop;


    public virtual item holding() {
        return _holding;
    }



    public override INTERACT_TYPE type() {
        return INTERACT_TYPE.COUNTER;
    }

    public virtual void Start() {

    }

    public override bool interact(owner src) {

        var ret = false;
        do {
            if (base.interact(src))
                break;
            var holdingItem = holding();
            if (holdingItem && src.receive(holdingItem)) {
                remove(holdingItem);
                ret = true;
                break;
            }
            holdingItem = src.holding();
            if (holdingItem && receive(holdingItem)) {
                src.remove(holdingItem);
                ret = true;
                break;
            }
        } while (false);
        return ret;
    }

    public static  void resetEvt() {
        onPick = onDrop = null;
    }
    public virtual bool receive(item i) {
        if (_holding != null) {
            Debug.LogError($"receive {_holding.name} failed exists in {transform.name}.");
            return false;
        }
        _holding = i;
        var itemTrans = _holding.transform;
        itemTrans.SetParent(_objAnchor);
        itemTrans.localPosition = Vector3.zero;
        Debug.Log($"received {_holding.name} {transform.name}.");
        onDrop?.Invoke(transform);
        return true;

    }

    public virtual item remove(item i = null) {
        item ret = null;
        do {
            if (i == null) {
                if (_holding != null) {
                    ret = _holding;
                    _holding = null;
                }
                break;
            }

            if (_holding != i)
                break;
            ret = _holding;
            _holding = null;
        } while (false);
        Debug.Log($"i removed {ret?.name} {transform.name} ret: {ret != null}.");
        if(ret) onPick?.Invoke(transform);
        return ret;
    }
}
