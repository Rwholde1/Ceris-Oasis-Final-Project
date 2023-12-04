using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Unity.Netcode;

public class InGameMusic : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioClip[] musicClip;
    public static InGameMusic singleton = null;
    int prevSong;
    int randSongIndex;

    public bool gameSongs = false;
    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (singleton != this)
        {
            Debug.Log(singleton.name + " replaced me");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = musicClip[0];
        musicSource.Play();

        SceneManager.activeSceneChanged += changeSceneMusic;
    }

    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.N))
        {
            musicSource.Stop();
        }*/
        
        if(!musicSource.isPlaying)
        {
            prevSong = randSongIndex;
            while(prevSong == randSongIndex)
            {
                randSongIndex = (int)Random.Range(1, musicClip.Length);
            }
            musicSource.clip = musicClip[randSongIndex];
            musicSource.Play();
        }

    }

    void changeSceneMusic(Scene priorScene, Scene scene) {
        //Scene scene = SceneManager.GetActiveScene(Scene scene);
        Debug.Log("OnSceneLoaded: " + scene.name);

        if (scene.name != "LobbyScene" && scene.name != "MainMenu" && !gameSongs) {
            StartGameMusic();
        }
    }

    public void StartGameMusic()
    {
        gameSongs = true;
        randSongIndex = (int)Random.Range(1, musicClip.Length);
        musicSource.clip = musicClip[randSongIndex];
        musicSource.Play();
        musicSource.loop = false;
    }
}
