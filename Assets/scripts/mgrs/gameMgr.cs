using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameMgr : MonoSingleton<gameMgr> {


    public enum SCENE { 
        HELLO,
        MAIN,
        GAME,
        LOADING,
    }


    public enum STAGE { 
            INITIAL = 0,
            LOADING,
            COUNT,
            STARTED,
            PAUSED,
            END,
    }
    [SerializeField] STAGE _stage = STAGE.INITIAL;
    public event Action<int> onTimerSecChg;
    public event Action<STAGE> onStageChg;
    [SerializeField] float[] _timerStamp = { 0f, 3.0f, 180f};
    [SerializeField] float _score = 0;


    public STAGE stage { get => _stage; private set {
            var chg = value != _stage; 
            _stage = value;
            if (chg) onStageChg?.Invoke(_stage);
        } }

    public float score { get => _score; set => _score = value; }
    public float[] timerStamp { get => _timerStamp; set => _timerStamp = value; }

    public bool running() { return _stage == STAGE.STARTED; }
    private void Start() {
        stage = STAGE.INITIAL;
        _timerStamp[0] = _timerStamp[1];
    }

    private void Update() {
        do {
            if (stage == STAGE.INITIAL) {
                stage = STAGE.LOADING;
                break;
            }

            if (stage == STAGE.LOADING) {
                stage = STAGE.COUNT;
                break;
            }
            if (stage == STAGE.COUNT) {
                var curInt = (int)(_timerStamp[0] + 1);
                _timerStamp[0] -= Time.deltaTime;
                var nextInt = (int)(_timerStamp[0] + 1);
                if (curInt - nextInt == 1) 
                    onTimerSecChg?.Invoke(curInt - 1);
                if (curInt == 0) {
                    stage = STAGE.STARTED;
                    _timerStamp[0] = 0;
                }
                break;
            }

            if (stage == STAGE.STARTED) {
                _timerStamp[0] += Time.deltaTime;
                if (_timerStamp[0] >= _timerStamp[2]) {
                    stage = STAGE.END;
                    _timerStamp[0] = _timerStamp[1];
                }
                break;
            }
        } while (false);
      
    }


    public void onInteractableChged(interactable p, interactable n) {
        Debug.Log($"highlight: {p?.transform.tag} -> {n?.transform.tag}");
        p?.highlight(false);
        n?.highlight(true);
    }
}
