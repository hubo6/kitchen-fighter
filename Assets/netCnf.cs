using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class netCnf : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button _btn_host;
    [SerializeField] Button _btn_client;
    void Start()
    {
        Assert.IsNotNull(_btn_host);
        Assert.IsNotNull(_btn_client);

        _btn_host.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        _btn_client.onClick.AddListener(() => NetworkManager.Singleton.StartClient());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
