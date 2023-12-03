using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CosmoGunScript : MonoBehaviour
{

    public GameObject[] guns;
    public int gunID1;
    public int gunID2;
    public Transform riflepos;
    public Transform pistolpos;
    public Transform shotpos;
    public Sprite[] GunPNG;
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;
    [SerializeField] private int gunactive = 1;
    [SerializeField] Animator animator;
    [SerializeField] Image gun1IMG;
    [SerializeField] Image gun2IMG;
    private FixedGunManager Gman;
    private Color gun1Color = Color.clear;
    private Color gun2Color = Color.clear;
    // Start is called before the first frame update
    private void Start()
    {
        Gman = GetComponent<FixedGunManager>();
        CopyOtherScript();
        gun1Color.a = 1.0f;
        gun2Color.a = 0.3f;
        gun1IMG.sprite = GunPNG[gunID1];
        gun2IMG.sprite = GunPNG[gunID2];
        gun1IMG.color = gun1Color;
        gun2IMG.color = gun2Color;


    }
    // Update is called once per frame
    void Update()
    {
        CopyOtherScript();
        if (gun1 == null)
        {
            gun1 = Instantiate(guns[gunID1], CheckTags(gunID1));
            AnimChecker(gun1);
            gun1IMG.sprite = GunPNG[gunID1];
        }
        if (gun2 == null)
        {
            gun2 = Instantiate(guns[gunID2], CheckTags(gunID2));
            gun2.SetActive(false);
            AnimChecker(gun2);
            gun2IMG.sprite = GunPNG[gunID2];
        }
        if (gunactive == 1 && !gun1.activeInHierarchy)
        {
            gun1Color.a = 1.0f;
            gun1.SetActive(true);
            gun2.SetActive(false);
            print("GUN SWITCHED TO GUN 1");
            AnimChecker(gun1);
            gun2Color.a = 0.3f;
            gun1IMG.color = gun1Color;
            gun2IMG.color = gun2Color;
        }
        if (gunactive == 2 && !gun2.activeInHierarchy)
        {
            gun1Color.a = 0.3f;
            gun2.SetActive(true);
            gun1.SetActive(false);
            print("GUN SWITCHED TO GUN 2");
            AnimChecker(gun2);
            gun2Color.a = 1.0f;
        }
        gun1IMG.color = gun1Color;
        gun2IMG.color = gun2Color;
        print(gun2IMG.color);
    }

    void CopyOtherScript()
    {
        if(gunID1 != Gman.gunID1)
        {
            AddNewCosmo(Gman.gunID1);
            gunID1 = Gman.gunID1;
        }
        if(gunID2 != Gman.gunID2)
        {
            AddNewCosmo(Gman.gunID2);
            gunID2 = Gman.gunID2;
        }
        gunactive = Gman.gunactive;
    }

    public void AddNewCosmo(int newgun)
    {
        if (gunactive == 1)
        {
            gun1IMG.sprite = GunPNG[newgun];
            Destroy(gun1);
            gunID1 = newgun;
        }
        else if (gunactive == 2)
        {
            gun2IMG.sprite = GunPNG[newgun];
            Destroy(gun2);
            gunID2 = newgun;
        }
    }
    void AnimChecker(GameObject weapon)
    {
        if (weapon == null)
        {
            return;
        }
        if (weapon.gameObject.tag == "Rifle")
        {
            print("Rifle");
            animator.SetBool("Rifle", true);
            animator.SetBool("Pistol", false);
            animator.SetBool("Shotgun", false);
        }
        else if (weapon.gameObject.tag == "Shotgun")
        {
            print("SHOTGUN");
            animator.SetBool("Rifle", false);
            animator.SetBool("Pistol", false);
            animator.SetBool("Shotgun", true);
        }
        else if (weapon.gameObject.tag == "Pistol")
        {
            print("PISTOL");
            animator.SetBool("Rifle", false);
            animator.SetBool("Pistol", true);
            animator.SetBool("Shotgun", false);
        }
    }
    Transform CheckTags(int id)
    {
        switch (guns[id].tag)
        {
            case "Rifle":
                return riflepos;
            case "Pistol":
                return pistolpos;
            case "Shotgun":
                return shotpos;
            default:
                return riflepos;
        }
    }
}
