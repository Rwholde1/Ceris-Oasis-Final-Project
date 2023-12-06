using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyMoneyFromGUI : MonoBehaviour
{
    public TextMeshProUGUI MoneyCount;
    [SerializeField] private TextMeshProUGUI self;

    private void Start()
    {
        //Canvas canvas = GameObject.Find("GUI").GetComponent<Canvas>();
        //TextMeshProUGUI[] textMeshProComponents = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        //MoneyCount = canvas.transform.Find("Money")?.GetComponent<TextMeshProUGUI>();
        MoneyCount = LobbySceneManagement.singleton.playerCamObject.GetComponent<AddMoney>().MoneyCount;
    }
    // Update is called once per frame
    void Update()
    {
        if (MoneyCount == null) {
            MoneyCount = LobbySceneManagement.singleton.playerCamObject.GetComponent<AddMoney>().MoneyCount;
        }
        self.text = MoneyCount.text;
    }
}
