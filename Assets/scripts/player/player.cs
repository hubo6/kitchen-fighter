using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class player : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] float _xInput;
    [SerializeField] float _zInput;
    [SerializeField] Vector3 _inputV3 = Vector3.zero;
    [SerializeField] float _spd = 6f;
    [SerializeField] float _rotSpd = 6f;
    [SerializeField] float _interactDis = 1f;
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] interactable _interactable;

    public Vector3 inputV3 { get => _inputV3; private set => _inputV3 = value; }
    public float spd { get => _spd; private  set => _spd = value; }
    public float rotSpd { get => _rotSpd; set => _rotSpd = value; }


    void updateInteractable()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactDis, _interactableLayer))
        {
            _interactable?.highlight(false);
            _interactable = null;
            return;
        }
        if (!hit.transform.TryGetComponent(out interactable obj))
            return;
        if (_interactable != obj)
        {
            _interactable?.highlight(false);
            _interactable = obj;
        }
        _interactable?.highlight(true);
        Debug.Log(_interactable?.transform.tag);
    }


    void Start() {
        input.ins.onInteract += this.onInteract;
    }

    public void onInteract(CallbackContext ctx)
    {
       

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
}
