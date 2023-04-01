using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] float _xInput;
    [SerializeField] float _zInput;
    [SerializeField] Vector3 _inputV3 = Vector3.zero;
    [SerializeField] float _spd = 6f;
    [SerializeField] float _rotSpd = 6f;

    public Vector3 inputV3 { get => _inputV3; private set => _inputV3 = value; }
    public float spd { get => _spd; private  set => _spd = value; }
    public float rotSpd { get => _rotSpd; set => _rotSpd = value; }

    void Start() {
       
    }


    void updateInput() {
        _inputV3.x = Input.GetAxisRaw("Horizontal");
        _inputV3.z = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update() {
        input.ins.updateInput(ref _inputV3);
        //updateInput();
     
      

    }
}
