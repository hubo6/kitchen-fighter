using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class item : NetworkBehaviour {
    // Start is called before the first frame update
    [SerializeField] protected itemCnf _cnf;
    [SerializeField] float _progress = 0;
    [SerializeField] Transform _follow;
    [SerializeField] Vector3 _followPosOffset = Vector3.zero;

    public itemCnf cnf { get => _cnf; private set => _cnf = value; }
    public Vector3 followTranOffset { get => _followPosOffset; set => _followPosOffset = value; }
    public Transform follow { get => _follow; set => _follow = value; }

    void Start() {
        if (_cnf == null) Debug.LogWarning($"item {transform.name} does not have itemCnf");
    }
    public float updateProgress(float u = 1) {
        _progress += u;
        return _progress;
    }

    public item setNetTransformParentAgent(Transform t = null, Vector3 posOffset = default(Vector3)) {
        _follow = t;
        _followPosOffset = posOffset;
        return this;
    }

    public void LateUpdate() {
        if (_follow == null)
            return;
        transform.position = _follow.position + _followPosOffset;
        transform.rotation = _follow.rotation;
    }
}
