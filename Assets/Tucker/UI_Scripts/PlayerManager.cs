using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerManager : NetworkBehaviour
{

    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    [SerializeField] int cooldown1 = 10;
    [SerializeField] int cooldown2 = 10;

    private bool alive = true;
    [SerializeField] object primaryWeapon;
    [SerializeField] object secondaryWeapon;
    public HealthBar healthBar;
    public AbilityManager abilities;
    public StatsManager stats;

    public Camera mainCam;
    public NetworkObject basicPing;

    public TMP_Text ammoText;
    public Image crosshair;

    //grenades
    //shielding?


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        abilities.setCooldown1(cooldown1);
        abilities.setCooldown2(cooldown2);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            alive = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stats.addDeath(1);
            currentHealth = maxHealth;
            healthBar.setHealth(maxHealth);
        }

        if(Input.GetKeyDown(KeyCode.J)) {
            stats.addElim(1);
        }
        if(Input.GetKeyDown(KeyCode.K)) {
            stats.addAssist(1);
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            takeDamage(10);
        }
        if(Input.GetMouseButtonDown(1)) {
            var dmg = Mathf.Floor(GetComponent<Transform>().localEulerAngles.y);
            stats.addDamage(1, (int) dmg);
        }

        if(Input.GetMouseButtonDown(2)) {
            Debug.Log("click ping");
            if (IsServer) {
                createPing();
            } else {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                Debug.Log("call ping");
                if(Physics.Raycast(ray, out RaycastHit hit)) {
                    Debug.Log("ping");
                    //Sets height offset and instantiates ping
                    Vector3 offset = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
                    createPingServerRpc(offset);
                }
            }
        }

        if (LobbySceneManagement.singleton.ammoCountText == null) {
            LobbySceneManagement.singleton.ammoCountText = ammoText;
        }
        if (LobbySceneManagement.singleton.crosshair == null) {
            LobbySceneManagement.singleton.crosshair = crosshair;
        }
        if (LobbySceneManagement.singleton.playerCamObject == null) {
            LobbySceneManagement.singleton.playerCamObject = mainCam;
        }
    }

    void takeDamage(int damageIn) {
        currentHealth -= damageIn;
        healthBar.setHealth(currentHealth);
    }

    public void createPing() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Debug.Log("call ping");
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            Debug.Log("ping");
            //Sets height offset and instantiates ping
            Vector3 offset = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
            NetworkObject ping = Instantiate(basicPing, offset, Quaternion.identity);
            ping.GetComponent<NetworkObject>().Spawn();
            //ping.SpawnWithOwnership(OwnerClientId);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    void createPingServerRpc(Vector3 offset) {
        NetworkObject ping = Instantiate(basicPing, offset, Quaternion.identity);
        ping.GetComponent<NetworkObject>().Spawn();
    }
    

}
