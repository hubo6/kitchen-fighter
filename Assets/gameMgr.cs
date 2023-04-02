using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMgr : MonoSingleton<gameMgr>
{
    // Start is called before the first frame update
    public void onInteractableChged(object sender, evtInteractableChged evt)
    {
        evt.p?.highlight(false);
        evt.n?.highlight(true);
    }
}
