using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class waitingListUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform _recipeTemplate;
    [SerializeField] Transform _iconTemplate;
    [SerializeField] Transform _list;

    class transformBarPair {
        public Transform obj;
        public Image bar;
    }
    //Dictionary<dishSchemaCounter, transformBarPair> _cache = new Dictionary<dishSchemaCounter, transformBarPair>();
    List<transformBarPair> _cache = new();

    private void Start() {
        //gameObject.SetActive(false);
        Assert.IsNotNull(_recipeTemplate);
        Assert.IsNotNull(_iconTemplate);
        Assert.IsNotNull(_list);
        deliveryMgr.ins.onAdd += onAdd;
        deliveryMgr.ins.onRm += onRm;
        deliveryMgr.ins.onUpdate += onUpdate;
        gameMgr.ins.onStageChg +=s => {
            if(s == gameMgr.STAGE.STARTED)
                gameObject.SetActive(true);
        };

    }
    public void onAdd(dishSchemaCounter schema) {
        var item = Instantiate(_recipeTemplate, _list);
        item.gameObject.SetActive(true);
        var icons = item.Find("icons");
        var name = item.GetComponentInChildren<TextMeshProUGUI>();
        var progress = item.Find("name").Find("process").Find("bar").GetComponent<Image>();
        name.SetText(schema.schemaRef.dishName);
        foreach (var s in schema.schemaRef.dishOrder) {
            var icon = Instantiate(_iconTemplate, icons);
            icon.gameObject.SetActive(true);
            icon.GetComponent<Image>().sprite = s.icon;
        }
        _cache.Add(new transformBarPair() { obj = item, bar = progress });
    }

    public void onRm(int idx) {
         var toRm = _cache.ElementAt(idx);
        _cache.RemoveAt(idx);
        Destroy(toRm.obj.gameObject);
    }

    public void onUpdate(List<dishSchemaCounter> toUpdate) {
        //foreach (var i in toUpdate) {
        //    if (!_cache.TryGetValue(i, out transformBarPair child)) continue;
        //    child.bar.fillAmount = i.timepass / i.schemaRef.waitingTime;
        //}
    }
}
