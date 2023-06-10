using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class waitingList : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform _recipeTemplate;
    [SerializeField] Transform _iconTemplate;
    [SerializeField] Transform _list;

    class transformBarPair {
        public Transform obj;
        public Image bar;
    }
    Dictionary<dishSchemaCounter, transformBarPair> _cache = new Dictionary<dishSchemaCounter, transformBarPair>();

    private void Start() {
        Assert.IsNotNull(_recipeTemplate);
        Assert.IsNotNull(_iconTemplate);
        Assert.IsNotNull(_list);
        deliveryMgr.ins.onAdd += onAdd;
        deliveryMgr.ins.onRm += onRm;
        deliveryMgr.ins.onUpdate += onUpdate;

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
        _cache.Add(schema, new transformBarPair() { obj = item, bar = progress });
    }

    public void onRm(List<dishSchemaCounter> toRm) {
        foreach (var i in toRm) {
            if (!_cache.TryGetValue(i, out transformBarPair child)) continue;
            _cache.Remove(i);
            Destroy(child.obj.gameObject);
        }
    }

    public void onUpdate(List<dishSchemaCounter> toUpdate) {
        foreach (var i in toUpdate) {
            if (!_cache.TryGetValue(i, out transformBarPair child)) continue;
            child.bar.fillAmount = i.timepass / i.schemaRef.waitingTime;
        }
    }
}
