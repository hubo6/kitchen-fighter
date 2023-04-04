using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class cuttingCounter : counter
{
    // Start is called before the first frame update
    [SerializeField] receiptCnf[] _receiptCnfs;
    [SerializeField] progressBar _progressBar;
    public override void Start()
    {
        if (_progressBar == null) throw new System.Exception($"_progress absent in {transform.name}.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void process()
    {
        if(_holding == null)
            return;

        receiptCnf cnf = null;
        for (var i = 0; i < _receiptCnfs.Length; i++) {
            if (_receiptCnfs[i].input == _holding.receipt) {
                cnf = _receiptCnfs[i];
                break;
            }
        }

        if (cnf == null)
            return;
        var progress = _holding.updateProgress(1);
        Debug.Log($"cutting for {_holding.receipt} -> {cnf.output} progress {progress}");
        if (progress < cnf.progress)
        {
            _progressBar.progress((float)progress / cnf.progress);
            return;
        }
        _progressBar.display(false);
        Destroy(remove(_holding).gameObject);
        receive(Instantiate(cnf.output.prefab).GetComponent<item>());
    }
}
