using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boomerangController;
    private MoveTowardMiddle boomerangScript;
    private vThirdPersonController playerMovement;
    private bool canSlowTime = true;

    // Start is called before the first frame update
    void Start()
    {
        boomerangScript = boomerangController.GetComponent<MoveTowardMiddle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSlowTime && Input.GetKeyDown(KeyCode.E))
        {
            canSlowTime = false;
            Time.timeScale = 0.2f;
            StartCoroutine(ResetTime());
        }
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(7f);
        canSlowTime = true;
    }
}
