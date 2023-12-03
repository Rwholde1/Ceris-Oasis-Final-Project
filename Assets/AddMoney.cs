using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddMoney : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI MoneyCount;

    private void Start()
    {
        //Canvas canvas = GameObject.Find("GUI").GetComponent<Canvas>();
        //TextMeshProUGUI[] textMeshProComponents = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        //MoneyCount = canvas.transform.Find("Money")?.GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        MoneyCount.text = ("$"+ money);
        if (Input.GetKeyDown(KeyCode.P))
        {
            money += 50;
        }
    }
}
