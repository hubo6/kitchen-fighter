using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter : interactable, owner  {
    // Start is called before the first frame update
    [SerializeField] Transform _objAnchor;
    [SerializeField] receipt _receipt;
    [SerializeField] Transform _holding;

    public override interactableType type() { 
        return interactableType.COUNTER; 
    }

    private void Start()
    {
        if (_receipt == null)
            throw new System.Exception($"counter receipt is null");
    }

    public override void interact(owner player) {

        base.interact(player);
        if (_holding != null)
        {
            var recv = player.receive(_holding);
            Debug.Log($"{transform.name} trans {_holding.name} {recv}");
            if (recv)
                remove(_holding);
           
            return;
        }
        else
        {
            var item = Instantiate(_receipt.prefab);
            receive(item);
        }

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

    public bool remove(Transform item)
    {
        if (_holding != item || _holding == null || item == null)
        {
            Debug.LogError($"item remove {_holding?.name} {item?.name} failed in  {transform.name}.");
            return false;
        }
        Debug.Log($"item removed {item.name} {transform.name}.");
        _holding = null;
        return true;
    }
}
