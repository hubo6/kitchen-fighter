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
    [SerializeField] protected counterAnim _counterAnim;
    [SerializeField] receiptCnf _holdingReciptCnf;
    public override void Start()
    {
        if (_progressBar == null)
            throw new System.Exception($"_progress absent in {transform.name}.");
        _counterAnim = GetComponentInChildren<counterAnim>();
        if (_counterAnim == null)
            throw new System.Exception($"counter receipt is null");
        _counterAnim.OnAnimEvt += () => {
            var progress = _holding.updateProgress(1);
            Debug.Log($"cutting for {_holding.receipt} -> {_holdingReciptCnf.output} progress {progress}");
            if (progress < _holdingReciptCnf.progress) {
                _progressBar.progress((float)progress / _holdingReciptCnf.progress);
                return;
            }
            _progressBar.display(false);
            Destroy(remove(_holding).gameObject);
            receive(Instantiate(_holdingReciptCnf.output.prefab).GetComponent<item>());
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool receive(item item)
    {
        var ret =  base.receive(item);
        if (ret) {
            _holdingReciptCnf = null;
            for (var i = 0; i < _receiptCnfs.Length; i++) {
                if (_receiptCnfs[i].input == _holding.receipt) {
                    _holdingReciptCnf = _receiptCnfs[i];
                    break;
                }
            }
        }
        return ret;
    }

    public override item remove(item i)
    {
        var ret = base.remove(i);
        if (ret)
        {
            _progressBar.display(false);
            _holdingReciptCnf = null;
        }
        return ret;
    }

    public override void process()
    {
        if(_holding == null)
            return;
        if (_holdingReciptCnf == null)
            return;
        if (_counterAnim.playing())
            return;
        _counterAnim.play();
    }
}
