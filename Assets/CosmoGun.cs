
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class CosmoGun: MonoBehaviour
{
    public float firerate = 15f;
    public ParticleSystem flash;
    public bool IsAutomatic = false;
    public float reloadtime = 1f;
    private bool isreloading = false;
    private float timetofire = 0f;



    // Update is called once per frame
    void Update()
    {
        if (IsAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time >= timetofire)
            {
                timetofire = Time.time + (1f / firerate);
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= timetofire)
            {
                timetofire = Time.time + (1f / firerate);
                Shoot();
            }
        }
    }



    private void Shoot() { 
        flash.Play();
        //GameObject sound = Instantiate(ShotFX);
        }
    }
