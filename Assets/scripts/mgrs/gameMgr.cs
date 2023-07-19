using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Rendering.DebugUI;

public class gameMgr : netSingleton<gameMgr> {
    public enum SCENE {
        HELLO,
        MAIN,
        GAME,
        LOADING,
    }
    [SerializeField] Dictionary<ulong, bool> _players = new Dictionary<ulong, bool>();
    public enum STAGE { 
            INITIAL = 0,
            LOADING,
            LOBBY,
            WAITING,
            COUNT,
            STARTED,
            PAUSED,
            END,
    }
    [SerializeField] NetworkVariable<STAGE> _stage = new(STAGE.INITIAL);
    [SerializeField] NetworkVariable<int> _timerSec = new();
    [SerializeField] NetworkVariable<float> _timerPlay = new();
    public event Action<int> onTimerSecChg;
    public event Action<float> onTimePlayChg;
    public event Action<STAGE> onStageChg;
    [SerializeField] float[] _timerStamp = { 1f, 3.0f, 180f};
    [SerializeField] float _score = 0;


    public override void OnNetworkSpawn() {

        _stage.OnValueChanged += (STAGE p, STAGE n) => {
            onStageChg?.Invoke(n);
        };

        _stage.OnValueChanged += (STAGE p, STAGE n) => {
            if (n == STAGE.PAUSED)
                Time.timeScale = 0;
            else if (n == STAGE.STARTED)
                Time.timeScale = 1.0f;
        };

        _timerSec.OnValueChanged += (int p, int n) => onTimerSecChg?.Invoke(n);
        _timerPlay.OnValueChanged += (float p, float n) => {
            onTimePlayChg?.Invoke(n);
        };

       input.ins.onPause += cb => togglePauseServerRpc();
        
     
        if (IsServer)
            _stage.Value = STAGE.WAITING;
    }

    [ServerRpc(RequireOwnership = false)]
    public void togglePauseServerRpc() {
        if (stage.Value == STAGE.PAUSED || stage.Value == STAGE.STARTED) {
            stage.Value = stage.Value == STAGE.PAUSED ? STAGE.STARTED : STAGE.PAUSED;
        }
    }


    public NetworkVariable<STAGE> stage { get => _stage; private set => _stage = value;}

    public float score { get => _score; set => _score = value; }
    public float[] timerStamp { get => _timerStamp; set => _timerStamp = value; }

    public bool running() { return _stage.Value == STAGE.STARTED; }
    public  void Start() {
        stage = new(STAGE.LOBBY); // STAGE.INITIAL;
    }


    public static void resetEnv() {
        Time.timeScale = 1f;
        ins.stage = new(STAGE.INITIAL);
        counter.resetEvt();
        cuttingCounter.resetEvt();
        trashCan.resetEvt();
    }

    private void Update() {
        if (!IsServer)
            return;
        do {
            if (stage.Value == STAGE.INITIAL) {
                stage.Value = STAGE.LOADING;
                break;
            }

            if (stage.Value == STAGE.LOADING) {
                _timerPlay.Value = 0;
                _timerStamp[0] = 1;
                stage.Value = STAGE.COUNT;
                break;
            }
            if (stage.Value == STAGE.COUNT) {
                _timerStamp[0] -= Time.deltaTime;
                if (_timerStamp[0] <= .0f) {
                    _timerStamp[0] = 1;
                    _timerStamp[1] -= 1;
                    if (_timerStamp[1] <= -1) {
                        stage.Value = STAGE.STARTED;
                        break;
                    }
                    _timerSec.Value = (int)_timerStamp[1];
                }
                break;
            }

            if (stage.Value == STAGE.STARTED) {
                _timerPlay.Value += Time.deltaTime;
                if (_timerPlay.Value >= _timerStamp[2]) 
                    stage.Value = STAGE.END;
                break;
            }
        } while (false);
      
    }


    public void onInteractableChged(interactable p, interactable n) {
        Debug.Log($"highlight: {p?.transform.tag} -> {n?.transform.tag}");
        p?.highlight(false);
        n?.highlight(true);
    }

    public void onInteract(CallbackContext ctx) {
        if (ins.stage.Value == STAGE.WAITING) 
            setReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void setReadyServerRpc(ServerRpcParams p = default) {
        _players[p.Receive.SenderClientId] = true;
        setReadyClientRpc(NetworkManager.Singleton.ConnectedClients[p.Receive.SenderClientId].PlayerObject);
        var allReady = true;
        foreach (var cid in NetworkManager.Singleton.ConnectedClients) {
            var kv = _players.TryGetValue(cid.Key, out var ready);
            if (!kv || ready != true) {
                allReady = false;
                break;
            }
        }
        if (allReady)
            _stage.Value = STAGE.COUNT;



    }
    [ClientRpc]
    private void setReadyClientRpc(NetworkObjectReference nid) {
        nid.TryGet(out var client);
        client.GetComponent<player>().ready.gameObject.SetActive(true);
    }
}
