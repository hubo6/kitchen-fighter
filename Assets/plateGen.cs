using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class plateGen : item {
    // Start is called before the first frame update
    [SerializeField] int _max = 5;
    [SerializeField] LinkedList<item> _cur = new LinkedList<item>();
    [SerializeField] float _genTime = 5f;
    [SerializeField] float _stamp = 0f;
    [SerializeField] Transform _prefab;
    [SerializeField] MeshRenderer _plateRdr;

    public LinkedList<item> cur { get => _cur; private set => _cur = value; }

    void Start()
    {
        _plateRdr = _prefab.GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_max == _cur.Count) return;
        _stamp += Time.deltaTime;
        if (_stamp < _genTime) return;

        _stamp = 0;
        var plate = Instantiate(_prefab, transform);
        plate.localPosition = new Vector3(0, _plateRdr.bounds.size.y, 0) * _cur.Count;
        _cur.AddLast(plate.GetComponent<item>());
    }

    public bool add(item plate) {
        if (plate == null)
            return false;
        if (cur.Count == _max)
            return false;
        plate.transform.parent = transform;
        plate.transform.localPosition = new Vector3(0, _plateRdr.bounds.size.y, 0) * _cur.Count;
        _cur.AddLast(plate);
        return true;
    }
    public item remove(int count = 1) {
        if (_cur.Count == 0)
            return null;

        var ret = _cur.Last().GetComponent<item>();
        _cur.RemoveLast();
        return ret;
    }

}
