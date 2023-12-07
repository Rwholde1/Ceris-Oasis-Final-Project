using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsToggleScript : MonoBehaviour
{
    private bool isenabled = false;
    [SerializeField] private GameObject text;
    public void Toggle()
    {
        isenabled = !isenabled; 
        text.SetActive(isenabled);
    }
}
