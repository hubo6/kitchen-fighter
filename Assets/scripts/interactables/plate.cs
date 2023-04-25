using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class plate : item, owner
{
    [SerializeField] receipt[]  _cnfArray;
    [SerializeField] Dictionary<RECEIPT_MSK, int> _cnf = new Dictionary<RECEIPT_MSK, int>();
    [SerializeField] Dictionary<RECEIPT_MSK, item> _contained = new Dictionary<RECEIPT_MSK, item>();
    [SerializeField] ulong _msk = 0;
    public item holding()
    {
        return this;
    }

    public bool receive(item i)
    {
        throw new System.NotImplementedException();
    }

    public item remove(item i = null)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var i in _cnfArray)
            _cnf.Add(i.msk, 1);
    }

    public bool clear(out List<item> cleared) {
        cleared = new List<item>();
        _msk = 0;
        if (_contained.Count == 0)
            return false;
        foreach (var i in _contained)
            cleared.Add(i.Value);
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
