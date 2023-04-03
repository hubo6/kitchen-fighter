using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum interactableType { 
    NONE = 0,
    COUNTER,
    MAX_SIZE
}

public class interactable : MonoBehaviour {
    [SerializeField] GameObject _highlight;
    // Start is called before the first frame update
    public virtual void highlight(bool select = true) {
        _highlight?.SetActive(select);
    }
    public virtual interactableType type() { return interactableType.NONE; }
    public virtual bool interact(owner src) {
        Debug.Log($"interact: {transform.tag}");
        return false;
    }

    //GameObject gameObj();
}
