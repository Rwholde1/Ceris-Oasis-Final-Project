using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject dropdownHolder;
    public Image levelImage;
    public TMP_Text levelString;
    public TMP_Text titleText;

    public Sprite[] levelImages;
    public string[] levelDescriptions;
    public string levelName = "Urban";

    private bool settled = false;
    // Start is called before the first frame update
    void Start()
    {
        //changeLevel();
    }

    void Awake() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbySceneManagement.singleton.levelSelector == null) {
            LobbySceneManagement.singleton.levelSelector = this;
        }

        if (!settled) {
            if (LobbySceneManagement.singleton != null) {
                //Debug.Log("settling");
                if (LobbySceneManagement.singleton.getLocalPlayer().getIsHost()) {
                    titleText.enabled = false;
                } else {
                    dropdownHolder.SetActive(false);
                }
                //changeLevel();
                settled = true;
            }
        }
        
    }

    public void changeLevel() {
        //Debug.Log("Changing text to: " + text);
        //Debug.Log("Dropdown val: " + dropdown.value);
        switch (dropdown.value) {
            case 0:
                //levelName = "Urban";
                levelName = "Epsilon";
                break;
            case 1:
                levelName = "Skyline";
                break;
            case 2:
                levelName = "Pherris Reactor";
                //levelName = "GunTest";
                break;
            default:
                levelName = "Epsilon";
                break;
        }
        LobbySceneManagement.singleton.SceneToPlay = levelName;
        LobbySceneManagement.singleton.updateSelectedLevel();
        //updateLevelStuff();
        

    }

    public void updateLevelStuff(string nameIn) {
        //Debug.Log("updating level to " + nameIn);
        levelName = nameIn;
        LobbySceneManagement.singleton.SceneToPlay = nameIn;
        LobbySceneManagement.singleton.LobbyHeader.SetText(nameIn);
        titleText.SetText(levelName);
        switch (levelName) {
            case "Epsilon": 
                levelImage.sprite = levelImages[0];
                levelString.SetText(levelDescriptions[0]);
                break;
            case "Skyline": 
                levelImage.sprite = levelImages[1];
                levelString.SetText(levelDescriptions[1]);
                break;
            case "Pherris Reactor": 
                levelImage.sprite = levelImages[2];
                levelString.SetText(levelDescriptions[2]);
                break;
            case "GunTest":
                levelImage.sprite = levelImages[0];
                levelString.SetText("Gun Test Scene Description");
                break;
            default: 
                levelImage.sprite = levelImages[0];
                levelString.SetText(levelDescriptions[0]);
                break;
        }
    }
}
