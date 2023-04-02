using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter :  interactable
{
    // Start is called before the first frame update
    public override interactableType type() {
        return interactableType.COUNTER;
    }
}
