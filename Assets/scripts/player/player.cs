using System;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using System.Collections.Generic;

public class player : NetworkBehaviour, owner {
    // Start is called before the first frame update

    [SerializeField] float _xInput;
    [SerializeField] float _zInput;
    [SerializeField] Vector3 _inputV3 = Vector3.zero;
    [SerializeField] float _spd = 6f;
    [SerializeField] float _rotSpd = 6f;
    [SerializeField] float _interactDis = 1f;
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] interactable _interactable;
    [SerializeField] Transform _objAnchor;
    [SerializeField] item _holding;
    [SerializeField] static float[] _spwanArea = { 0,0,0,8,0}; //xyzwh
    [SerializeField] Material[] _meterials;
    [SerializeField] Transform _ready;





    public event Action<interactable, interactable> onInteractableChged;

    public item holding() {
        return _holding;
    }
    public Vector3 inputV3 { get => _inputV3; private set => _inputV3 = value; }
    public float spd { get => _spd; private set => _spd = value; }
    public float rotSpd { get => _rotSpd; set => _rotSpd = value; }
    public Transform ready { get => _ready;}

    public void setInteractable(interactable obj = null) {
        var pre = _interactable;
        _interactable = obj;
        if (pre != _interactable)
            onInteractableChged?.Invoke(pre, _interactable);
    }

    public override void OnNetworkSpawn() {
        Debug.Log($"player nid {this.NetworkObjectId}");

        if (IsServer) {
            var cnt = NetworkManager.Singleton.ConnectedClients.Count;
            var gap = _spwanArea[3] / (cnt + 1);
            var xStart = _spwanArea[0] - (cnt + 1) / 2f * gap;
            transform.position = new Vector3(xStart + gap * cnt, _spwanArea[1], _spwanArea[2]);
            NetworkManager.Singleton.OnClientDisconnectCallback += onClientDisconn;
        }

        if (IsOwner) {
            input.ins.onInteract += ctx => {
                do {
                    if (!gameMgr.ins.running())
                        break;
                    if (_interactable == null)
                        break;
                    interactServerRpc(netRef(), _interactable.NetworkObject);
                } while (false);
            };
           input.ins.onInteract += gameMgr.ins.onInteract;
           input.ins.onProcess += ctx => {
               do {
                   if (!gameMgr.ins.running())
                       break;
                   if (_interactable == null)
                       break;
                   processServerRpc(netRef(), _interactable.NetworkObject);
               } while (false);
            };
            onInteractableChged += gameMgr.ins.onInteractableChged;
        }
    }

    private void onClientDisconn(ulong cid) {
        Debug.Log($"{cid}");
        if (cid == OwnerClientId) {
            holding().GetComponent<NetworkObject>().Despawn();
        }
    }

    void updateInteractable() {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactDis, _interactableLayer)) {
            setInteractable(null);
            return;
        }
        if (!hit.transform.TryGetComponent(out interactable obj))
            return;
        setInteractable(obj);
    }

    void Start() {
        Assert.IsNotNull(_objAnchor);
        Assert.IsNotNull(_ready);
        _ready.gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    void interactServerRpc(NetworkObjectReference from, NetworkObjectReference to) {
        interactClientRpc(from, to);
    }
    [ClientRpc]
    void interactClientRpc(NetworkObjectReference from, NetworkObjectReference to) {
        from.TryGet(out var fromNetObj);
        to.TryGet(out var toNetObj);
        Assert.IsNotNull(fromNetObj); Assert.IsNotNull(toNetObj);
        toNetObj.GetComponent<interactable>()?.interact(fromNetObj.GetComponent<owner>());
    }

    [ServerRpc(RequireOwnership = false)]
    void processServerRpc(NetworkObjectReference from, NetworkObjectReference to) {
        processClientRpc(from, to);
    }
    [ClientRpc]
    void processClientRpc(NetworkObjectReference from, NetworkObjectReference to) {
        from.TryGet(out var fromNetObj);
        to.TryGet(out var toNetObj);
        Assert.IsNotNull(fromNetObj); Assert.IsNotNull(toNetObj);
        toNetObj.GetComponent<interactable>()?.process();
    }


    // Update is called once per frame
    void Update() {
        if (!IsOwner)
            return;
        input.ins.updateInput(ref _inputV3);
        updateInteractable();
    }

    public bool receive(item i) {
        var ret = false;
        do {
            if (_holding == null) {
                i.setNetTransformParentAgent(_objAnchor, Vector3.zero);
                _holding = i;
                //var itemTrans = i.transform;
                //itemTrans.SetParent(_objAnchor);
                //itemTrans.localPosition = Vector3.zero;
              
                Debug.Log($"received {_holding.name} {transform.name}.");
                ret = true;
                break;
            }
            if (_holding.cnf.type == ITEM_TYPE.PLATE) { //_holding combination validation here
                ret = (_holding as plate).receive(i);
                break;
            }

            if (_holding.cnf.type == ITEM_TYPE.PROCESSED && i.cnf.type == ITEM_TYPE.PLATE) {
                (i as plate).receive(_holding);
                i.setNetTransformParentAgent(_objAnchor, Vector3.zero);
                //i.transform.SetParent(_objAnchor);
                //i.transform.localPosition = Vector3.zero;
                _holding = i;
                ret = true;
                break;
            }
            Debug.LogWarning($"receive {_holding.name} failed exists in {transform.name}.");
        } while (false);
        return ret;
    }

    public item remove(item i = null) {
        item ret = null;
        do {
            if (i == null) {
                if (_holding != null) {
                    ret = _holding;
                    _holding = null;
                }
                break;
            }
            if (_holding != i)
                break;
            ret = _holding;
            _holding = null;
        } while (false);
        Debug.Log($"removed {ret?.name} from {transform.name} ret: {ret != null}.");
        return ret;
    }

    public NetworkObject netRef() { return NetworkObject; }
}

