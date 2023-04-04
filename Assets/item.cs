using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] receipt _receipt;
    [SerializeField] uint _progress = 0;

    public receipt receipt { get => _receipt; private set => _receipt = value; }

    void Start()
    {
        if (_receipt == null ) Debug.LogWarning($"item {transform.name} does not have receipt");
    }
    public uint updateProgress(uint u = 1) {
        _progress += u;
        return _progress;
    }

}
