using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class cuttingCounter : counter {
    // Start is called before the first frame update
    [SerializeField] processableCnf[] _cnfs;
    [SerializeField] Dictionary<ITEM_MSK, processableCnf> _cnfsMap = new Dictionary<ITEM_MSK, processableCnf>();
    [SerializeField] progressBar _progressBar;
    [SerializeField] protected counterAnim _counterAnim;
    [SerializeField] processableCnf _holdingCnf;
    public override void Start() {
        Assert.IsTrue(_progressBar);
        _counterAnim = GetComponentInChildren<counterAnim>();
        Assert.IsTrue(_counterAnim);
        _progressBar.gameObject.SetActive(false);
        foreach (var c in _cnfs) _cnfsMap[c.input.msk] = c;
        _counterAnim.OnAnimEvt += () => {
            var progress = holding().updateProgress(1);
            Debug.Log($"cutting for {holding().cnf} -> {_holdingCnf.output} progress {progress}");
            if (progress < _holdingCnf.progress) {
                _progressBar.progress((float)progress / _holdingCnf.progress);
                return;
            }
            _progressBar.display(false);
            var item = Instantiate(_holdingCnf.output.prefab).GetComponent<item>();
            if (item == null)
                throw new Exception($"{_holdingCnf.output.prefab.name} does not have item compoent itself");
            Destroy(remove(holding()).gameObject);
            receive(item);
        };
    }

    // Update is called once per frame
    void Update() {

    }

    public override bool receive(item i) {
        var ret = base.receive(i);
        if (ret) {
            _holdingCnf = null;
            if (i.cnf.type == ITEM_TYPE.RAW)
                _cnfsMap.TryGetValue(i.cnf.msk, out _holdingCnf);
        }
        return ret;
    }

    public override item remove(item i) {
        var ret = base.remove(i);
        if (ret) {
            _progressBar.display(false);
            _holdingCnf = null;
        }
        return ret;
    }

    public override void process() {
        if (holding() == null || _holdingCnf == null)
            return;
        if (_counterAnim.playing())
            return;
        _counterAnim.play();
    }
}
