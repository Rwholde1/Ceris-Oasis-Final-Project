using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] GunPrefabs;
    public GameObject[] COSMETICGuns;
    public GameObject RiflePoint;
    public GameObject PistolPoint;
    public GameObject ShotgunPoint;
    public Camera playerCamera;

    [SerializeField] private int currentGunID = 0;
    private GameObject currentGun;
    private GameObject currentCosmo;

    void Start()
    {
        // Instantiate the initial gun based on the currentGunID
        currentGun = InstantiateGun(currentGunID);
        currentCosmo = InstantiateCosmo(currentGunID);
    }

    void Update()
    {
        // Your existing logic for updating the guns goes here
        UpdateGunsLogic();

        // Always set currentGun to the corresponding prefab
        currentGun = GunPrefabs[currentGunID];
    }

    // Placeholder method for your actual logic for updating the guns
    private void UpdateGunsLogic()
    {
        Debug.Log("Stage 1");

        if (currentGunID != 0)
        {
            Debug.Log("Stage 2");

            if (currentGun != GunPrefabs[currentGunID])
            {
                setGun(currentGunID);
                Debug.Log("Stage 3");
            }
        }
    }

    public void setGun(int GunID)
    {
        // Check if the new gunID is different from the currentGunID
        if (GunID != currentGunID)
        {
            // Check if the currentGun is enabled before destroying it
            if (currentGun != null && currentGun.activeSelf)
            {
                // Disable the currentGun before destroying it
                currentGun.SetActive(false);
                currentCosmo.SetActive(false);

                // Destroy the old gun
                Destroy(currentGun);
                Destroy(currentCosmo);
            }

            // Instantiate the new gun immediately
            currentGun = InstantiateGun(GunID);
            currentCosmo = InstantiateCosmo(GunID);
            currentGunID = GunID;
        }
    }

    private GameObject InstantiateGun(int gunID)
    {
        // Instantiate the gun based on the gunID and return the instance
        // (You might want to adjust this based on your specific needs)
        // Example:
        GameObject newGun = Instantiate(GunPrefabs[gunID], playerCamera.transform.position, playerCamera.transform.rotation, gameObject.transform);

        // Enable the new gun
        newGun.SetActive(true);

        return newGun;
    }

    private GameObject InstantiateCosmo(int gunID)
    {
        // Instantiate the gun based on the gunID and return the instance
        // (You might want to adjust this based on your specific needs)
        // Example:
        GameObject newCosmo;

        if (GunPrefabs[gunID].CompareTag("Rifle"))
        {
            Debug.Log("RIFLE: Instantiating gun:" + gunID);
            newCosmo = Instantiate(COSMETICGuns[gunID], RiflePoint.transform.position, RiflePoint.transform.rotation);
            newCosmo.transform.parent = RiflePoint.transform;
        }
        else if (GunPrefabs[gunID].CompareTag("Pistol"))
        {
            Debug.Log("PISTOL: Instantiating gun:" + gunID);
            newCosmo = Instantiate(COSMETICGuns[gunID], PistolPoint.transform.position, PistolPoint.transform.rotation);
            newCosmo.transform.parent = PistolPoint.transform;
        }
        else if (GunPrefabs[gunID].CompareTag("Shotgun"))
        {
            Debug.Log("SHOTGUN: Instantiating gun:" + gunID);
            newCosmo = Instantiate(COSMETICGuns[gunID], ShotgunPoint.transform.position, ShotgunPoint.transform.rotation);
            newCosmo.transform.parent = ShotgunPoint.transform;
        }
        else
        {
            Debug.LogError("TAG ERROR, CREATING EMPTY");
            newCosmo = Instantiate(COSMETICGuns[0], playerCamera.transform.position, playerCamera.transform.rotation);
            newCosmo.transform.parent = playerCamera.transform;
        }

        // Enable the new gun
        newCosmo.SetActive(true);

        if (newCosmo == null)
        {
            Debug.LogError("Cosmetic gun is null after instantiation!");
        }

        return newCosmo;
    }


}
