using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface owner 
{
    bool receive(item i);
    item remove(item i = null);
    item holding();
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
