using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logView : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Text _txt;
    [SerializeField] Button _btnClose;
    [SerializeField] Button _btnClear;
    [SerializeField] Button _btnAutoScroll;
    public void log<T>(T v) {
        if(_txt != null)
            _txt.text += $"{DateTime.Now.ToString("HH:mm:ss")}|{v.ToString()}\n";
    }


    public void logCb(string condition, string stackTrace, LogType type) {
        log(condition);
    }

    public void Start() {
        Application.logMessageReceived += logCb;
        _btnClose?.onClick.AddListener(() => {
        });
        _btnClear?.onClick.AddListener(() => {
            this._txt.text = "";
            this.log("cleared.");
        });

        _btnAutoScroll?.onClick.AddListener(() => {
            this._txt.text = "";
            this.log("cleared.");
        });
    }
}
