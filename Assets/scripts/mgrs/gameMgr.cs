using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameMgr : MonoSingleton<gameMgr> {


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
    [SerializeField] float[] _timerStamp = { 0f, 3.0f, 180f};


    public STAGE stage { get => _stage; }

    public bool running() { return _stage == STAGE.STARTED; }
    private void Start() {
        _stage = STAGE.INITIAL;
        _timerStamp[0] = _timerStamp[1];
    }

    private void Update() {
        do {
            if (_stage == STAGE.INITIAL) {
                _stage = STAGE.LOADING;
                break;
            }

            if (_stage == STAGE.LOADING) {
                _stage = STAGE.COUNT;
                break;
            }
            if (_stage == STAGE.COUNT) {
                var curInt = (int)(_timerStamp[0] + 1);
                _timerStamp[0] -= Time.deltaTime;
                var nextInt = (int)(_timerStamp[0] + 1);
                if (curInt - nextInt == 1) 
                    onTimerSecChg?.Invoke(curInt - 1);
                if (curInt == 0) {
                    _stage = STAGE.STARTED;
                    _timerStamp[0] = 0;
                }
                break;
            }

            if (_stage == STAGE.STARTED) {
                _timerStamp[0] += Time.deltaTime;
                if (_timerStamp[0] >= _timerStamp[2]) {
                    _stage = STAGE.END;
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
