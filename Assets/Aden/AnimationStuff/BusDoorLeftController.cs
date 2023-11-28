using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorLeftController : MonoBehaviour
{
    [SerializeField] private Animator leftDoor = null;

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!openTrigger)
            {
                leftDoor.Play("DoorOpen",0,0.0f);
                gameObject.SetActive(false);
            }

            else if(closeTrigger)
            {
                leftDoor.Play("DoorClose", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}
