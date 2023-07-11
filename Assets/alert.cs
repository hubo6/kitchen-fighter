using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class alert : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Animator _animtor;
    void Start() {
        Assert.IsNotNull(_animtor);
    }

    // Update is called once per frame
    void Update() {

    }

    public void display(bool flag) {
        if (gameObject.activeSelf != flag)
            gameObject.SetActive(flag);
        if (_animtor.GetBool("alert") != flag)
            _animtor.SetBool("alert", flag); 
    }
}
