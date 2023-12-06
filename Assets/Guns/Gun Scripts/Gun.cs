
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;

public class Gun : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public float firerate = 15f;
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
    public bool allowshooting = false;
    public TMP_Text ammocount;
    public int burstamount = 0;
    public float timebetweenbursts = 0;
    private Camera playerCamera;
    private GameObject player;
    private int moneytosendtoplayer;
    private GameObject shop;
    private int burstshotsfired = 0;

    public bool infAmmo = false;

    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;
        WeaponManager parentComponent = parentObject.GetComponent<WeaponManager>();
        //playerCamera = GameObject.Find("First Person Camera").GetComponent<Camera>();
        //playerCamera = LobbySceneManagement.singleton.playerCamObject.GetComponent<Camera>();
        playerCamera = LobbySceneManagement.singleton.playerCamObject;
        ammocount = LobbySceneManagement.singleton.ammoCountText;
        crosshairImage = LobbySceneManagement.singleton.crosshair;
        //Canvas canvas = GameObject.Find("GUI").GetComponent<Canvas>();
        //crosshairImage = canvas.GetComponentInChildren<Image>();
        //TextMeshProUGUI[] textMeshProComponents = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        //ammocount = canvas.transform.Find("Ammo Count")?.GetComponent<TextMeshProUGUI>();
        currentammo = MagSize;

        //add ammoCount tmp
        //add crosshair images

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

        if (LobbySceneManagement.singleton.getLocalPlayer().GetComponent<FirstPersonMovement>().isMovementEnabled == false) { allowshooting = false; }
        if (LobbySceneManagement.singleton.dead) { allowshooting = false; }
        if (!LobbySceneManagement.singleton.playerCamObject.transform.Find("Holder").gameObject.enabled) { allowshooting = false; }
        }
        //Async start astuff
        if (playerCamera == null) {
            //playerCamera = LobbySceneManagement.singleton.playerCamObject.GetComponent<Camera>();
            playerCamera = LobbySceneManagement.singleton.playerCamObject;
        }
        if (ammocount == null) {
            ammocount = LobbySceneManagement.singleton.ammoCountText;
        }
        if (crosshairImage == null) {
            crosshairImage = LobbySceneManagement.singleton.crosshair;
        }



        if(burstshotsfired>0)
        {
            allowshooting = false;
        }
        else if(burstshotsfired == burstamount)
        {
            burstshotsfired = 0;
            Invoke("allowShoot", 0.1f);
        }
         shop = GameObject.Find("Shop");
        if (shop == null)
        {
            Invoke("allowShoot", 0.1f);
        }
        else
        { 
            allowshooting = false; 
        }
        if (allowshooting)
        {
            if (isreloading)
            {
                if (IsAutomatic && !cosmetic)
                {
                    anim.SetBool("Continuous", false);
                }
                if (isAiming)
                {
                    float targetFOV = isAiming ? adsFOV : normalFOV;
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * adsSpeed);
                }
                return;
            }
            if (currentammo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentammo != MagSize) && !cosmetic)
            {
                StartCoroutine(Reload());
                return;
            }
            if (IsAutomatic)
            {
                if (Input.GetButton("Fire1") && Time.time >= timetofire)
                {
                    timetofire = Time.time + (1f / firerate);
                    if (!cosmetic) anim.SetBool("Continuous", true);
                    Shoot();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    if (!cosmetic) anim.SetBool("Continuous", false);
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
                if (infAmmo) {
                    ammocount.text = currentammo + "/∞"; 
                } else {
                    ammocount.text = currentammo + "/" + maxAmmo;
                }
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

    }

    public void allowShoot() {
        allowshooting = true;
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
        if(MagSize <= maxAmmo || infAmmo)
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
        if (burstamount != 0)
        {
            allowshooting = false;
            StartCoroutine(ShootBurst());
            return;
        }
        flash.Play();
        if (!cosmetic)
        {
            currentammo--;
            anim.SetTrigger("Fire");
            GameObject sound = Instantiate(ShotFX);
            Destroy(sound, 2);

                for (int j = 0; j < bulletspershot; j++)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, accuracyfunct(playerCamera.transform.forward, accuracy), out hit, range))
                    {
                        Debug.Log("Hit this: " + hit.transform.name);
                        //Debug.Log(playerCamera.transform.forward);
                        //Debug.Log("Hit reg: " + hit.transform.GetComponentInChildren<EnemyHitRegister>());
                    if (hit.transform.GetComponent<EnemyHitRegister>() != null) {
                        Debug.Log("hit parent enemy mesh");
                        EnemyHitRegister enem = hit.transform.GetComponent<EnemyHitRegister>();
                        enem.takeDamage(damage, LobbySceneManagement.singleton.getLocalPlayer().identity, "Single");
                    } else if (hit.transform.GetComponentInParent<EnemyHitRegister>() != null) {
                        Debug.Log("hit child enemy mesh");
                        EnemyHitRegister enem = hit.transform.GetComponentInParent<EnemyHitRegister>();
                        enem.takeDamage(damage, LobbySceneManagement.singleton.getLocalPlayer().identity, "Single");
                    }
                        /*
                        target Target = hit.transform.GetComponent<target>();
                        if (Target != null)
                        {
                            if (Target.TakeDamage(damage))
                            {
                                moneytosendtoplayer = Target.gimmemoney();
                                sendmoney();
                            }
                        }*/
                        GameObject impact = Instantiate(impacteffect, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impact, 1);
                    
                    }
                }
                
            
        }
    }
    private IEnumerator ShootBurst()
    {

        for (int i = 0; i < burstamount; i++)
        {
            burstshotsfired++;
            flash.Play();
            currentammo--;
            anim.SetTrigger("Fire");
            GameObject sound = Instantiate(ShotFX);
            Destroy(sound, 2);
            for (int j = 0; j < bulletspershot; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, accuracyfunct(playerCamera.transform.forward, accuracy), out hit, range))
                {
                    Debug.Log("Hit this: " + hit.transform.name);
                    //Debug.Log(playerCamera.transform.forward);
                    //Debug.Log("Hit reg: " + hit.transform.GetComponentInChildren<EnemyHitRegister>());
                    //Debug.Log("Hit reg child: " + hit.transform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<EnemyHitRegister>());
                    if (hit.transform.GetComponent<EnemyHitRegister>() != null) {
                        Debug.Log("hit parent enemy mesh");
                        EnemyHitRegister enem = hit.transform.GetComponent<EnemyHitRegister>();
                        enem.takeDamage(damage, LobbySceneManagement.singleton.getLocalPlayer().identity, "Single");
                    } else if (hit.transform.GetComponentInParent<EnemyHitRegister>() != null) {
                        Debug.Log("hit child enemy mesh");
                        EnemyHitRegister enem = hit.transform.GetComponentInParent<EnemyHitRegister>();
                        enem.takeDamage(damage, LobbySceneManagement.singleton.getLocalPlayer().identity, "Single");
                    }
                    /*
                    target Target = hit.transform.GetComponent<target>();
                
                    if (Target != null)
                    {
                        if (Target.TakeDamage(damage))
                        {
                            moneytosendtoplayer = Target.gimmemoney();
                            sendmoney();
                        }
                    }
                    */
                    GameObject impact = Instantiate(impacteffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 1);
                }
                yield return new WaitForSeconds(timebetweenbursts); // Wait for 1 second between each inner loop iteration
            }
        }
    }

    
    void sendmoney()
    {
        if (player == null)
        {
            Transform pt = playerCamera.transform.parent;
            player = pt.gameObject;
        }
        player.GetComponent<AddMoney>().money += moneytosendtoplayer;
        moneytosendtoplayer = 0;
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
