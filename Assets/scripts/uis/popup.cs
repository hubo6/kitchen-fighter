using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class popup : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] Transform _gameOver;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] Transform _options;


    void Start() {
        Assert.IsNotNull(_gameOver);
        Assert.IsNotNull(_options);
        Assert.IsNotNull(_score);
        gameObject.SetActive(false);
        gameMgr.ins.onStageChg += (gameMgr.STAGE s) => {
            if (s == gameMgr.STAGE.END) {
                gameObject.SetActive(true);
            } else if (s == gameMgr.STAGE.PAUSED) {
                gameObject.SetActive(true);
                gameObject.SetActive(true);
            }
        };
    }

    // Update is called once per frame
    void Update() {

    }
}
