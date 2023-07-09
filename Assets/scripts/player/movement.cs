using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

[RequireComponent(typeof(CapsuleCollider))]
public class movement : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] player _player;
    [SerializeField] LayerMask _maskLayer;
    [SerializeField] CapsuleCollider _capsuleCldr;
    [SerializeField] float[] _stepCounter = { 0, 0, 0, 0.3f }; //x, z ,cur, threshold
    RaycastHit _hit;
    void Start() {
        Assert.IsNotNull(_capsuleCldr);
    }

    // Update is called once per frame
    void Update() {
        //step counter;
        _stepCounter[2] += Time.deltaTime;
        if (_stepCounter[2] > _stepCounter[3]) {
            _stepCounter[2] = 0;
            if (Mathf.Abs(_stepCounter[0] - transform.position.x) > 0.01f || Mathf.Abs(_stepCounter[1] - transform.position.z) > 0.01f)
                soundMgr.ins.onMove(transform);
        }
        _stepCounter[0] = transform.position.x;
        _stepCounter[1] = transform.position.z;
        //step counter end;


        if (_player.inputV3 == Vector3.zero) return;
        var dis = _player.spd * Time.deltaTime;
        var offset = Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, _player.inputV3.normalized, _player.rotSpd * Time.deltaTime);
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _capsuleCldr.height, _capsuleCldr.radius, _player.inputV3.normalized, out RaycastHit hit, dis))
            offset = _player.inputV3.normalized * dis;
        else {
            var xInput = new Vector3(_player.inputV3.normalized.x, 0, 0);
            if (xInput != Vector3.zero) {
                var xDis = dis * Mathf.Abs(_player.inputV3.normalized.x);
                if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _capsuleCldr.height, _capsuleCldr.radius, xInput, xDis))
                    offset = xInput * dis;
            }


            var zInput = new Vector3(0, 0, _player.inputV3.normalized.z);
            if (zInput != Vector3.zero) {
                var zDis = dis * Mathf.Abs(_player.inputV3.normalized.z);
                if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _capsuleCldr.height, _capsuleCldr.radius, zInput, zDis))
                    offset = zInput * dis;
            }
        }
        transform.position += offset;
    }
}
