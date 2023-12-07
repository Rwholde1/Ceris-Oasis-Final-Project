using UnityEngine;
using Unity.Netcode;

public class Jetpack : NetworkBehaviour
{
    public float thrustForce = 10f;         // The force applied when using the jetpack
    public float maxFuel = 100f;            // Maximum fuel for the jetpack
    public float fuelConsumptionRate = 5f;  // Rate at which fuel is consumed
    private KeyCode jetpackKey = KeyCode.Space; // Key to trigger the jetpack
    public GameObject jets;
    public float currentFuel;              // Current fuel level
    private bool isUsingJetpack;            // Flag to check if the jetpack is being used
    public float timetowait;
    public float refillrate;
    private Rigidbody rb;
    private bool hasusedjetpack = true;
    private float timeSinceLastUse;
    [SerializeField] GroundCheck groundCheck;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;
        jets.SetActive(false);
    }

    void Update()
    {
        // Check for input to trigger the jetpack
        if (Input.GetKeyDown(jetpackKey) && currentFuel > 0 && LobbySceneManagement.singleton.getLocalPlayer().GetComponent<FirstPersonMovement>().isMovementEnabled == true)
        {
            isUsingJetpack = true;
        }

        if (Input.GetKeyUp(jetpackKey) || currentFuel <= 0)
        {
            isUsingJetpack = false;
        }
        setJetsActiveServerRpc(isUsingJetpack);

        timeSinceLastUse += Time.deltaTime;
        if(!groundCheck.isGrounded) timeSinceLastUse = 0f;
        if (!isUsingJetpack && timeSinceLastUse > timetowait && currentFuel < maxFuel)
        {
            currentFuel += refillrate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            //print("REFUEL:" + currentFuel);

        }
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
            //print("FUEL:" + currentFuel);
            hasusedjetpack = true;
        }

        if (currentFuel >= 100) hasusedjetpack = true;

        if (currentFuel <100 && !hasusedjetpack)
        {
            //lerpin

        }

        Checktime();
    }

    private void Checktime()
    {

        if (currentFuel >= 100) return;
        if(hasusedjetpack)
        {

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void setJetsActiveServerRpc(bool isJetpackOn) {
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            //Debug.Log("setting jets to " + isJetpackOn);
            setJetsActiveClientRpc(isJetpackOn);
        }
    }

    [ClientRpc]
    public void setJetsActiveClientRpc(bool isJetpackOn) {
        //Debug.Log("turning jetpack status " + isJetpackOn);
        jets.SetActive(isJetpackOn);
    }
}
