using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class scoreFlag : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Animator _animtor;
    [SerializeField] GameObject _succ;
    [SerializeField] GameObject _fail;

    void Start() {
        _succ.SetActive(false);
        _fail.SetActive(false);
        Assert.IsNotNull(_animtor); Assert.IsNotNull(_succ); Assert.IsNotNull(_fail);
        deliveryMgr.ins.onDeliver += succ => {
            _succ.SetActive(succ);
            _fail.SetActive(!succ);
            _animtor.SetTrigger("flag");
        };
    }

    // Update is called once per frame
    void Update() {

    }
}
