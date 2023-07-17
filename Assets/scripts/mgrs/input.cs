using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static utils;

public class input : monoSingleton<input> {
    public const string INPUT_SAVE = "input_map_save";
    [SerializeField] static inputControl _inputCtrl;
    public event Action<CallbackContext> onInteract;
    public event Action<CallbackContext> onProcess;
    public event Action<CallbackContext> onPause;

    List<keyBinds> _inputMap = new List<keyBinds>() { new keyBinds(){ key = "up", idx=1}, new keyBinds() { key = "down" , idx=2}, new keyBinds() { key = "left" , idx=3}, new keyBinds() { key = "right" , idx=4}
    , new keyBinds() { key = "active_1" }, new keyBinds() { key = "active_2" } };
    public List<keyBinds> inputMap { get => _inputMap; }
    // Start is called before the first frame update
    public override void Awake() {
        base.Awake();
        if (_inputCtrl == null) {
            _inputCtrl = new inputControl();
            if(PlayerPrefs.HasKey(INPUT_SAVE))
                _inputCtrl.LoadBindingOverridesFromJson(PlayerPrefs.GetString(INPUT_SAVE));
            _inputCtrl.player.Enable();
            _inputCtrl.player.interact.performed +=  ctx => ins?.onInteract(ctx);
            _inputCtrl.player.process.performed += ctx => ins?.onProcess(ctx);
            _inputCtrl.player.pause.performed +=  ctx => ins?.onPause(ctx);
        }
       
    }

    public class keyBinds {
        public string key;
        public int idx = 0;
    }

    (InputAction, int) get(string key) {
        InputAction bind = null;
        int idx = 0;
        switch (key) {
            case "up":
                bind = _inputCtrl.player.move;
                idx = 1;
                break;
            case "down":
                bind = _inputCtrl.player.move;
                idx = 2;
                break;
            case "left":
                bind = _inputCtrl.player.move;
                idx = 3;
                break;
            case "right":
                bind = _inputCtrl.player.move;
                idx = 4;
                break;
            case "active_1":
                bind = _inputCtrl.player.interact;
                break;
            case "active_2":
                bind = _inputCtrl.player.process;
                break;
            default:
                break;
        }
        return (bind, idx);
    } 

    public string getWord(string key) {
        var opt = get(key);
        return opt.Item1.bindings[opt.Item2].ToDisplayString();
    }



    public void rebind(string key, Action onDone = null) {
        var pair = get(key);
        _inputCtrl.player.Disable();
        pair.Item1.PerformInteractiveRebinding(pair.Item2).OnComplete(cb => {
            cb.Dispose();
            _inputCtrl.player.Enable();
            PlayerPrefs.SetString(INPUT_SAVE, _inputCtrl.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            if (onDone != null) onDone();
        }).Start();
    }

    public void updateInput(ref Vector3 input) {
        var inputV2 = _inputCtrl.player.move.ReadValue<Vector2>();
        input.x = inputV2.x;
        input.y = 0;
        input.z = inputV2.y;
    }
}
