using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLobbyMusic : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    void Start()
    {
        musicSource.Play();
    }
}
