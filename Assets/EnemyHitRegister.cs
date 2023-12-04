using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyHitRegister : NetworkBehaviour
{

    public int enemyID = -1;
    public int health;
    public int difficultyClass;
    private int[,] difficultyStats = { {1, 3, 60}, {3, 5, 60}, {7, 10, 75}, {12, 20, 100} };

    //0 is basic, 1 is easy, 2 is moderate, 3 is advanced

    [SerializeField] private bool[] hitBy = { false, false, false, false };
    [SerializeField] private int payout;
    // Start is called before the first frame update
    void Start()
    {   
        payout = calculatePayouts(difficultyStats[difficultyClass, 0], difficultyStats[difficultyClass, 1], difficultyStats[difficultyClass, 2]);
        Debug.Log("Payout: " + payout + " scrap");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N) && LobbySceneManagement.singleton.getLocalPlayer().getIsLocalPlayer())
        {
            Debug.Log("Damage timeee");
            takeDamage(25, LobbySceneManagement.singleton.getLocalPlayer().identity, "Single");
        }

        //Comment out for production
        if (EnemyWaveManager.singleton.spawnedEnemies.Count == 0) {
           EnemyWaveManager.singleton.spawnedEnemies.Add(gameObject);
        }
        
    }

    private int calculatePayouts(int low, int high, int odds) {
        float dropOdds = (float) odds * 0.01f;
        int payNum = (int) Mathf.Ceil(Random.Range((float) low - 1f, (float) high));
        float theseOdds = Random.Range(0f, 1f);
        Debug.Log(dropOdds + " " + payNum + " " + theseOdds); 
        if (theseOdds <= odds) {
            return payNum;
        } else {
            return 0;
        }
        
    }

    public void takeDamage(int damage, int playerID, string type) {
        Debug.Log("Hit by player " + playerID + " with damage type " + type + " for " + damage + " damage");
        if (type != "Sustained AOE") {
            //takeDamageServerRpc(health, damage, playerID - 1, enemyID);
            //for testing only
            takeDamageServerRpc(health, damage, playerID - 1, enemyID + 1);
        } else {
            takeDamageServerRpc(health, damage, playerID - 1, enemyID + 1);
        }
        //take damage s


    }

    [ServerRpc(RequireOwnership = false)]
    public void takeDamageServerRpc(int healthIn, int damage, int playerID, int enemID) {
        Debug.Log("take damage server rpc");
        if (IsServer) {
            if (healthIn - damage <= 0) {
                Debug.Log("Killed by player " + (playerID + 1));
                dieServerRpc(playerID, enemID);
                return;
            }

            Debug.Log("Sending hit to client");
            takeDamageClientRpc(damage, playerID, enemID);

        }
        
    }

    [ClientRpc] 
    public void takeDamageClientRpc(int damage, int playerID, int enemID){
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsClient()) {
            Debug.Log("client rpc got hit " + EnemyWaveManager.singleton.spawnedEnemies[enemID] + " " + gameObject);
            if (gameObject == EnemyWaveManager.singleton.spawnedEnemies[enemID]) {
                Debug.Log("I'm the victim! " + this);
                if (!hitBy[playerID]) {
                    hitBy[playerID] = true;
                }

                health -= damage;
                Debug.Log("Damage: " + LobbySceneManagement.singleton.statsArray[playerID, 3]);
                //Credit player for damage
                LobbySceneManagement.singleton.statsArray[playerID, 3] += damage;
                Debug.Log("Damage: " + LobbySceneManagement.singleton.statsArray[playerID, 3]);
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void dieServerRpc(int playerID, int enemID) {
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            dieClientRpc(playerID, enemID);
        }
    }

    [ClientRpc] 
    public void dieClientRpc(int playerID, int enemID){
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsClient()) {
            if (gameObject == EnemyWaveManager.singleton.spawnedEnemies[enemID]) {
                Debug.Log("I'm the dead victim! " + this);
                //credit player for remaining health as damage
                LobbySceneManagement.singleton.statsArray[playerID, 3] += health;
                health = 0;
                //credit player for kill
                Debug.Log("Player " + (playerID + 1) + " killed me");
                LobbySceneManagement.singleton.statsArray[playerID, 0]++;
                //credit all players with assists who aren't the killer with assists
                for (int i = 0; i < 4; i++) {
                    if (i != playerID && hitBy[i]) {
                        Debug.Log("assisted by player " + i);
                        LobbySceneManagement.singleton.statsArray[i, 1]++;
                    }
                }
                
                //pay player
                if (playerID == LobbySceneManagement.singleton.getLocalPlayer().identity - 1) {
                    LobbySceneManagement.singleton.playerCamObject.GetComponent<AddMoney>().pay(payout);
                    Debug.Log("Paid player " + payout + " scrap");    
                }
                
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }

    //take damage
        //log hit damage to scoreboard
        //log who dealt it in assists
        //on death, post assists and killer to scoreboard, send scrap to killer

        //write enem manager singleton script as EnemyWaveManager
}
