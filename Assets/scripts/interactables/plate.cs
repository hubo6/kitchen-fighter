using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class plate : item, owner {
    [SerializeField] OrderedDictionary _contained = new OrderedDictionary();
    [SerializeField] int _msk = 0;
    [SerializeField] static float _layoutOffset = 0.1f;
    [SerializeField] float _curLayoutOffset = _layoutOffset;

    [SerializeField] Transform _template;
    [SerializeField] GameObject _icons_ui;

    public int msk { get => _msk; }

    public item holding() {
        return this;
    }

    public bool receive(item i) {
        if (i.cnf.type != ITEM_TYPE.PROCESSED)
            return false;
        _msk |= 1 << (int)i.cnf.msk;
        i.transform.parent = transform;
        i.transform.localPosition = Vector3.up * _curLayoutOffset;
        _curLayoutOffset += i.cnf.height;
        if (!_contained.Contains(i.cnf.msk))
            _contained[i.cnf.msk] = new List<item>();
        (_contained[i.cnf.msk] as List<item>).Add(i);
        var dish = deliveryMgr.ins.plateReArrange(this);
        var icon = Instantiate(_template, _icons_ui.transform);
        icon.gameObject.SetActive(true);
        icon.Find("pic").GetComponent<Image>().sprite = i.cnf.icon;
        if (!_icons_ui.activeSelf)
            _icons_ui.SetActive(true);
        Debug.LogFormat($"{gameObject.tag} redish:{dish}");
        return true;
    }

    public item remove(item i = null) {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start() {
        Assert.IsNotNull(_template);
        Assert.IsNotNull(_icons_ui);
        _icons_ui.SetActive(false);
    }

    public int clear(List<item> cleared) {
        var ret = _msk;
        _curLayoutOffset = _layoutOffset;
        if (cleared != null) {
            cleared.Clear();
            foreach (DictionaryEntry sub in _contained)
                cleared.AddRange(sub.Value as List<item>);
        }
        _contained.Clear();
        _msk = 0;
        foreach (Transform child in _icons_ui.transform)
            if (child.gameObject.activeSelf) Destroy(child.gameObject); //template is here
        _icons_ui.SetActive(false);
        return ret;
    }

    public void rearrange(dishSchema sch) {
        _curLayoutOffset = _layoutOffset;
        foreach (var i in sch.dishOrder) {
            if (!_contained.Contains(i.msk))
                continue;
            //todo the dish re-arrangement
            foreach (var item in (_contained[i.msk] as List<item>)) {
                item.transform.localPosition = Vector3.up * _curLayoutOffset;
                _curLayoutOffset += item.cnf.height;
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public NetworkObject netRef() { return NetworkObject; }
}
