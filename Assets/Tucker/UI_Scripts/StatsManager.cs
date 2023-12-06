using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool isOpen = false;
    public GameObject StatsUI;
    public GameObject HUDUI;
    public GameObject pauseMenuUI;
    public int [,] playerStats = {{0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}};
    //private TextMeshProUGUI[,] playerStatsTex;

    [SerializeField] TextMeshProUGUI player1Elims;
    [SerializeField] TextMeshProUGUI player1Assists;
    [SerializeField] TextMeshProUGUI player1Deaths;
    [SerializeField] TextMeshProUGUI player1Damage;

    [SerializeField] TextMeshProUGUI player2Elims;
    [SerializeField] TextMeshProUGUI player2Assists;
    [SerializeField] TextMeshProUGUI player2Deaths;
    [SerializeField] TextMeshProUGUI player2Damage;

    [SerializeField] TextMeshProUGUI player3Elims;
    [SerializeField] TextMeshProUGUI player3Assists;
    [SerializeField] TextMeshProUGUI player3Deaths;
    [SerializeField] TextMeshProUGUI player3Damage;

    [SerializeField] TextMeshProUGUI player4Elims;
    [SerializeField] TextMeshProUGUI player4Assists;
    [SerializeField] TextMeshProUGUI player4Deaths;
    [SerializeField] TextMeshProUGUI player4Damage;

    public Image[] playerIcons = new Image[4];
    public Sprite[] charSprites = new Sprite[4];
    public TMP_Text[] playerNames = new TMP_Text[4];

    public Image localClassIcon;

    void Start() {
        /*
        TextMeshProUGUI[] transforms = StatsUI.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI trans in transforms) {
            Debug.Log(trans);
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        //Checks that user isn't in pause menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                isPaused = false;
            } else {
                isPaused = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !pauseMenuUI.active && LobbySceneManagement.singleton.getLocalPlayer().gameObject.GetComponentInChildren<FirstPersonMovement>().isMovementEnabled) {
            open();
        } 
        if (Input.GetKeyUp(KeyCode.Tab) && !pauseMenuUI.active) {
            close();
        } 
        if (pauseMenuUI.active) {
            close();
            HUDUI.SetActive(false);
            //Time.timeScale = 0f;
        }

        player1Elims.text = playerStats[0, 0] + "";
        player1Assists.text = playerStats[0, 1] + "";
        player1Deaths.text = playerStats[0, 2] + "";
        player1Damage.text = playerStats[0, 3] + "";

        player2Elims.text = playerStats[1, 0] + "";
        player2Assists.text = playerStats[1, 1] + "";
        player2Deaths.text = playerStats[1, 2] + "";
        player2Damage.text = playerStats[1, 3] + "";
        
        player3Elims.text = playerStats[2, 0] + "";
        player3Assists.text = playerStats[2, 1] + "";
        player3Deaths.text = playerStats[2, 2] + "";
        player3Damage.text = playerStats[2, 3] + "";
        
        player4Elims.text = playerStats[3, 0] + "";
        player4Assists.text = playerStats[3, 1] + "";
        player4Deaths.text = playerStats[3, 2] + "";
        player4Damage.text = playerStats[3, 3] + "";
        
    }

    void open() {
        isOpen = true;
        StatsUI.SetActive(true);
        HUDUI.SetActive(false);
    }

    void close() {
        isOpen = false;
        StatsUI.SetActive(false);
        HUDUI.SetActive(true);
    }

    //Increments given player's elim counter
    public void addElim(int player) {
        playerStats[player - 1, 0]++;
    }

    //Increments given player's assist counter
    public void addAssist(int player) {
        playerStats[player - 1, 1]++;
    }

    //Increments given player's death counter
    public void addDeath(int player) {
        playerStats[player - 1, 2]++;
    }

    //Adds damage dealt to given player's damage counter
    public void addDamage(int player, int damageIn) {
        playerStats[player - 1, 3] += damageIn;
    }

    public void setSprite(int charId, int playerId) {
        Debug.Log("setting icon");
        playerIcons[playerId].sprite = charSprites[charId];
        if (playerId == LobbySceneManagement.singleton.getLocalPlayer().identity - 1) {
            localClassIcon.sprite = charSprites[charId];
        }
    }
}