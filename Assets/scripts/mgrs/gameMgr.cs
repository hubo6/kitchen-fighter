using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMgr : MonoSingleton<gameMgr>
{
    // Start is called before the first frame update
    public void onInteractableChged(interactable p, interactable n)
    {
        Debug.Log($"highlight: {p?.transform.tag} -> {n?.transform.tag}");
        p?.highlight(false);
        n?.highlight(true);
    }
}
