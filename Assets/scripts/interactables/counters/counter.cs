using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter : interactable, owner  {
    // Start is called before the first frame update
    [SerializeField] protected Transform _objAnchor;

    [SerializeField] protected Transform _holding;




    public override interactableType type() { 
        return interactableType.COUNTER; 
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

    public bool receive(Transform item)
    {
        if (_holding != null)
        {
            Debug.LogError($"item receive {_holding.name} failed exists in {transform.name}.");
            return false;
        }
        _holding = item;
        _holding.SetParent(_objAnchor);
        _holding.localPosition = Vector3.zero;
        Debug.Log($"item received {_holding.name} {transform.name}.");
        return true;

    }

    public Transform remove(Transform item = null) {
        Transform ret = null;
        do {
            if (item == null) {
                if (_holding != null)  {
                    ret = _holding;
                    _holding = null;
                }
                break;
            }

            if (_holding != item)
                break;
            ret = _holding;
            _holding = null;
        } while (false);
        Debug.Log($"item removed {ret?.name} {transform.name} ret: {ret != null}.");
        return ret;
    }
}
