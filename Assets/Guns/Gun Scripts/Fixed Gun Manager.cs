using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FixedGunManager : MonoBehaviour
{

    public GameObject[] guns;
    public int gunID1;
    public int gunID2;
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;
    public int gunactive = 1;

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
            print("GUN SWITCHED TO GUN 1");
        }
        if(gunactive == 2 && !gun2.activeInHierarchy)
        {
            gun2.SetActive(true);
            gun1.SetActive(false);
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
        if(gunactive == 1)
        {
            Destroy(gun1);
            gunID1 = newgun;
        }
        else if (gunactive == 2)
        {
            Destroy(gun2);
            gunID2 = newgun;
        }
    }

}
