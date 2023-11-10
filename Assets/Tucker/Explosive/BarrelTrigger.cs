using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour
{

    public bool hasExploded = false;
    public bool shouldExplode = false;
    [SerializeField] float explRadius = 2;
    [SerializeField] float recursiveDelay = 0.3f;
    Collider[] colliders = new Collider[100];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void explode() {
        hasExploded = true;
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, explRadius, colliders);
        for (int i = 0; i < numColliders; i++) {
            //chain effect to trigger other bombs
            string colName = colliders[i].gameObject.name;
            if (colName.Length >= 16) {
                if (colName.Substring(0,16).Equals("Explosive Object")) {
                    GameObject target = colliders[i].gameObject;
                    var otherScript = target.GetComponent<ExplosionController>();
                    if (otherScript.hasExploded == false) {
                        otherScript.Invoke("explode", recursiveDelay);
                    }
                }
            }
        }
    }
}
