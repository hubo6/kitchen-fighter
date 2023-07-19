using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class gameOverUi : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI _score;
    void Start() {
        Assert.IsNotNull(_score);
    }

    // Update is called once per frame
    void Update() {

    }
}
