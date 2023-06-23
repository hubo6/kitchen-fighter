using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class startTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI _txt;
    [SerializeField] Animator _animator;
    void Start() {
        Assert.IsNotNull(_txt);
        Assert.IsNotNull(_animator);
        gameObject.SetActive(false);
      
        gameMgr.ins.onTimerSecChg += onTimerSecChg;
        gameMgr.ins.onStageChg +=s => {
            if (s == gameMgr.STAGE.COUNT)
                gameObject.SetActive(true);
            else if (s == gameMgr.STAGE.STARTED)
                gameObject.SetActive(false);
        };
    }

    // Update is called once per frame
    void onTimerSecChg(int num)
    {
        _txt.SetText($"{num}");
        _animator.SetTrigger("pop");
    }
}
