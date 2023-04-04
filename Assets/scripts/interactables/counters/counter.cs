using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter : interactable, owner  {
    // Start is called before the first frame update
    [SerializeField] protected Transform _objAnchor;

    [SerializeField] protected item _holding;




    public override INTERACT_TYPE type() { 
        return INTERACT_TYPE.COUNTER; 
    }

    public virtual void Start() {

    }

    public override bool interact(owner src) {

        var ret = false; 
        do {
            ret = base.interact(src);
            if (ret) 
                break;
            if (_holding != null)
            {
                if (src.receive(_holding))
                {
                    remove(_holding);
                    ret = true;
                }
                break;
            }
            var recv = src.remove();
            if (recv)
            {
                receive(recv);
                ret = true;
            }
        } while (false);
        return ret;
    }

    public bool receive(item i)
    {
        if (_holding != null)
        {
            Debug.LogError($"receive {_holding.name} failed exists in {transform.name}.");
            return false;
        }
        _holding = i;
        var itemTrans = _holding.transform;
        itemTrans.SetParent(_objAnchor);
        itemTrans.localPosition = Vector3.zero;
        Debug.Log($"received {_holding.name} {transform.name}.");
        return true;

    }

    public item remove(item i = null) {
        item ret = null;
        do {
            if (i == null) {
                if (_holding != null)  {
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
        return ret;
    }
}
