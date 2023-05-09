using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INTERACT_TYPE {
    NONE = 0,
    COUNTER,
    MATERIAL,
    MAX_SIZE
}

public class interactable : MonoBehaviour {
    [SerializeField] GameObject _highlight;
    // Start is called before the first frame update
    public virtual void highlight(bool select = true) {
        _highlight?.SetActive(select);
    }
    public virtual INTERACT_TYPE type() { return INTERACT_TYPE.NONE; }
    public virtual bool interact(owner src) {
        Debug.Log($"interact: {transform.tag}");
        return false;
    }

    public virtual void process() {
        Debug.Log($"process: {transform.tag}");
        return;
    }

    //GameObject gameObj();
}
