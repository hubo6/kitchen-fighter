using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] receipt _receipt;
    void Start()
    {
        if (_receipt) Debug.LogWarning($"item {transform.name} does not have receipt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
