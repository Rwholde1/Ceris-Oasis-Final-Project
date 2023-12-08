using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixedGunManager : MonoBehaviour
{

    public GameObject[] guns;
    public int gunID1;
    public int gunID2;
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;
    public int gunactive = 1;

    void Start() {
        if (SceneManager.GetActiveScene().name == "Pherris Reactor") {
            Debug.Log("isPherris");
            Invoke("ResetGuns", 2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(gun1 == null)
        {
            gun1 = Instantiate(guns[gunID1], transform);
        }
        if (gun2 == null)
        {
            gun2 = Instantiate(guns[gunID2], transform);
            gun2.SetActive(false);
        }
        if(gunactive == 1 && !gun1.activeInHierarchy)
        {
            gun1.SetActive(true);
            gun2.SetActive(false);
            if (gunID1 == 0) {
                gun2.GetComponent<Gun>().ammocount.text = "0/0";
            }
            print("GUN SWITCHED TO GUN 1");
        }
        if(gunactive == 2 && !gun2.activeInHierarchy)
        {
            gun2.SetActive(true);
            gun1.SetActive(false);
            if (gunID2 == 0) {
                gun1.GetComponent<Gun>().ammocount.text = "0/0";
            }
            print("GUN SWITCHED TO GUN 2");
        }
        HandleWeaponSwitching();
    }
    void HandleWeaponSwitching()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0f)
        {
            print("Scrollwheel up");
            if (gunactive == 1)
            {
                gunactive = 2;
                return;
            }
            if (gunactive == 2)
            {
                gunactive = 1;
                return;
            }

        }
        else if(scrollWheel < 0f)
        {
            print("Scrollwheel down");
            if (gunactive == 1)
            {
                gunactive = 2;
                return;
            }
            if (gunactive == 2)
            {
                gunactive = 1;
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            gunactive = 1;
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunactive = 2;
            return;
        }

    }

    public void AddNewGun(int newgun)
    {
        Debug.Log("new gun timeeee");
        //Ensures empty slot is filled before overwriting existing gun
        if (gunID1 == 0) {
            Destroy(gun1);
            gunID1 = newgun;
        } else if (gunID2 == 0) {
            Destroy(gun2);
            gunID2 = newgun;
        } else {
            //Overwrites existing gun
            if(gunactive == 1) {
                Destroy(gun1);
                gunID1 = newgun;
            } else if (gunactive == 2) {
                Destroy(gun2);
                gunID2 = newgun;
            }
        }
        
    }

    public void ResetGuns()
    {
        /*
        Debug.Log("new gun timeeee");
        //Ensures empty slot is filled before overwriting existing gun
        if (gunID1 == 0) {
            Destroy(gun1);
            gunID1 = newgun;
        } else if (gunID2 == 0) {
            Destroy(gun2);
            gunID2 = newgun;
        } else {
            //Overwrites existing gun
            if(gunactive == 1) {
                Destroy(gun1);
                gunID1 = newgun;
            } else if (gunactive == 2) {
                Destroy(gun2);
                gunID2 = newgun;
        }
        }*/
        Debug.Log("resetting guns");
        Destroy(gun1);
        Destroy(gun2);
        gunID1 = 7;
        gunID2 = 0;
        gunactive = 1;
    }

    //fixPherris

}
