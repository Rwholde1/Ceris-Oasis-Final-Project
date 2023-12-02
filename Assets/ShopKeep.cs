using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeep : MonoBehaviour
{
    private bool isShopOpen = false;
    private GameObject player;
    public GameObject shop;
    public GameObject GUI;
    private CanvasGroup cg;
    private int lastcheckedmoney = 0;
    public int[] costofeachgun;
    private void Start()
    {
        cg = GUI.GetComponent<CanvasGroup>();
        
    }
    public void ToggleShop()
    {
        // Toggle the shop state
        isShopOpen = !isShopOpen;

        // Activate/Deactivate the shop UI
        if (isShopOpen)
        {
            OpenShop();
        }
        else
        {
            CloseShop();
        }
    }
    private void FixedUpdate()
    {
        if (isShopOpen && (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.F)))
            { 
            isShopOpen=false;
            CloseShop();


        }
        if (cg == null) { print("ERROR NULL"); }

    }

    void OpenShop()
    {
        // Activate your shop UI panel here
        Cursor.lockState = CursorLockMode.None;
        shop.SetActive(true);
        cg.alpha = 0;
        Debug.Log("Shop opened");
        player.GetComponent<FirstPersonMovement>().SetMovementEnabled(false);
    }

    void CloseShop()
    {
        // Deactivate your shop UI panel here
        Cursor.lockState = CursorLockMode.Locked;
        shop.SetActive(false);
        cg.alpha = 1.0f;
        Debug.Log("Shop closed");
        player.GetComponent<FirstPersonMovement>().SetMovementEnabled(true);
    }
    public void getInteractionPoint(GameObject inputplayer)
    {
        player = inputplayer;
    }

    public void NewGun(int gunwanted)
    {
       lastcheckedmoney = player.GetComponent<AddMoney>().money;
        if (costofeachgun[gunwanted] <= lastcheckedmoney)
                {
            player.GetComponentInChildren<FixedGunManager>().AddNewGun(gunwanted);
            player.GetComponent<AddMoney>().money -= costofeachgun[gunwanted];
        }
    }

    public void newgrenades(int grenadesamount)
    {
        lastcheckedmoney = player.GetComponent<AddMoney>().money;
        if (costofeachgun[7] <= lastcheckedmoney)
        {
            player.GetComponentInChildren<GrenadeThrower>().getmoreGrenades(grenadesamount);
            player.GetComponent<AddMoney>().money -= costofeachgun[7];
        }
    }
}
