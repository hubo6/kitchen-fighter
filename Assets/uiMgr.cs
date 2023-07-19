using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using static gameMgr;

public class uiMgr : MonoBehaviour {

    public enum UI_COMPONENT {
        LOG_VIEW = 0,
        WAITING_LIST,
        TIMER,
        CLOCK,
        NET,
        GAME_OVER,
        PAUSE,
        SETTING,
    }


    [SerializeField] timer _timer;
    [SerializeField] gameOverUi _gameOver;
    [SerializeField] pauseUi _pause;
    [SerializeField] settingUi _setting;
    [SerializeField] netCnf _netCnf;

    [SerializeField] logView _logView;
    [SerializeField] waitingListUI _waitingList;
    [SerializeField] clock _clock;



    [SerializeField] Dictionary<UI_COMPONENT, GameObject> _components = new();
    // Start is called before the first frame update
    void Start() {
        Assert.IsNotNull(_timer);
        Assert.IsNotNull(_clock);
        Assert.IsNotNull(_gameOver);
        Assert.IsNotNull(_pause);
        Assert.IsNotNull(_logView);
        Assert.IsNotNull(_waitingList);
        Assert.IsNotNull(_setting);
        Assert.IsNotNull(_netCnf);
        Assert.IsNotNull(_clock);

        _components.Add(UI_COMPONENT.LOG_VIEW, _logView.gameObject);
        _components.Add(UI_COMPONENT.WAITING_LIST, _waitingList.gameObject);
        _components.Add(UI_COMPONENT.TIMER, _timer.gameObject);
        _components.Add(UI_COMPONENT.CLOCK, _clock.gameObject);
        _components.Add(UI_COMPONENT.NET, _netCnf.gameObject);
        _components.Add(UI_COMPONENT.GAME_OVER, _gameOver.gameObject);
        _components.Add(UI_COMPONENT.PAUSE, _pause.gameObject);
        _components.Add(UI_COMPONENT.SETTING, _setting.gameObject);
        hide();
        show(UI_COMPONENT.NET);
        gameMgr.ins.onStageChg += (STAGE s) => {
            if (s == STAGE.LOBBY) {
                hide();
                show(UI_COMPONENT.NET);
                return;
            }

            if (s == STAGE.WAITING) {
                hide();
                return;
            }

            if (s == STAGE.COUNT) {
                hide();
                show(UI_COMPONENT.TIMER);
                return;
            }

            if (s == STAGE.STARTED) {
                hide();
                show(UI_COMPONENT.CLOCK);
                show(UI_COMPONENT.WAITING_LIST);
                return;
            }

            if (s == STAGE.PAUSED) {
                hide();
                show(UI_COMPONENT.PAUSE);
                return;
            }

            if (s == STAGE.END) {
                return;
            }
        };
    }

    void hide() {
        foreach (var obj in _components)
            if (obj.Value.activeSelf)
                obj.Value.SetActive(false);
    }

    void show(UI_COMPONENT ui) {
        if (!_components.TryGetValue(ui, out var com))
            return;
        if (!com.activeSelf)
            com.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

    }
}
