using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate() {
        if (Mathf.Abs(Vector3.Angle(transform.forward, Camera.main.transform.forward)) < 90)
            transform.forward = Camera.main.transform.forward;
        else
            transform.forward = -Camera.main.transform.forward;
    }
}
