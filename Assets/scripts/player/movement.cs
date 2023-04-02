using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CapsuleCollider))]
public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] player _player;
    [SerializeField] LayerMask _maskLayer;
    CapsuleCollider _capsuleCldr;
    RaycastHit _hit;
    void Start()
    {
        _capsuleCldr = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.inputV3 == Vector3.zero) return;
        var dis = _player.spd * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, _player.inputV3.normalized, _player.rotSpd * Time.deltaTime);
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _capsuleCldr.height, _capsuleCldr.radius,_player.inputV3.normalized, out RaycastHit hit, dis))
            transform.position += _player.inputV3.normalized * dis;
        else
        {
            //if (hit.distance > 0.01f)
            //{
            //    transform.position += _player.inputV3.normalized * (hit.distance - 0.01f);
            //    return;
            //}
            var xInput = new Vector3(_player.inputV3.normalized.x, 0, 0);
            if (xInput != Vector3.zero)
            {
                var xDis = dis * Mathf.Abs(_player.inputV3.normalized.x);
                if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _capsuleCldr.height, _capsuleCldr.radius, xInput, xDis))
                    transform.position += xInput * dis;
            }


            var zInput = new Vector3(0, 0, _player.inputV3.normalized.z);
            if (zInput != Vector3.zero)
            {
                var zDis = dis * Mathf.Abs(_player.inputV3.normalized.z);
                if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _capsuleCldr.height, _capsuleCldr.radius, zInput, zDis))
                    transform.position += zInput * dis;
            }
        }



    }
}
