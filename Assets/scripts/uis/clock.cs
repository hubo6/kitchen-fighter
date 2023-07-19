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
        gameMgr.ins.onTimePlayChg += (s) =>  _indicator.fillAmount = 1 - s / gameMgr.ins.timerStamp[2];
    }
}
