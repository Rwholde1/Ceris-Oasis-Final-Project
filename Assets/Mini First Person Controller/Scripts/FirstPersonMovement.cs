using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    public Animator animator;
    public bool isMovementEnabled = true;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isMovementEnabled)
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift))
            {
                //walking forward
                animator.SetBool("W", true);
                animator.SetBool("A", false);
                animator.SetBool("S", false);
                animator.SetBool("D", false);

            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("Shift", true);
                animator.SetBool("W", true);
                animator.SetBool("A", false);
                animator.SetBool("S", false);
                animator.SetBool("D", false);
                //walking back
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("Shift", false);
                animator.SetBool("W", false);
                animator.SetBool("A", false);
                animator.SetBool("S", true);
                animator.SetBool("D", false);
                //walking back
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool("Shift", false);
                animator.SetBool("W", false);
                animator.SetBool("A", false);
                animator.SetBool("S", false);
                animator.SetBool("D", true);
                //walking right
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("Shift", false);
                animator.SetBool("W", false);
                animator.SetBool("A", true);
                animator.SetBool("S", false);
                animator.SetBool("D", false);
                //walking left
            }
            else
            {
                animator.SetBool("Shift", false);
                animator.SetBool("W", false); ;
                animator.SetBool("A", false);
                animator.SetBool("S", false);
                animator.SetBool("D", false);
            }
            // Update IsRunning from input.
            IsRunning = canRun && Input.GetKey(runningKey);

            // Get targetMovingSpeed.
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
            {
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

            // Apply movement.
            rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        }
    }
    public void SetMovementEnabled(bool isEnabled)
    {
        isMovementEnabled = isEnabled;
    }
}