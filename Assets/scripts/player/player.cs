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
    [SerializeField] item _holding;

    public event Action<interactable , interactable> onInteractableChged;


    public Vector3 inputV3 { get => _inputV3; private set => _inputV3 = value; }
    public float spd { get => _spd; private  set => _spd = value; }
    public float rotSpd { get => _rotSpd; set => _rotSpd = value; }

    public void setInteractable(interactable obj = null)
    {
        var pre = _interactable;
        _interactable = obj;
        if(pre != _interactable)
            onInteractableChged?.Invoke( pre, _interactable);
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
        input.ins.onInteract += (CallbackContext ctx) => { _interactable?.interact(this); };
        input.ins.onProcess += (CallbackContext ctx) => { _interactable?.process(); };
        onInteractableChged += gameMgr.ins.onInteractableChged;

        if (_objAnchor == null) throw new Exception($"{transform.tag} does not have a obj_anchor.");
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

    public bool receive(item i)
    {
        var ret = false;
        do
        {
            if (_holding != null)
            {
                if (_holding.receipt.objName.Equals("plate")) { //holding combination validation here
                    if (i.receipt.objName.Equals("cheese")) {
                        var k = 0;
                    }
                }
                else {
                    Debug.LogError($"receive {_holding.name} failed exists in {transform.name}.");
                    break;
                }
            }
            _holding = i;
            var itemTrans = i.transform;
            itemTrans.SetParent(_objAnchor);
            itemTrans.localPosition = Vector3.zero;
            Debug.Log($"received {_holding.name} {transform.name}.");
            ret = true;
        } while (false);
        return ret;
    }

    public virtual item holding(item i = null)
    {
        return null;
    }

    public item remove(item i = null)
    {
        item ret = null;
        do
        {
            if (i == null)
            {
                if (_holding != null)
                {
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
        Debug.Log($"removed {ret?.name} from {transform.name} ret: {ret != null}.");
        return ret;
    }
}

