using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistAnimations : MonoBehaviour
{
    private int TransitionTo;
    private int upperbound = 4;
    private int lowerbound = 1;
    [SerializeField] private Animator animator;
    //1-6
    // Update is called once per frame
    void FixedUpdate()
    {
        TransitionTo = Random.Range(lowerbound, upperbound);
        animator.SetInteger("TransitionTo", TransitionTo);
    }
}
