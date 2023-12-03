using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
using Unity.Netcode;

public class FirstPersonMovement : NetworkBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    public Animator animator;
    public float spawnRadius = 2f;
    //private Rigidbody rig;
    public bool isMovementEnabled = true;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        if(!LobbySceneManagement.singleton.getLocalPlayer().getIsLocalPlayer()) {
            //Debug.Log("isn't local player");
            return;
        }
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        Transform thisTransform = GetComponent<Transform>();
        Debug.Log("Initial position: " + thisTransform.position);
        thisTransform.position = Vector3.Scale(new Vector3(1f, 0f, 1f), thisTransform.position);
        thisTransform.position += new Vector3(Random.Range(-spawnRadius, spawnRadius), 1.488f, Random.Range(-spawnRadius, spawnRadius));
        Debug.Log("New position: " + thisTransform.position);
    }

    void FixedUpdate()
    {   
        if(!LobbySceneManagement.singleton.getLocalPlayer().getIsLocalPlayer() || !isMovementEnabled) {
            //Debug.Log("isn't local player");
            return;
        }
        //Debug.Log("is local player");
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift))
        {
            //walking forward
            animator.SetBool("W", true);
            animator.SetBool("A", false);
            animator.SetBool("S", false);
            animator.SetBool("D", false);
            //Debug.Log("walking forward");

        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Shift", true);
            animator.SetBool("W", true);
            animator.SetBool("A", false);
            animator.SetBool("S", false);
            animator.SetBool("D", false);
            //sprinting
            //Debug.Log("sprinting");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Shift", false);
            animator.SetBool("W", false);
            animator.SetBool("A", false);
            animator.SetBool("S", true);
            animator.SetBool("D", false);
            //walking back
            //Debug.Log("walking back");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Shift", false);
            animator.SetBool("W", false);
            animator.SetBool("A", false);
            animator.SetBool("S", false);
            animator.SetBool("D", true);
            //walking right
            //Debug.Log("walking right");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("Shift", false);
            animator.SetBool("W", false);
            animator.SetBool("A", true);
            animator.SetBool("S", false);
            animator.SetBool("D", false);
            //walking left
            //Debug.Log("walking left");
        }
        else
        {
            animator.SetBool("Shift", false);
            animator.SetBool("W", false); ;
            animator.SetBool("A", false);
            animator.SetBool("S", false);
            animator.SetBool("D", false);
            //Debug.Log("not moving");
        }
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);
        /*
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
        }*/

        /*
        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        if (rig == null) {
            rig = gameObject.GetComponent<Rigidbody>();
        }
        //Debug.Log(gameObject);
        //Debug.Log(rig.velocity);
        Debug.Log(transform.rotation);
        Debug.Log("y: " + targetVelocity.x);
        Debug.Log("x: " + targetVelocity.y);
        

        rig.velocity = transform.rotation * new Vector3(targetVelocity.x, rig.velocity.y, targetVelocity.y);
        */
        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        Debug.Log("Movement Data:");
        Debug.Log(Input.GetAxis("Horizontal") + " " + Input.GetAxis("Vertical"));
        Debug.Log(runSpeed + " " + speed);
        // Apply movement.
        Rigidbody rig = gameObject.GetComponent<Rigidbody>();
        rig.velocity = transform.rotation * new Vector3(targetVelocity.x, rig.velocity.y, targetVelocity.y);
        Debug.Log(rig.velocity);
    }
    
    public void SetMovementEnabled(bool isEnabled)
    {
        isMovementEnabled = isEnabled;
    }
}