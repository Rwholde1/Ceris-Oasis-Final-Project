using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject[] Cosmetics;
    public Animator animator;

    public int selectedweapon = 0;
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        int previousWeapon = selectedweapon;
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedweapon >= transform.childCount - 1) selectedweapon = 0;
            else selectedweapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedweapon <= 0) selectedweapon = transform.childCount - 1;
            else selectedweapon--;
        }
        if(previousWeapon != selectedweapon)
        {
            SelectWeapon();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedweapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedweapon = 1;
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedweapon)
            {
                weapon.gameObject.SetActive(true);
                Cosmetics[i].gameObject.SetActive(true);
                if (weapon.gameObject.tag == "Rifle")
                {
                    animator.SetBool("Rifle", true);
                    animator.SetBool("Pistol", false);
                    animator.SetBool("Shotgun", false);
                }
                else if (weapon.gameObject.tag == "Shotgun")
                {
                    animator.SetBool("Rifle", false);
                    animator.SetBool("Pistol", false);
                    animator.SetBool("Shotgun", true);
                }
                else if (weapon.gameObject.tag == "Pistol")
                {
                    animator.SetBool("Rifle", false);
                    animator.SetBool("Pistol", true);
                    animator.SetBool("Shotgun", false);
                }
            }
            else
            {
                weapon.gameObject.SetActive(false);
                Cosmetics[i].gameObject.SetActive(false);
            }

            i++;

        }

    }
}
