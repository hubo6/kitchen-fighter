using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class plate : item, owner
{
    [SerializeField] receipt[]  _cnfArray;
    //[SerializeField] LinkedList<RECEIPT_MSK, float> _cnf = new Dictionary<RECEIPT_MSK, float>();
    [SerializeField] Dictionary<RECEIPT_MSK, LinkedList<item>> _contained = new Dictionary<RECEIPT_MSK, LinkedList<item>>();
    [SerializeField] ulong _msk = 0;
    [SerializeField] float _layoutOffset = 0.5f;
    public item holding()
    {
        return this;
    }

    public  bool receive(item i)
    {
        if (i.receipt.type != ITEM_TYPE.PROCESSED)
            return false;
        _msk |= (ulong)i.receipt.msk;
        var dish = gameMgr.ins.plateRedish(this);
        Debug.LogFormat($"{gameObject.tag} redish:{dish}");
        i.transform.parent = transform;
        i.transform.localPosition = Vector3.up * _layoutOffset;
        _layoutOffset += i.receipt.height;
        if (_contained.TryGetValue(i.receipt.msk, out LinkedList<item> list))
            list.AddLast(i);
        else {
            var newList = new LinkedList<item>();
            newList.AddLast(i);
            _contained[i.receipt.msk] = newList;
        }
        return true;
    }

    public item remove(item i = null)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        //foreach (var i in _cnfArray)
        //    _cnf.Add(i.msk, 1);
    }

    public void clear(out List<item> cleared) {
        cleared = new List<item>(); //cache list todo
        _msk = 0;
        _layoutOffset = 0;
        foreach (var list in _contained)
            foreach(var i in list.Value)
                cleared.Add(i);
        _contained.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
