using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class staticResetMgr
{
    // Start is called before the first frame update
    public static void resetEnv() {
        Time.timeScale = 1f;
        counter.resetEvt();
        cuttingCounter.resetEvt();
        trashCan.resetEvt();
    }
}
