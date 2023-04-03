using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class player : MonoBehaviour, owner {
    // Start is called before the first frame update

    [SerializeField] float _xInput;
    [SerializeField] float _zInput;
    [SerializeField] Vector3 _inputV3 = Vector3.zero;
    [SerializeField] float _spd = 6f;
    [SerializeField] float _rotSpd = 6f;
    [SerializeField] float _interactDis = 1f;
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] interactable _interactable;
    [SerializeField] Transform _objAnchor;
    [SerializeField] Transform _holding;

    public event EventHandler<evtInteractableChged> onInteractableChged;


    public Vector3 inputV3 { get => _inputV3; private set => _inputV3 = value; }
    public float spd { get => _spd; private  set => _spd = value; }
    public float rotSpd { get => _rotSpd; set => _rotSpd = value; }

    public void setInteractable(interactable obj = null)
    {
        var pre = _interactable;
        _interactable = obj;
        if(pre != _interactable)
            onInteractableChged?.Invoke(this, new evtInteractableChged() { p = pre, n = _interactable });
    }


    void updateInteractable()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactDis, _interactableLayer))
        {
            setInteractable(null);
            return;
        }
        if (!hit.transform.TryGetComponent(out interactable obj))
            return;
        setInteractable(obj);
    }


    void Start() {
        input.ins.onInteract += this.onInteract;
        onInteractableChged += gameMgr.ins.onInteractableChged;
        if (_objAnchor == null) throw new Exception($"{transform.tag} does not have a obj_anchor.");
    }

    public void onInteract(CallbackContext ctx)
    {
         _interactable?.interact(this);

    }


    void updateInput() {
        _inputV3.x = Input.GetAxisRaw("Horizontal");
        _inputV3.z = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update() {
        input.ins.updateInput(ref _inputV3);
        updateInteractable();
        //updateInput();



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

    public Transform remove(Transform item = null)
    {
        Transform ret = null;
        do
        {
            if (item == null)
            {
                if (_holding != null)
                {
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

public class evtInteractableChged : EventArgs {
    public interactable p;
    public interactable n;
}
