using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class heatPanCounter : counter {
    // Start is called before the first frame update
    [SerializeField] processableCnf[] _processableCnfs;
    [SerializeField] Dictionary<itemCnf, processableCnf> _processableCnfsMap = new Dictionary<itemCnf, processableCnf>();
    [SerializeField] progressBar _progressBar;
    [SerializeField] processableCnf _holdingCnf;
    [SerializeField] GameObject _glowShim;
    [SerializeField] GameObject _splash;
    [SerializeField] AudioSource _audio;
    //public static event Action<Transform> onSizzle;

    protected IEnumerator progress(processableCnf cnf) {
        yield return null;

    }

    public override void Start() {
        Assert.IsNotNull(_progressBar);
        Assert.IsNotNull(_audio);
        Assert.IsNull(Array.Find(_processableCnfs, i => {
            return i.input.msk != ITEM_MSK.MEAT_PIE && (i.input.type != ITEM_TYPE.RAW || i.input.type != ITEM_TYPE.PROCESSED);
        }));
        _progressBar.gameObject.SetActive(false);
        foreach (var p in _processableCnfs) _processableCnfsMap.Add(p.input, p);
    }

    public override bool receive(item item) {
        var ret = item.cnf.msk == ITEM_MSK.MEAT_PIE && base.receive(item);
        if (ret) 
            _processableCnfsMap.TryGetValue(item.cnf, out _holdingCnf);
        if(item.cnf.msk == ITEM_MSK.MEAT_PIE && item.cnf.type == ITEM_TYPE.RAW)
            _audio.Play();
        else
            _audio.Stop();
        return ret;
    }

    public override item remove(item i = null) {
        var ret = base.remove(i);
        if (ret) {
            _progressBar.display(false);
            _holdingCnf = null;
            _audio.Stop();
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
        //onSizzle?.Invoke(transform);
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
