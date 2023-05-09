using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class plate : item, owner {
    [SerializeField] itemCnf[] _cnfArray;
    //[SerializeField] LinkedList<ITEM_MSK, float> _cnf = new Dictionary<ITEM_MSK, float>();
    [SerializeField] Dictionary<ITEM_MSK, LinkedList<item>> _contained = new Dictionary<ITEM_MSK, LinkedList<item>>();
    [SerializeField] int _msk = 0;
    [SerializeField] static float _layoutOffset = 0.1f;
    [SerializeField] float _curLayoutOffset = _layoutOffset;

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
        if (_contained.TryGetValue(i.cnf.msk, out LinkedList<item> list))
            list.AddLast(i);
        else {
            var newList = new LinkedList<item>();
            newList.AddLast(i);
            _contained[i.cnf.msk] = newList;
        }
        var dish = gameMgr.ins.plateReArrange(this);
        Debug.LogFormat($"{gameObject.tag} redish:{dish}");
        return true;
    }

    public item remove(item i = null) {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start() {
        //foreach (var i in _cnfArray)
        //    _cnf.Add(i.msk, 1);
    }

    public void clear(out List<item> cleared) {
        cleared = new List<item>(); //cache list todo
        _msk = 0;
        _curLayoutOffset = _layoutOffset;
        foreach (var list in _contained)
            foreach (var i in list.Value)
                cleared.Add(i);
        _contained.Clear();
    }

    public void rearrange(DishSchema sch) {
        _curLayoutOffset = _layoutOffset;
        foreach (var i in sch.dishOrder) {
            if (!_contained.TryGetValue(i.msk, out LinkedList<item> itemList))
                continue;
            foreach (var item in itemList) {
                item.transform.localPosition = Vector3.up * _curLayoutOffset;
                _curLayoutOffset += item.cnf.height;
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
