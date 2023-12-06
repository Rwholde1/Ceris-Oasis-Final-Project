using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float lerpSpeed = 1.0f; // Lerp speed
    public float pauseTime = 15.0f; // Pause time at each waypoint
    public float rotationSpeed = 5.0f; // Rotation speed
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private Animator animator;
    public int startpoint = 0;
    Transform poop;
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(MoveBetweenWaypoints());
        currentWaypointIndex = startpoint;
    }

    private IEnumerator MoveBetweenWaypoints()
    {
        while (true)
        {
            // Set animation parameter to true
            animator.SetBool("IsMoving", true);

            // Lerp towards the current waypoint
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;

            while (elapsedTime < 1f)
            {
                Vector3 newPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime);
                transform.position = newPosition;

                // Face the direction of movement
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                elapsedTime += Time.deltaTime * lerpSpeed;
                yield return null;
            }

            // Ensure that we reach the exact target position
            transform.position = targetPosition;

            // Set animation parameter to false after reaching the waypoint
            if (currentWaypointIndex != 1 && currentWaypointIndex != 2 && currentWaypointIndex != 7 && currentWaypointIndex != 5)
            {
                animator.SetBool("IsMoving", false);

                // Pause for a specified time at the waypoint
                yield return new WaitForSeconds(pauseTime);
            }
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the colliding object is the player
        print("Deez MFN NUTS");
        if (other.CompareTag("Player"))
        {
            poop = LobbySceneManagement.singleton.getLocalPlayerTransform();
            print("Attached To: " + other.transform.parent.name);
            print("Poop: " + poop);
            //poop = other.transform.parent;
            // Attach the player to the walker by making it a child
            poop.SetParent(transform, true);

            //LobbySceneManagement.singleton.playerCamObject.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Detach the player from the walker
            poop.SetParent(null, true);
            //LobbySceneManagement.singleton.playerCamObject.gameObject.transform.parent = null;
        }
    }
}
