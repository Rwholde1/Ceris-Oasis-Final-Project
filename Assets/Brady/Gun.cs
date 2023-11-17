
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float firerate = 15f;
    public Camera cam;
    public ParticleSystem flash;
    public bool IsAutomatic = false;
    public Animator anim;
    public GameObject ShotFX;
    public GameObject impacteffect;
    public float accuracy = 0;
    public bool cosmetic = false;
    public int maxAmmoReserviour = 250;
    public int maxAmmo = 100;
    public int MagSize = 10;
    private int currentammo;
    public float reloadtime = 1f;
    private bool isreloading = false;
    public int bulletspershot = 1;
    private float timetofire = 0f;
    public float normalFOV = 60f;
    public float adsFOV = 30f;
    public float adsSpeed = 5f;
    private bool isAiming = false;
    public Image crosshairImage;

    public TextMeshProUGUI ammocount;

    private Camera playerCamera;
    void Start()
    {
        currentammo = MagSize;
        playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (!cosmetic)
        {
            isreloading = false;
            anim.SetBool("IsReloading", false);
            isAiming = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (isreloading)
        {
            if (IsAutomatic)
            {
                anim.SetBool("Continuous", false);
            }
            if(isAiming)
            {
                float targetFOV = isAiming ? adsFOV : normalFOV;
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * adsSpeed);
            }
            return;
        }
        if(currentammo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentammo != MagSize))
        {
            StartCoroutine(Reload());
            return;
        }
        if (IsAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time >= timetofire)
            {
                timetofire = Time.time + (1f / firerate);
                anim.SetBool("Continuous", true);
                Shoot();
            }
            if (Input.GetButtonUp("Fire1"))
                {
                anim.SetBool("Continuous", false);
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

        if (!cosmetic)
        {
            ammocount.text = currentammo + "/" + maxAmmo;
            if (crosshairImage != null)
            {
                float targetScale = isAiming ? 0.5f : 1f;
                crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, new Vector3(targetScale, targetScale, 1f), Time.deltaTime * adsSpeed);
            }


            HandleFOVInput();
            float targetFOV = isAiming ? adsFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * adsSpeed);
        }

    }
    IEnumerator Reload()
    {
        isreloading = true;
        Debug.Log("Reloading");
        anim.SetBool("IsReloading", true) ;

        yield return new WaitForSeconds(reloadtime - .25f);
        anim.SetBool("IsReloading", false);
        yield return new WaitForSeconds(.25f);
        if(currentammo != 0)
        {
            maxAmmo += currentammo;
            currentammo = 0;
        }
        if(MagSize <= maxAmmo)
        {
            currentammo = MagSize;
            maxAmmo -=MagSize;
        }
        else if(MagSize > maxAmmo)
        {
            currentammo = maxAmmo;
            maxAmmo = 0;
        }
        else if (maxAmmo == 0)
        {
            Debug.Log("All Out Of Ammo");
        }
        isreloading = false;
    }

    void HandleFOVInput()
    {
        float sensitivity = isAiming ? 0.5f : 1f; // Adjust the sensitivity factor as needed

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        if (Input.GetButtonDown("Fire2") && !isAiming)
        {
            isAiming = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;
        }
    }

    private void Shoot() { 
        flash.Play();
        if (!cosmetic)
        {
            currentammo--;
            anim.SetTrigger("Fire");
            GameObject sound = Instantiate(ShotFX);
            Destroy(sound, 2);
            for (int i = 0; i < bulletspershot; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, accuracyfunct(cam.transform.forward, accuracy), out hit, range))
                {
                    Debug.Log(hit.transform.name);
                    Debug.Log(cam.transform.forward);
                    target Target = hit.transform.GetComponent<target>();
                    if (Target != null)
                    {
                        Target.TakeDamage(damage);

                    }
                    GameObject impact = Instantiate(impacteffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 1);
                }
            }
        }
    }


    Vector3 accuracyfunct(Vector3 camForw,  float accuracy)
    {
        float random1 = 0f;
        float random2 = 0f;
        if (isAiming)
        {
            random1 = UnityEngine.Random.Range(-accuracy/3, accuracy/3) / 100;
            random2 = UnityEngine.Random.Range(-accuracy/3, accuracy / 3) / 100;
        }
        else
        {

            random1 = UnityEngine.Random.Range(-accuracy, accuracy) / 100;
            random2 = UnityEngine.Random.Range(-accuracy, accuracy) / 100;
        }
        Debug.Log(random1 + " " + random2);
        return new Vector3(camForw.x + random1, camForw.y + random2, camForw.z);
    }
}
