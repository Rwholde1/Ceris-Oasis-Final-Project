using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpType : MonoBehaviour
{
    [SerializeField] private string powerUpName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getPowerUpName()
    {
        return powerUpName;
    }
}
