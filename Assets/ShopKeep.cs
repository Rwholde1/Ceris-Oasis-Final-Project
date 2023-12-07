using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeep : MonoBehaviour
{

    /// <summary>
    /// NOTE! Currently the buttons do nothing, You cannot Exit with F, The hud does not disable, and the pause menu still appears
    /// </summary>
    private bool isShopOpen = false;
    private GameObject player;
    public GameObject shop;
    public GameObject GUI;
    private CanvasGroup cg;
    private int lastcheckedmoney = 0;
    public int[] costofeachgun;
    private void Start()
    {
        //cg = GUI.GetComponent<CanvasGroup>();
        
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
        //if (cg == null) { print("ERROR NULL"); }

    }

    void OpenShop()
    {
        // Activate your shop UI panel here
        Cursor.lockState = CursorLockMode.None;
        shop.SetActive(true);

        /*
        CanvasGroup[] cg = GUI.GetComponentsInChildren<CanvasGroup>();
        foreach(CanvasGroup canvas in cg) {
            canvas.alpha = 0f;
        }*/
        GUI.SetActive(false);
        Debug.Log("Shop opened");
        player.GetComponent<FirstPersonMovement>().SetMovementEnabled(false);
    }

    void CloseShop()
    {
        // Deactivate your shop UI panel here
        Cursor.lockState = CursorLockMode.Locked;
        shop.SetActive(false);
        /*
        CanvasGroup[] cg = GUI.GetComponentsInChildren<CanvasGroup>();
        foreach(CanvasGroup canvas in cg) {
            canvas.alpha = 1.0f;
        }*/
        GUI.SetActive(true);
        Debug.Log("Shop closed");
        player.GetComponent<FirstPersonMovement>().SetMovementEnabled(true);
    }
    public void getInteractionPoint(GameObject inputplayer)
    {
        //player = inputplayer;
        //player = LobbySceneManagement.singleton.getLocalPlayer().gameObject;
        player = LobbySceneManagement.singleton.playerCamObject.gameObject;
    }

    public void NewGun(int gunwanted)
    {
        Debug.Log("player tries to buy new gun " + gunwanted);
       lastcheckedmoney = player.GetComponent<AddMoney>().money;
       Debug.Log("With money " + lastcheckedmoney);
        if (costofeachgun[gunwanted] <= lastcheckedmoney)
                {
                    Debug.Log("player bought gun");
                    Debug.Log(player.GetComponentInChildren<FixedGunManager>());
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

    public void MoreAmmo()
    {
        FixedGunManager[] deez = player.GetComponentsInChildren<FixedGunManager>();
        foreach(FixedGunManager nutz in deez)
        {
            if(nutz.gameObject.GetComponentInChildren<Gun>() != null)
            {
                if (nutz.gameObject.GetComponentInChildren<Gun>().gameObject.activeInHierarchy)
                {
                    nutz.gameObject.GetComponentInChildren<Gun>().maxAmmo += 25;
                    player.GetComponent<AddMoney>().money -= 25;
                }
            }

        }
    }
}


