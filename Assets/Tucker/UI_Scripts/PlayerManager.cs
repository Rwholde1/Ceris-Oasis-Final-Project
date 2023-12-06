using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerManager : NetworkBehaviour
{

    [SerializeField] public int maxHealth = 1200;
    public int currentHealth;

    [SerializeField] int cooldown1 = 10;
    [SerializeField] int cooldown2 = 10;

    public bool alive = true;
    [SerializeField] object primaryWeapon;
    [SerializeField] object secondaryWeapon;
    public HealthBar healthBar;
    public AbilityManager abilities;
    public StatsManager stats;

    public Camera mainCam;
    public NetworkObject basicPing;

    public TMP_Text ammoText;
    public Image crosshair;

    public bool canDamage = true;
    public float invincibleCooldown = 0.5f;

    //grenades
    //shielding?


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        //abilities.setCooldown1(cooldown1);
        abilities.setCooldown2(cooldown2);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && alive == true) {
            alive = false;
            LobbySceneManagement.singleton.getLocalPlayer().playerDies();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stats.addDeath(1);
            currentHealth = maxHealth;
            healthBar.setHealth(maxHealth);
        }

        /*
        if(Input.GetKeyDown(KeyCode.J)) {
            stats.addElim(1);
        }
        if(Input.GetKeyDown(KeyCode.K)) {
            stats.addAssist(1);
        }*/
        if(Input.GetKeyDown(KeyCode.L)) {
            takeDamage(10, LobbySceneManagement.singleton.getLocalPlayer().identity);
        }/*
        if(Input.GetMouseButtonDown(1)) {
            var dmg = Mathf.Floor(GetComponent<Transform>().localEulerAngles.y);
            stats.addDamage(1, (int) dmg);
        }*/

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

    /*
    public void takeDamage(int damageIn) {
        currentHealth -= damageIn;
        healthBar.setHealth(currentHealth);
    }*/

    public void receiveHeals(int healthIn) {
        if (currentHealth + healthIn > maxHealth) {
            currentHealth = maxHealth;
            healthBar.setHealth(currentHealth);
        } else {
            currentHealth += healthIn;
            healthBar.setHealth(currentHealth);
        }
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



    public void receiveHealth(int healthIn, int playerID) {
        Debug.Log("Hit player " + playerID + " for " + healthIn + " health");

        //takeDamageServerRpc(health, damage, playerID - 1, enemyID);
        //for testing only
        receiveHealthServerRpc(healthIn, playerID - 1);
    }


    [ServerRpc(RequireOwnership = false)]
    public void receiveHealthServerRpc(int healthIn, int playerID) {
        Debug.Log("heals server rpc");
        if (IsServer) {

            Debug.Log("Sending health to client");
            receiveHealthClientRpc(healthIn, playerID);

        }
        
    }

    [ClientRpc] 
    public void receiveHealthClientRpc(int healthIn, int playerID){
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsClient()) {
            //Debug.Log("client rpc got hit " + EnemyWaveManager.singleton.spawnedEnemies[enemID] + " " + gameObject);
            
            //if (gameObject == LobbySceneManagement.singleton.players[playerID].GetComponent<PlayerManager>()) {
            if (LobbySceneManagement.singleton.players[playerID].transform == GetComponent<FirstPersonLook>().character && canDamage) {
                Debug.Log("I'm the victim of health! " + this + " " + healthIn);

                Debug.Log("current: " + currentHealth);
                currentHealth += healthIn;
                Debug.Log("New: " + currentHealth);
                if (currentHealth > maxHealth) {
                    currentHealth = maxHealth;
                }
                healthBar.setHealth(currentHealth);
                //health -= damage;
                //Debug.Log("Damage: " + LobbySceneManagement.singleton.statsArray[playerID, 3]);
                //Credit player for damage
                //LobbySceneManagement.singleton.statsArray[playerID, 3] += damage;
                //Debug.Log("Damage: " + LobbySceneManagement.singleton.statsArray[playerID, 3]);
            }
        }
    }
    
    public void takeDamage(int damageIn, int playerID) {
        Debug.Log("Hit player " + playerID + " for " + damageIn + " damage");

        //takeDamageServerRpc(health, damage, playerID - 1, enemyID);
        //for testing only
        takeDamageServerRpc(damageIn, playerID - 1);
    }


    [ServerRpc(RequireOwnership = false)]
    public void takeDamageServerRpc(int damageIn, int playerID) {
        Debug.Log("player damage server rpc");
        if (IsServer) {

            Debug.Log("Sending damage to client");
            takeDamageClientRpc(damageIn, playerID);

        }
        
    }

    [ClientRpc] 
    public void takeDamageClientRpc(int damageIn, int playerID){
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsClient()) {
            //Debug.Log("client rpc got hit " + EnemyWaveManager.singleton.spawnedEnemies[enemID] + " " + gameObject);
            
            //if (gameObject == LobbySceneManagement.singleton.players[playerID].GetComponent<PlayerManager>()) {

            //FIX CONDITION?
            Debug.Log("damage id in is " + playerID);
            Debug.Log("checking damage identity " + LobbySceneManagement.singleton.players[playerID].transform + " against " + GetComponent<FirstPersonLook>().character);
            if (LobbySceneManagement.singleton.players[playerID].transform == GetComponent<FirstPersonLook>().character && canDamage) {
                Debug.Log("I'm the player victim! " + this + " " + damageIn);
                if (damageIn >= currentHealth) {
                    currentHealth = 0;
                    playerDieServerRpc(playerID);
                    return;
                }
                Debug.Log("current: " + currentHealth);
                currentHealth -= damageIn;
                Debug.Log("New: " + currentHealth);
                if (currentHealth < 0) {
                    currentHealth = 0;
                }
                healthBar.setHealth(currentHealth);
                StartCoroutine(InvincibleTime());
                //health -= damage;
                //Debug.Log("Damage: " + LobbySceneManagement.singleton.statsArray[playerID, 3]);
                //Credit player for damage
                //LobbySceneManagement.singleton.statsArray[playerID, 3] += damage;
                //Debug.Log("Damage: " + LobbySceneManagement.singleton.statsArray[playerID, 3]);
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void playerDieServerRpc(int playerID) {
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            Debug.Log("Player died on server");
            playerDieClientRpc(playerID);
        }
    }

    [ClientRpc] 
    public void playerDieClientRpc(int playerID){
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsClient()) {
            LobbySceneManagement.singleton.statsArray[playerID, 2]++;
            if (LobbySceneManagement.singleton.players[playerID].transform == GetComponent<FirstPersonLook>().character && canDamage) {
                Debug.Log("I'm the dead victim! " + this);
                //credit player for death
                //LobbySceneManagement.singleton.statsArray[playerID, 2]++;

            }
        }
    }

    IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(invincibleCooldown);
        canDamage = true;
    }

}
