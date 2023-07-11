using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCam : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool invert = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate() {
        transform.forward =Camera.main.transform.forward  *  (invert ? -1 : 1);
    }
}
