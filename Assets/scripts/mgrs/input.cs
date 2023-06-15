using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class input : MonoSingleton<input> {
    [SerializeField] static inputControl _inputCtrl;
    public event Action<CallbackContext> onInteract;
    public event Action<CallbackContext> onProcess;
    public event Action<CallbackContext> onPause;
    // Start is called before the first frame update
    void Awake() {
        if (_inputCtrl == null) {
            _inputCtrl = new inputControl();
            _inputCtrl.player.Enable();
            _inputCtrl.player.interact.performed += (CallbackContext ctx) => ins?.onInteract(ctx);
            _inputCtrl.player.process.performed += (CallbackContext ctx) => ins?.onProcess(ctx);
            _inputCtrl.player.pause.performed += (CallbackContext ctx) => ins?.onPause(ctx);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void updateInput(ref Vector3 input) {
        var inputV2 = _inputCtrl.player.move.ReadValue<Vector2>();
        input.x = inputV2.x;
        input.y = 0;
        input.z = inputV2.y;
    }
}
