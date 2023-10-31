using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoveTowardMiddle : MonoBehaviour
{
    public float forwardSpeed = 10.0f; // Speed at which the boomerang moves forward
    public float returnSpeed = 15.0f;  // Speed at which the boomerang returns
    public float flightDuration = 3.0f; // Total duration of boomerang flight
    public float damage = 5.0f;


    private bool isFlyingForward = false;
    private bool hasCollided = false;
    [SerializeField] private Transform playerposition;
    private Vector3 centerOfScreen; // Center of the screen in world space
    private bool CanShoot = true;
    [SerializeField] private Transform camposition;
    [SerializeField] private GameObject childBoom;
    private PowerUpType PowerUp;
    private string PowerUpName;
    private DropSystem eAt;

    private void Start()
    {
        centerOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 30));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFlyingForward && !hasCollided && CanShoot)
        {
            centerOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 30));
            print(centerOfScreen.ToString());
            // Start moving the boomerang forward
            childBoom.SetActive(true);
            childBoom.transform.position = (new Vector3(playerposition.position.x, playerposition.position.y, playerposition.position.z));
            isFlyingForward = true;
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,-camposition.rotation.eulerAngles.x);

            StartCoroutine(FlyForward());

        }
        if (hasCollided && CanShoot)
        {

            transform.position = (new Vector3(playerposition.position.x, playerposition.position.y, playerposition.position.z));
                hasCollided = false;
                CanShoot = true;
                isFlyingForward = false;

        }

        if (!isFlyingForward && !hasCollided && CanShoot)
        {
            transform.position = new Vector3(playerposition.position.x, playerposition.position.y + 0.4f, playerposition.position.z);
        }

    }

    public void AttackRateIncrease(float increase)
    {
        forwardSpeed += increase;
        returnSpeed += increase;
        if (flightDuration > 0.5f)
        {
            flightDuration -= (increase / (increase * 10));
        } 
    }

    private IEnumerator FlyForward()
    {
        CanShoot = false;
        float elapsedTime = 0.0f;

        while (elapsedTime < flightDuration)
        {
            Vector3 direction = centerOfScreen - transform.position;
            direction.Normalize();
            transform.Translate(direction * forwardSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;

            // Check for collisions with objects
            if (hasCollided)
            {
                // If a collision occurs, break the loop and start returning
                break;
            }
        }

        // If no collision occurred during the forward flight, start returning
        StartCoroutine(ReturnBoomerang());
    }

    private IEnumerator ReturnBoomerang()
    {


        while (Vector3.Distance(transform.position, (new Vector3(playerposition.position.x, playerposition.position.y + 1f, playerposition.position.z))) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position,(new Vector3(playerposition.position.x, playerposition.position.y + 1f, playerposition.position.z)), returnSpeed * Time.deltaTime);
            yield return null;
        }
        CanShoot = true;
        isFlyingForward = false;
        hasCollided = false;
        // Reset position and flags
    }

    // Handle collisions with objects
    private void OnTriggerEnter(Collider collider)
    {
        if (!CanShoot && !hasCollided && collider.gameObject.tag != "Player")
        {
            if(collider.gameObject.tag == "Enemy")
            {
                eAt = collider.gameObject.GetComponent<DropSystem>();
                eAt.Damage(damage);
            }    
            Debug.Log("Collision detected with " + collider.gameObject.name);
            hasCollided = true;
        }
        if(collider.gameObject.tag == "Player")
        {
            childBoom.SetActive(false);
                
        }
        if (CanShoot && collider.gameObject.tag == "PowerUp")
        {
            Debug.Log("Collision detected with " + collider.gameObject.name);
            PowerUp = collider.gameObject.GetComponent<PowerUpType>();
            PowerUpName = PowerUp.getPowerUpName();
            switch (PowerUpName)
            {
                case "AttackRate":
                    float ARincrease = 5f;
                    forwardSpeed += ARincrease;
                    returnSpeed += ARincrease;
                    if (flightDuration > 0.5f)
                    {
                        flightDuration -= 0.75f;
                    }
                    Destroy(collider.gameObject);
                    break;
                case "DamageUp":
                    float DMGincrease = 5f;
                    damage += DMGincrease;
                    Destroy(collider.gameObject);
                    break;
                default: 
                    break;
            }
        }
    }
}
