using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject buttonMenuUI;
    public GameObject infoMenuUI;
    public GameObject creditsMenuUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play() {
        //isPaused = false;
        //pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        SceneManager.LoadScene("StageOne");
    }

    public void Info() {
        Debug.Log("info time");
        buttonMenuUI.SetActive(false);
        infoMenuUI.SetActive(true);
        //SceneManager.LoadScene(menuScene.name);
    }

    public void Credits() {
        Debug.Log("credits time");
        buttonMenuUI.SetActive(false);
        creditsMenuUI.SetActive(true);
        //SceneManager.LoadScene(menuScene.name);
    }

    public void Quit() {
        Debug.Log("quitting");
        Application.Quit();
    }

    public void Return() {
        infoMenuUI.SetActive(false);
        creditsMenuUI.SetActive(false);
        buttonMenuUI.SetActive(true);
    }
}
