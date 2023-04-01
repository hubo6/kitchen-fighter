using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class input : MonoSingleton<input>
{
    [SerializeField] inputControl _inputCtrl;
   // Start is called before the first frame update
    void Awake()
    {
        _inputCtrl = new inputControl();
        _inputCtrl.player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateInput(ref Vector3 input)
    {
        //if (_inputCtrl == null) {
        //    _inputCtrl = new Input_control();
        //    _inputCtrl.player.Enable();
        //}
        var inputV2 = _inputCtrl.player.move.ReadValue<Vector2>();
        input.x = inputV2.x;
        input.y = 0;
        input.z = inputV2.y;
    }
}
