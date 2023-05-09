using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class item : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] protected itemCnf _cnf;
    [SerializeField] float _progress = 0;

    public itemCnf cnf { get => _cnf; private set => _cnf = value; }

    void Start() {
        if (_cnf == null) Debug.LogWarning($"item {transform.name} does not have itemCnf");
    }
    public float updateProgress(float u = 1) {
        _progress += u;
        return _progress;
    }
}
