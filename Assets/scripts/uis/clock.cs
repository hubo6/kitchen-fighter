using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class clock : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Image _indicator;
    void Start() {
        Assert.IsNotNull(_indicator);
        gameObject.SetActive(false);
        gameMgr.ins.onStageChg +=  s => {
            if (s == gameMgr.STAGE.STARTED)
                gameObject.SetActive(true);
            else if (s == gameMgr.STAGE.END)
                gameObject.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update() {
        _indicator.fillAmount = 1- gameMgr.ins.timerStamp[0] / gameMgr.ins.timerStamp[2];
    }
}
