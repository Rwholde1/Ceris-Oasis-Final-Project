using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2f;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    public Camera thisPingCam;
    private bool lockedIn = false;
    //public float spawnRadius = 2f;
    public Animator thisAnim;

    public Camera minimapCam;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        //character = GetComponentInParent<FirstPersonMovement>().transform;
        //character = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<Transform>();
        //character = NetworkManager.LocalClient.PlayerObject;
        //character = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Transform>();
        //character = LobbySceneManagement.singleton.getLocalPlayerTransform();
        //Debug.Log("Character transform: " + character);
        //Debug.Log("Initial position: " + character.transform.position + " New Position: " + Vector3.Scale(new Vector3(1f, 0f, 1f), character.transform.position));
        //transform.position = Vector3.Scale(new Vector3(1f, 0f, 1f), character.transform.position);

    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
        //character = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<Transform>();
        //character = NetworkManager.LocalClient.PlayerObject;
        var ThisChar = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        ThisChar.GetComponent<FPCOnSceneEnter>().pingCam = thisPingCam;
        //ThisChar.GetComponent<FirstPersonMovement>().animator = thisAnim;

        //character = ThisChar.GetComponent<Transform>();

        Debug.Log("deleting meshes");
        SkinnedMeshRenderer[] theseMeshes = ThisChar.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer thisMesh in theseMeshes) {
            //Debug.Log("go " + thisMesh.gameObject);
            //Debug.Log("layer is: " + thisMesh.gameObject.layer);
            if (thisMesh.gameObject.layer != 30) {
                Debug.Log(thisMesh + " deleted");
                thisMesh.enabled = false;
            }
            
        }
        MeshRenderer[] thoseMeshes = ThisChar.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer thisMesh in thoseMeshes) {
            //Debug.Log("layer is: " + thisMesh.gameObject.layer);
            if (thisMesh.gameObject.layer != 30) {
                Debug.Log(thisMesh + " deleted");
                thisMesh.enabled = false;
            }
        }


    }

    void Update()
    {

        if (!lockedIn) {
            //Normal case
            character = LobbySceneManagement.singleton.getLocalPlayerTransform();
            if (SceneManager.GetActiveScene().name == "GunTest") {
                Debug.Log("In Gun Test");
                character = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
            if (character != null && LobbySceneManagement.singleton.playerSpawnZone != null) {
                Debug.Log("Character transform: " + character);
                Debug.Log("Initial position: " + character.transform.position);
                //character.transform.position = Vector3.Scale(new Vector3(1f, 0f, 1f), character.transform.position);
                character.transform.position = LobbySceneManagement.singleton.playerSpawnZone.position;
                float spawnRadius = LobbySceneManagement.singleton.playerSpawnZoneRadius;
                character.transform.position += new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.5f, Random.Range(-spawnRadius, spawnRadius));
                Debug.Log("New position: " + character.transform.position);
                character.GetComponentInParent<FirstPersonMovement>().enabled = true;
                Debug.Log("FPM Script enabled: " + character.GetComponentInParent<FirstPersonMovement>().enabled);

                lockedIn = true;
            }
            
            Debug.Log("Fetching character");
        }

        /*
        if (LobbySceneManagement.singleton.playerCamObject == null) {
            LobbySceneManagement.singleton.playerCamObject = GetComponent<GameObject>();
        }*/

        //if (character.GetC)
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);


        transform.position = character.transform.position;
        transform.position += new Vector3(0f, 1.488f, 0f);


        // Rotate camera up-down and controller left-right from velocity.
        //Debug.Log("updated mouse look " + velocity.y);

        //transform.rotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        //transform.rotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        transform.eulerAngles = new Vector3(-velocity.y, velocity.x, 0f);

        //character.localRotation = Quaternion.AngleAxis( velocity.y, Vector3.right);

        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        if (minimapCam != null) {
            //minimapCam.GetComponent<Transform>().rotation = Quaternion.identity;
            //minimapCam.GetComponent<Transform>().eulerAngles = new Vector3(90f, velocity.x, 0f);
            Vector3 newPos = LobbySceneManagement.singleton.getLocalPlayerTransform().position;
            newPos.y = minimapCam.GetComponent<Transform>().transform.position.y;
            minimapCam.GetComponent<Transform>().transform.position = newPos;

            minimapCam.GetComponent<Transform>().transform.rotation = Quaternion.Euler(90f, LobbySceneManagement.singleton.getLocalPlayerTransform().eulerAngles.y, 0f);
        }


    }

    void LateUpdate() {
        if (minimapCam != null) {
            //minimapCam.GetComponent<Transform>().rotation = Quaternion.identity;
            //minimapCam.GetComponent<Transform>().eulerAngles = new Vector3(90f, velocity.x, 0f);
        }
    }
}
