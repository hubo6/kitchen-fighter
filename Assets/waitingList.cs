using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class waitingList : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform _recipeTemplate;
    [SerializeField] Transform _iconTemplate;
    [SerializeField] Transform _list;

    private void Start() {
        Assert.IsNotNull(_recipeTemplate);
        Assert.IsNotNull(_iconTemplate);
        Assert.IsNotNull(_list);
        waitingListMgr.ins.onAdd += onAdd;
        waitingListMgr.ins.onRm += onRm;
        waitingListMgr.ins.onUpdate += onUpdate;

    }
    public void onAdd(dishSchemaCounter schema) {
       
        var item = Instantiate(_recipeTemplate, _list);
        item.gameObject.SetActive(true);
        var icons = item.Find("body").Find("icons");
        var name = item.GetComponentInChildren<TextMeshProUGUI>();
        var progress = item.Find("process");
        name.SetText(schema.schemaRef.dishName);


        foreach (var s in schema.schemaRef.dishOrder) {
            var icon = Instantiate(_iconTemplate, icons);
            icon.gameObject.SetActive(true);
            icon.GetComponent<Image>().sprite = s.icon;

        }





    }

    public void onRm(List<dishSchemaCounter> toRm) { 
    }

    public void onUpdate(Dictionary<dishSchemaCounter, float> toUpdate) {
    }
}
