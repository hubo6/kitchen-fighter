using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class popup : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] Transform _gameOver;
    [SerializeField] TextMeshProUGUI _score;

    [SerializeField] Transform _options;
    [SerializeField] Button _btnOpt;
    [SerializeField] Button _btnResume;
    [SerializeField] Button _btnQuit;

    [SerializeField] Transform _settings;
    [SerializeField] Button _btnMusicVol;
    [SerializeField] Button _btnSoundVol;
    [SerializeField] Button _btnSettingsBack;
    [SerializeField] Transform _btnKeyBind;
    [SerializeField] Transform _bindHint;


    void Start() {
        Assert.IsNotNull(_gameOver);
        Assert.IsNotNull(_options);
        Assert.IsNotNull(_score);
        Assert.IsNotNull(_btnOpt);
        Assert.IsNotNull(_btnResume);
        Assert.IsNotNull(_btnQuit);

        Assert.IsNotNull(_settings);
        Assert.IsNotNull(_btnMusicVol);
        Assert.IsNotNull(_btnSoundVol);
        Assert.IsNotNull(_btnSettingsBack);

        Assert.IsNotNull(_btnKeyBind);
        Assert.IsNotNull(_bindHint);

        //_btnResume.onClick.AddListener(() => gameMgr.ins.togglePauseServerRpc());
        //_btnQuit.onClick.AddListener(() => SceneManager.LoadScene(gameMgr.SCENE.MAIN.ToString()));
        //_btnOpt.onClick.AddListener(() => {
        //    hide();
        //    gameObject.SetActive(true);
        //    _settings.gameObject.SetActive(true);
        //    _btnMusicVol.Select();
        //});
        //gameMgr.ins.onStageChg += s => {
        //    hide();
        //    if (s == gameMgr.STAGE.END) {
        //        gameObject.SetActive(true);
        //        _gameOver.gameObject.SetActive(true);

        //    } else if (s == gameMgr.STAGE.PAUSED) {
        //        gameObject.SetActive(true);
        //        _options.gameObject.SetActive(true);
        //        _btnResume.Select();
        //    }
        //};
        _btnMusicVol.GetComponentInChildren<TextMeshProUGUI>().text = $"Music Vol:{soundMgr.ins.music_vol:F1}";
        _btnMusicVol.onClick.AddListener(() => {
            soundMgr.ins.music_vol = (soundMgr.ins.music_vol + 0.1f) % 1f;
            _btnMusicVol.GetComponentInChildren<TextMeshProUGUI>().text = $"Music Vol:{soundMgr.ins.music_vol:F1}";
        });
        _btnSoundVol.GetComponentInChildren<TextMeshProUGUI>().text = $"Sound Vol:{soundMgr.ins.effect_vol:F1}";
        _btnSoundVol.onClick.AddListener(() => {
            soundMgr.ins.effect_vol = (soundMgr.ins.effect_vol + 0.1f) % 1f;
            _btnSoundVol.GetComponentInChildren<TextMeshProUGUI>().text = $"Sound Vol:{soundMgr.ins.effect_vol:F1}";
        });

        _btnSettingsBack.onClick.AddListener(() => {
            hide();
            gameObject.SetActive(true);
            _options.gameObject.SetActive(true);
            _btnResume.Select();
        });

        var keysContainer = _settings.Find("options").Find("container_bind");
        foreach (var key in input.ins.inputMap) {
            var item = Instantiate(_btnKeyBind, keysContainer);
            item.gameObject.SetActive(true);
            item.Find("key").GetComponent<TextMeshProUGUI>().text = key.key;
            var btn = item.Find("btn");
            var btnTxt = btn.Find("bind_key").GetComponent<TextMeshProUGUI>();
            var word = input.ins.getWord(key.key);
            btnTxt.text = word;
            btn.GetComponent<Button>().onClick.AddListener(() => {
                _bindHint.gameObject.SetActive(true);
                _bindHint.Find("txt").GetComponent<TextMeshProUGUI>().text = $"Press Any Key  to Rebind{key} (Current {word} )";
                input.ins.rebind(key.key, () => {
                    var wordNew = input.ins.getWord(key.key);
                    btnTxt.text = wordNew;
                    _bindHint.gameObject.SetActive(false);
                 });
            });
        }
        _btnKeyBind.gameObject.SetActive(false);
        var containerForm = keysContainer.GetComponent<RectTransform>();
        containerForm.sizeDelta =
            new Vector2(containerForm.sizeDelta.x, containerForm.sizeDelta.y * input.ins.inputMap.Count);
        hide();
    }

    void hide() {
        if (gameObject.activeSelf) gameObject.SetActive(false);
        if (_options.gameObject.activeSelf) _options.gameObject.SetActive(false);
        if (_settings.gameObject.activeSelf) _settings.gameObject.SetActive(false);
        if (_gameOver.gameObject.activeSelf) _gameOver.gameObject.SetActive(false);
        if (_bindHint.gameObject.activeSelf) _bindHint.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }
}
