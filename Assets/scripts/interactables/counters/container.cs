using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class container : counter {
    [SerializeField] itemCnf _itemCnf;
    [SerializeField] protected counterAnim _counterAnim;
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        if (_itemCnf == null)
            throw new System.Exception($"counter itemCnf is null");
        _counterAnim = GetComponentInChildren<counterAnim>();
        if (_counterAnim == null)
            throw new System.Exception($"counter itemCnf is null");
        _counterAnim.OnAnimEvt += () => {
            receive(Instantiate(_itemCnf.prefab).GetComponent<item>());
        };
    }

    public override bool interact(owner src) {
        var ret = false;
        do {
            if (base.interact(src))
                break;
            if (holding() != null)
                break;
            if (_counterAnim.playing()) {
                Debug.LogWarning($"{transform.name} is busy of interaction.");
                break;
            }
            _counterAnim.play();
            ret = true;

        } while (false);
        return ret;
    }

    public void onClosed() {
    }
}
