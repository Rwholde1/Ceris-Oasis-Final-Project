using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
    public GameObject RiflePoint;
    public GameObject PistolPoint;
    public GameObject ShotgunPoint; // Attachment point for secondary gun

    private GameObject primaryGunObject;
    private GameObject secondaryGunObject;

    public GameObject[] availableGuns;
    public GameObject[] availableCosmo;
    [SerializeField]private int gunid = 0;

    void Start()
    {
        ChoosePrimaryGun();
        //ChooseSecondaryGun();
    }

    public void ChoosePrimaryGun()
    {
        DestroyGun(primaryGunObject); // Destroy existing primary gun if any
        GameObject newgun = GetGun(availableGuns[gunid]);
        GameObject cosmo = Cosmo(availableCosmo[gunid]);
        // Instantiate the primary gun object
        Debug.Log($"You chose {newgun.name} as your primary gun.");
    }

    GameObject checkTags(GameObject gun)
    {
        GameObject newgun;
        if (gun.CompareTag("Rifle"))
        {
            Debug.Log("RIFLE: Instantiating gun:" + gun.name);
             newgun = Instantiate(gun, transform.position, transform.rotation);
            newgun.transform.parent = transform;
            return newgun;
        }
        else if (gun.CompareTag("Pistol"))
        {
            Debug.Log("PISTOL: Instantiating gun:" + gun.name);
             newgun = Instantiate(gun, transform.position, transform.rotation);
            newgun.transform.parent = transform;
            return newgun;
        }
        else if (gun.CompareTag("Shotgun"))
        {
            Debug.Log("SHOTGUN: Instantiating gun:" + gun.name);
             newgun = Instantiate(gun, transform.position, transform.rotation);
            newgun.transform.parent = transform;
            return newgun;
        }
        else
        {
            Debug.Log("TAG ERROR: Instantiating gun:" + gun.name + " " + gun.tag);
            newgun = Instantiate(gun, transform.position, transform.rotation);
            newgun.transform.parent = transform;
            return newgun;
        }
    }

    GameObject Cosmo(GameObject gun)
    {
        GameObject newgun;
        if (gun.CompareTag("Rifle"))
        {
            Debug.Log("RIFLE: Instantiating gun:" + gun.name);
            newgun = Instantiate(gun, RiflePoint.transform.position, RiflePoint.transform.rotation);
            newgun.transform.parent = RiflePoint.transform;
            return newgun;
        }
        else if (gun.CompareTag("Pistol"))
        {
            Debug.Log("PISTOL: Instantiating gun:" + gun.name);
            newgun = Instantiate(gun, PistolPoint.transform.position, PistolPoint.transform.rotation);
            newgun.transform.parent = PistolPoint.transform;
            return newgun;
        }
        else if (gun.CompareTag("Shotgun"))
        {
            Debug.Log("SHOTGUN: Instantiating gun:" + gun.name);
            newgun = Instantiate(gun, ShotgunPoint.transform.position, ShotgunPoint.transform.rotation);
            newgun.transform.parent = ShotgunPoint.transform;
            return newgun;
        }
        else
        {
            Debug.Log("TAG ERROR: Instantiating gun:" + gun.name + " " + gun.tag);
            newgun = Instantiate(gun, ShotgunPoint.transform.position, ShotgunPoint.transform.rotation);
            newgun.transform.parent = ShotgunPoint.transform;
            return newgun;
        }
    }

    /*    void ChooseSecondaryGun()
        {
            DestroyGun(secondaryGunObject); // Destroy existing secondary gun if any

            Debug.Log("\nAvailable secondary guns:");
            for (int i = 0; i < availableGuns.Length; i++)
            {
                Debug.Log($"{i + 1}. {availableGuns[i]}");
            }

            int choice = GetGunChoice();

            // Instantiate the secondary gun object
            secondaryGunObject = Instantiate(availableGuns[choice], secondaryGunAttachment.position, secondaryGunAttachment.rotation);
            secondaryGunObject.transform.parent = secondaryGunAttachment;
            Debug.Log($"You chose {secondaryGunObject.name} as your secondary gun.");
        }*/

    public GameObject GetGun(GameObject newgun)
    {

        return checkTags(newgun);
    }

    void DestroyGun(GameObject gunObject)
    {
        if (gunObject != null)
        {
            Destroy(gunObject);
        }
    }

    public void changegunid(int newid)
    {
        gunid = newid;
    }
}
