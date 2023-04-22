using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected receipt _receipt;
    [SerializeField] float _progress = 0;

    public receipt receipt { get => _receipt; private set => _receipt = value; }

    void Start()
    {
        if (_receipt == null ) Debug.LogWarning($"item {transform.name} does not have receipt");
    }
    public float updateProgress(float u = 1) {
        _progress += u;
        return _progress;
    }
}
