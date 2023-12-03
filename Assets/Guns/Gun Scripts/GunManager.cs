using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject RiflePoint;
    public GameObject PistolPoint;
    public GameObject ShotgunPoint;

    private GameObject currentGunObject;
    private GameObject currentCosmeticObject;

    public GameObject[] availableGuns;
    public GameObject[] availableCosmo;
    private GameObject[] items = new GameObject[2];
    private GameObject[] cosmoItem = new GameObject[2];
    private int selectedGunIndex = 0;
    private GameObject newgun;
    private GameObject newCosmetic;
    void Start()
    {
        ChooseGun();
        items[0] = availableGuns[0];
        cosmoItem[0] = availableCosmo[0];
        items[1] = availableGuns[1];
        cosmoItem[1] = availableCosmo[1];
    }

    void Update()
    {
        HandleWeaponSwitching();
    }

    void HandleWeaponSwitching()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0f)
        {
            SwitchToNextGun();
        }
        else if (scrollWheel < 0f)
        {
            SwitchToPreviousGun();
        }

        for (int i = 0; i < availableGuns.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedGunIndex = i;
                ChooseGun();
            }
        }
    }

    void SwitchToNextGun()
    {
        selectedGunIndex = 0;
        ChooseGun();
    }

    void SwitchToPreviousGun()
    {
        selectedGunIndex = 1;
        ChooseGun();
    }

    void ChooseGun()
    {
        DestroyCurrentGun();

        GameObject selectedGun = InstantiateGun(items[selectedGunIndex], gameObject);
        currentGunObject = selectedGun;

        InstantiateCosmo(cosmoItem[selectedGunIndex], GetGunPoint());
        currentCosmeticObject = currentGunObject; 

        Debug.Log("You chose " + currentGunObject.name + " as your current gun.");
    }

    public void AcquireNewGun(int id)
    {
        DestroyCurrentGun();


        if (selectedGunIndex == 0)
        {
            items[0] = availableGuns[id];
            cosmoItem[0] = availableCosmo[id];
            newgun = items[0];
            newCosmetic = cosmoItem[0];
        }
        else if (selectedGunIndex == 1)
        {
            items[1] = availableGuns[selectedGunIndex];
            cosmoItem[1] = availableCosmo[selectedGunIndex];
            newgun = items[1];
            newCosmetic = cosmoItem[1];
        }
        currentGunObject = newgun;

        currentCosmeticObject = newCosmetic;

        Debug.Log($"You acquired {currentGunObject.name}.");
        ChooseGun();
    }

    GameObject InstantiateGun(GameObject gunPrefab, GameObject gunPoint)
    {
        GameObject newGun = Instantiate(gunPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
        newGun.transform.parent = gunPoint.transform;
        return newGun;
    }

    GameObject InstantiateCosmo(GameObject gunPrefab, GameObject cosmeticPoint)
    {
        GameObject newcosmo = Instantiate(gunPrefab, cosmeticPoint.transform.position, cosmeticPoint.transform.rotation);
            newcosmo.transform.parent = cosmeticPoint.transform;
        return newcosmo;
    }

    GameObject GetGunPoint()
    {
        switch (availableGuns[selectedGunIndex].tag)
        {
            case "Rifle":
                return RiflePoint;
            case "Pistol":
                return PistolPoint;
            case "Shotgun":
                return ShotgunPoint;
            default:
                return ShotgunPoint; 
        }
    }


    void DestroyCurrentGun()
    {
        if (currentGunObject != null)
        {
            Destroy(currentGunObject);
        }

        if (currentCosmeticObject != null)
        {
            Destroy(currentCosmeticObject);
        }
    }
}
