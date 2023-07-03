using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logView : MonoSingleton<logView>
{
    // Start is called before the first frame update
    [SerializeField] Text _txt;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnClear;

    public void log<T>(T v)
    {
        _txt.text += $"{DateTime.Now.ToString("HH:mm:ss")}|{v.ToString()}\n";
    }


    public void logCb(string condition, string stackTrace, LogType type)
    {
        log(condition);
    }

    void Start()
    {
            Application.logMessageReceived += logCb;
            btnClose?.onClick.AddListener(() =>
            {
            });
            btnClear?.onClick.AddListener(() =>
            {
                ins._txt.text = "";
                ins.log("cleared.");
            });
    }
}
