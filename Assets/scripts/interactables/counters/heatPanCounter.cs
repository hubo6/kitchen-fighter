using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class heatPanCounter : counter {
    // Start is called before the first frame update
    [SerializeField] processableCnf[] _processableCnfs;
    [SerializeField] Dictionary<itemCnf, processableCnf> _processableCnfsMap = new Dictionary<itemCnf, processableCnf>();
    [SerializeField] progressBar _progressBar;
    [SerializeField] processableCnf _holdingCnf;
    [SerializeField] GameObject _glowShim;
    [SerializeField] GameObject _splash;

    //protected processableCnf getReceiptCnfByInput(itemCnf cnf) {
    //    processableCnf reciptCnf = null;
    //    for (var i = 0; i < _processableCnfs.Length; i++)
    //    {
    //        if (_processableCnfs[i].input == cnf)
    //        {
    //            reciptCnf = _processableCnfs[i];
    //            break;
    //        }
    //    }
    //    return reciptCnf;
    //}

    protected IEnumerator progress(processableCnf cnf) {
        yield return null;

    }

    public override void Start() {
        if (_progressBar == null)
            throw new System.Exception($"_progress absent in {transform.name}.");
        processableCnf defect = Array.Find(_processableCnfs, i => {
            return i.input.msk != ITEM_MSK.MEAT_PIE && (i.input.type != ITEM_TYPE.RAW || i.input.type != ITEM_TYPE.PROCESSED);
        });
        if (defect)
            throw new System.Exception($"{transform.name} disfunction with {defect}");
        foreach (var p in _processableCnfs)
            _processableCnfsMap.Add(p.input, p);
    }

    public override bool receive(item item) {
        var ret = item.cnf.msk == ITEM_MSK.MEAT_PIE && base.receive(item);
        if (ret)
            _processableCnfsMap.TryGetValue(item.cnf, out _holdingCnf);
        return ret;
    }

    public override item remove(item i = null) {
        var ret = base.remove(i);
        if (ret) {
            _progressBar.display(false);
            _holdingCnf = null;
        }
        return ret;
    }

    private void Update() {
        if (!(_holdingCnf && holding())) {
            _glowShim.SetActive(false);
            _splash.SetActive(false);
            return;
        }

        _glowShim.SetActive(true);
        var progress = holding().updateProgress(Time.deltaTime);
        if (holding().cnf.stat == 0)
            _progressBar.progress(progress / _holdingCnf.progress);
        //if (_holding.itemCnf.stat == 1) //warning
        //    _progressBar.progress(progress / _holdingCnf.progress);
        _splash.SetActive(holding().cnf.stat == 1);
        if (progress < _holdingCnf.progress)
            return;

        if (holding().cnf.stat == 0)
            _progressBar.display(false);

        var nextRecv = Instantiate(_holdingCnf.output.prefab).GetComponent<item>();
        Destroy(remove()?.gameObject);
        receive(nextRecv);
    }
}
