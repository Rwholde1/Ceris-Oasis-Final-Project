using UnityEngine;

public class Jetpack : MonoBehaviour
{
    public float thrustForce = 10f;         // The force applied when using the jetpack
    public float maxFuel = 100f;            // Maximum fuel for the jetpack
    public float fuelConsumptionRate = 5f;  // Rate at which fuel is consumed
    private KeyCode jetpackKey = KeyCode.Space; // Key to trigger the jetpack
    public GameObject jets;
    private float currentFuel;              // Current fuel level
    private bool isUsingJetpack;            // Flag to check if the jetpack is being used

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;
        jets.SetActive(false);
    }

    void Update()
    {
        // Check for input to trigger the jetpack
        if (Input.GetKeyDown(jetpackKey) && currentFuel > 0)
        {
            isUsingJetpack = true;
            
        }

        if (Input.GetKeyUp(jetpackKey) || currentFuel <= 0)
        {
            isUsingJetpack = false;
        }
        jets.SetActive(isUsingJetpack);
    }

    void FixedUpdate()
    {
        // Apply force if the jetpack is being used and there is fuel
        if (isUsingJetpack && currentFuel > 0)
        {
            // Apply force in the upward direction
            rb.AddForce(Vector3.up * thrustForce, ForceMode.Force);

            // Consume fuel
            currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;
            print("FUEL:" + currentFuel);
        }
    }
}
