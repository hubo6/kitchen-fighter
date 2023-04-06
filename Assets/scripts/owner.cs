using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface owner 
{
    // Start is called before the first frame update
    //public interactable _interactable;
    bool receive(item i);
    item remove(item i = null);
    item holding(item i = null);

}



//public class interactionMgr : MonoSingleton<interactionMgr> {

//    public bool owner(owner src, owner dst)
//    {
//        var ret = false;
//        do
//        {
//            if (dst == null || src == null) break;
//            if (src.owner() != null)
//            {
//               var rm =  src.remove(src);
//                if (!rm) break;
//            }

           

//        } while (false);
//        return ret
//    }
//}
